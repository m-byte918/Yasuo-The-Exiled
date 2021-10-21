using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to keep track of (save-able) player stats

public class PlayerStats : MonoBehaviour {
    private int _coinCount = 0;

    public int getCoinCount() {
        return _coinCount;
    }

    public void setCoinCount(int count) {
        _coinCount = count;
    }
}
