using UnityEngine;

public class Whirlwind : MonoBehaviour {
    public GameObject whirlwind;
    public GameObject arrowCollider;
    //public AudioSource whirlwindSound;
    private float speed = 40f;

    public void launch() {
        // Play sound
        //whirlwindSound.Play();

        // Make visible
        whirlwind.SetActive(true);

        // Launch from player position
        whirlwind.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + transform.localScale.y,
            transform.position.z
        );
    }

    void FixedUpdate() {
        // Dont move if not active
        if (!whirlwind.activeSelf) {
            return;
        }
        // Launch in forward direction
        Vector3 velocity = -arrowCollider.transform.forward * speed * Time.fixedDeltaTime;
        whirlwind.GetComponent<Rigidbody>().MovePosition(whirlwind.transform.position + velocity);
    }
}
