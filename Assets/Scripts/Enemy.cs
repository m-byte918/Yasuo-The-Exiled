using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    Rigidbody rb;
    NavMeshAgent agent;
    Transform playerTransform;
    float colorHoldTime = 0f;
    float snapToGroundIgnoreTime = 0f;
    float groundY = 0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (Time.time >= colorHoldTime) {
            // Reset color after current time exceeds color hold time
            transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
        if (!agent.enabled) {
            if (Time.time < snapToGroundIgnoreTime) {
                rb.useGravity = false;
                return;
            }
            if (transform.position.y < groundY) {
                // Stop from falling into the ground
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }
            if (rb.velocity.y < 0) {
                // Fall faster
                rb.velocity += -transform.up * 20 * Time.deltaTime;
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

        // Look at the player and move toward them
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        agent.SetDestination(playerTransform.position);
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

        rb.AddForce(transform.up * 75, ForceMode.Impulse);
    }
}
