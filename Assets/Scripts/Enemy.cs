using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    Rigidbody rb;
    NavMeshAgent agent;
    public GameObject coinPrefab;
    private GameObject player;
    float colorHoldTime = 0f;
    float snapToGroundIgnoreTime = 0f;
    float groundY = 0f;

    // Auto attack
    private float nextAutoAttackTime = 0f;
    public float autoAttackDuration = 2f; // seconds
    public float autoAttackDamage = 20f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Prevent auto hit at the start of the game
        nextAutoAttackTime = Time.time + autoAttackDuration;

        ++player.GetComponent<WaveManager>().currentEnemyCount;
    }

    void FixedUpdate() {
        if (!agent.enabled) {
            if (Time.time < snapToGroundIgnoreTime) {
                rb.useGravity = false;
                //rb.velocity += new Vector3(0, 4f * Time.fixedDeltaTime, 0);
                //rb.AddForce(transform.up * 2, ForceMode.VelocityChange);
                return;
            }
            if (transform.position.y < groundY) {
                // Stop from falling into the ground
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }
            if (rb.velocity.y < 0) {
                // Fall faster
                rb.velocity += -transform.up * 20 * Time.fixedDeltaTime;
            }
            rb.useGravity = true;
            if (Math.Abs(transform.position.y - groundY) < 3f) {
                // Re-enable movement once enemy falls back to the ground
                agent.enabled = true;
            }
            return; // Don't move while airborne
        }
        // Stop enemy if it gets too close or too far from player
        agent.isStopped = agent.remainingDistance <= 3f || agent.remainingDistance >= 15f;

        // Auto attack the player
        if (Time.time >= nextAutoAttackTime && agent.remainingDistance <= 3f) {
            GameObject.Find("2D Health Slider").GetComponent<Health>().takeDamage(autoAttackDamage);
            nextAutoAttackTime = Time.time + autoAttackDuration;
        }
        // Look at the player and move toward them
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        agent.SetDestination(player.transform.position);
    }

    void Update() {
        if (Time.time >= colorHoldTime) {
            // Reset color after current time exceeds color hold time
            transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
    }

    public void takeDamage(float value) {
        // Flash red
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        colorHoldTime = Time.time + .4f; // Hold for 0.4 seconds

        // Reduce health
        Slider health = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        health.value -= value;

        if (health.value <= 0f) {
            // Out of health -> die
            Destroy(gameObject);

            // Spawn a coin
            Instantiate(coinPrefab, transform.position, transform.rotation);

            // Unity waits until the next frame to remove the object from
            // tags, so having a seperate enemy count is necessary
            WaveManager wm = player.GetComponent<WaveManager>();

            if (--wm.currentEnemyCount == 0) {
                // Spawn next wave
                player.GetComponent<WaveManager>().spawnNextWave();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Whirlwind") {
            return;
        }
        takeDamage(50);
        agent.enabled = false;
        groundY = transform.position.y;

        // Don't snap back to ground until half a second passes
        snapToGroundIgnoreTime = Time.time + .5f;

        //rb.velocity = new Vector3(0, 16f, 0);
        rb.AddForce(transform.up * 32, ForceMode.VelocityChange);
    }
}
