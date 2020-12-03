using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {
    NavMeshAgent agent;

    public float rotateSpeedMovement = 1f;
    float rotateVelocity;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (!Input.GetMouseButton(1))
            return;
        // Checking if the raycast shot hits something that uses the navmesh system.
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
            // MOVEMENT
            // Have the player move to the raycast/hit point
            agent.SetDestination(hit.point);

            // Rotation
            Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);

            float rotationY = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5)
            );
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }
}
