using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public Canvas playerHealthCanvas;

    public float waveGrowthRate = 2.0f;
    public int currentEnemyCount = 0;
    private int currentWaveSize = 1;

    public void spawnNextWave() {
        // Scale current wave size by predetermined factor
        int nextWaveSize = (int)System.Math.Ceiling(currentWaveSize * waveGrowthRate);

        // Enemy spawn bound
        float minX = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        float minZ = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).z;
        float maxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        float maxZ = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).z;

        for (int i = 0; i < nextWaveSize; ++i) {
            // Choose random position within spawn bound
            float spawnX = Random.Range(minX, maxX);
            float spawnZ = Random.Range(minZ, maxZ);
            Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, spawnZ);

            // Spawn new enemy
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.GetComponentInChildren<EnemyHealthSlider>().playerHealthCanvas = playerHealthCanvas;
        }
        currentWaveSize = nextWaveSize;
    }
}
