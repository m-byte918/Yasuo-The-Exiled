using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeep : MonoBehaviour {
    public Canvas shop;

    void OnTriggerEnter(Collider other) {
        if (other.name != "Player") {
            return;
        }
        shop.gameObject.SetActive(true); // Open shop UI
    }
}
