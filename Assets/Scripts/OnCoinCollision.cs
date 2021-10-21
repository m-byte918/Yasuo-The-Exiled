using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCoinCollision : MonoBehaviour {
    public Text coinCountText;

    void Start() {
        coinCountText = GameObject.Find("Wallet").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.name != "Player") {
            return;
        }
        // In the future we should add this to the player "wallet"
        // But for now, just delete
        Destroy(gameObject);

        // Update coin count and set text accordingly
        PlayerStats stats = other.GetComponent<PlayerStats>();
        stats.setCoinCount(stats.getCoinCount() + 1);
        coinCountText.text = "Coins: " + stats.getCoinCount();
    }
}
