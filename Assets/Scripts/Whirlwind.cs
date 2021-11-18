using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Whirlwind : MonoBehaviour {
    public GameObject whirlwind;
    public GameObject arrowCollider;
    public GameObject whirlwindPrefab;
    //public AudioSource whirlwindSound;
    private float speed = 40f; // Speed for all whirlwinds
    Vector3 direction; // Direction whirlwind will launch
    List<whirlwindData> wData = new List<whirlwindData>();

    struct whirlwindData {
        GameObject whirlwind;
        Vector3 velocity;
    };

    public void launch() {
        // Play sound
        //whirlwindSound.Play();

        //wData.Add(new whirlwindData())

        // Make visible
        whirlwind.SetActive(true);

        // Launch from player position
        whirlwind.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + transform.localScale.y,
            transform.position.z
        );
        direction = -arrowCollider.transform.forward;
    }

    void FixedUpdate() {
        // Dont move if not active
        if (!whirlwind.activeSelf) {
            return;
        }
        // Launch in forward direction
        Vector3 velocity = direction * speed * Time.fixedDeltaTime;
        whirlwind.GetComponent<Rigidbody>().MovePosition(whirlwind.transform.position + velocity);
    }
}
