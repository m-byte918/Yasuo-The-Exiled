using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.name != "Player") {
            // Only a player can trigger wave spawn
            return;
        }
        WaveManager wm = GetComponentInParent<WaveManager>();
        if (wm.currentEnemyCount > 0) {
            // Enemies in current wave have not been killed, do not start a new one
            return;
        }
        // Disable collision with inner collider
        if (wm.currentEnemyCount == 0)
        {
            GetComponents<Collider>()[0].isTrigger = true;
        }

        // Spawn next wave
        wm.spawnNextWave();
    }
}
