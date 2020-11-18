using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    NavMeshAgent agent;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        // Stop enemy if it gets too close or too far from player
        agent.isStopped = agent.remainingDistance <= 3f || agent.remainingDistance >= 15f;

        // Look at the player and move toward them
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        agent.SetDestination(playerTransform.position);
    }
}
