using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script to keep track of (save-able) player stats

public class PlayerStats : MonoBehaviour {
    private int _coinCount = 0;
    public Text coinCountText = null;

    public int getCoinCount() {
        return _coinCount;
    }

    public void setCoinCount(int count) {
        _coinCount = count;

        // Update wallet text
        coinCountText.text = "Coins: " + _coinCount;
    }
}
