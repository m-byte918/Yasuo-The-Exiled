using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCoinCollision : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        // In the future we should add this to the player "wallet"
        // But for now, just delete
        if (other.name == "Player")
            Destroy(gameObject);
    }
}
