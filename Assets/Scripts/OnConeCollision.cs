using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnConeCollision : MonoBehaviour {
    public List<Collider> currentCollisions = new List<Collider>();

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy"))
            currentCollisions.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy"))
            currentCollisions.Remove(other);
    }
}
