﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour {
    Rigidbody rb;
    NavMeshAgent agent;
    //public GameObject coinPrefab;
    private GameObject player;
    private Animator anim;
    float colorHoldTime = 0f;
    float snapToGroundIgnoreTime = 0f;
    float groundY = 0f;

    // Game End Things
    public GameObject Fade;
    private Scene currentScene;
    String sceneName;

    // Auto attack
    private float nextAutoAttackTime = 0f;
    public float autoAttackDuration = 2f; // seconds
    public float autoAttackDamage = 20f;

    void Start() {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        //groundY = transform.position.y;
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());

        // Prevent auto hit at the start of the game
        nextAutoAttackTime = Time.time + autoAttackDuration;

        ++GameObject.Find("Waves").GetComponent<WaveManager>().currentEnemyCount;
    }

    void FixedUpdate() {
        if (!agent.enabled) {
            if (Time.time < snapToGroundIgnoreTime) {
                //rb.useGravity = false;
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                //rb.velocity += new Vector3(0, 4f * Time.fixedDeltaTime, 0);
                //rb.AddForce(transform.up * 2, ForceMode.VelocityChange);
                return;
            }
            if (transform.position.y < groundY) {
                // Stop from falling into the ground
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }
            /*if (rb.velocity.y < 0) {
                // Fall faster
                rb.velocity += -transform.up * 20 * Time.fixedDeltaTime;
            }*/
            //rb.useGravity = true;
            if (Math.Abs(transform.position.y - groundY) < 3f) {
                // Re-enable movement once enemy falls back to the ground
                agent.enabled = true;
            }
            return; // Don't move while airborne
        }
        float dist = agent.pathPending ? Vector3.Distance(transform.position, player.transform.position) : agent.remainingDistance;

        // Stop enemy if it gets too close or too far from player
        agent.isStopped = dist <= 3f || dist >= 15f;
        //anim.SetBool("isWalking", !agent.isStopped);

        // Auto attack the player
        bool isAttacking = Time.time >= nextAutoAttackTime && dist <= 3f;
        //anim.SetBool("isAttacking", isAttacking);
        if (isAttacking) {
            GameObject.Find("2D Health Slider").GetComponent<Health>().takeDamage(autoAttackDamage);
            nextAutoAttackTime = Time.time + autoAttackDuration;
        }
        // Look at the player and move toward them
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        agent.SetDestination(player.transform.position);
        
        /*if (transform.position.y < groundY - transform.lossyScale.y) {
            // If fell through the ground for some reason, suicide
            takeDamage(100);
        }*/
    }

    void Update() {
        if (Time.time >= colorHoldTime) {
            // Reset color after current time exceeds color hold time
            transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
    }

    public void setHealth(float h) {
        Slider health = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        health.maxValue = h;
        health.value = h;
    }

    public void takeDamage(float value) {
        // Flash red
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        colorHoldTime = Time.time + .4f; // Hold for 0.4 seconds

        // Reduce health
        Slider health = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        if (health.value <=  0f) {
            // Don't kill enemy twice
            return;
        }
        health.value -= value;

        if (health.value <= 0f) {
            // Out of health -> die
            if (agent.enabled) {
                agent.isStopped = true;
                agent.enabled = false;
            }
            if (gameObject != null) {
                if (sceneName == "Boss Scene") {
                    Fade.SetActive(true);
                    StartCoroutine(WaitForSceneLoad());
                } else {
                    Destroy(gameObject);
                }
            }
            // Spawn a coin
            //Instantiate(coinPrefab, transform.position, transform.rotation);

            // Increment coin count and set text accordingly
            PlayerStats stats = player.GetComponent<PlayerStats>();
            stats.setCoinCount(stats.getCoinCount() + 10);

            // Unity waits until the next frame to remove the object from
            // tags, so having a seperate enemy count is necessary
            WaveManager wm = GameObject.Find("Waves").GetComponent<WaveManager>();
            --wm.currentEnemyCount;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Whirlwind") {
            return;
        }
        takeDamage(other.GetComponent<Whirlwind>().damage);
        agent.enabled = false;
        groundY = transform.position.y;

        // Don't snap back to ground until half a second passes
        snapToGroundIgnoreTime = Time.time + .5f;

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.up * 12f, ForceMode.Impulse);
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
}

