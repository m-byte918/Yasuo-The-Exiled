using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCoinCollision : MonoBehaviour {
    public Text coinCountText;

    void Start() {
        coinCountText = GameObject.Find("Wallet").GetComponent<Text>();
    }

    void OnCollisionEnter(Collision c) {
        if (c.collider.name != "Player") {
            return;
        }
        // But for now, just delete
        Destroy(gameObject);

        // Increment coin count and set text accordingly
        PlayerStats stats = c.collider.GetComponent<PlayerStats>();
        stats.setCoinCount(stats.getCoinCount() + 1);
        coinCountText.text = "Coins: " + stats.getCoinCount();
    }
}
