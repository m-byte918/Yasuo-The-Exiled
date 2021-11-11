using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public Canvas playerHealthCanvas;

    public float waveGrowthRate = 0.5f;
    public int currentEnemyCount = 0;
    private int currentWaveSize = 1;
    private int currentWave = 0;

    public void spawnNextWave() {
        ++currentWave;

        GameObject currentWaveTrigger = GameObject.Find("SpawnZone " + currentWave);
        GameObject nextWaveTrigger = GameObject.Find("SpawnZone " + (currentWave + 1));

        if (!currentWaveTrigger || !nextWaveTrigger) {
            // No start or end of waves
            return;
        }
        // Scale current wave size by predetermined factor
        int nextWaveSize = ++currentWaveSize;

        Debug.Log("Next Wave Size: " + nextWaveSize);

        // Enemy spawn bound
        float minX = currentWaveTrigger.transform.position.x - currentWaveTrigger.transform.lossyScale.x / 2f;
        float maxX = currentWaveTrigger.transform.position.x + currentWaveTrigger.transform.lossyScale.x / 2f;
        float minZ = currentWaveTrigger.transform.position.z + currentWaveTrigger.transform.lossyScale.z / 2f;
        float maxZ = currentWaveTrigger.transform.position.z - currentWaveTrigger.transform.lossyScale.z / 2f;
        float spawnY = GameObject.Find("Player").transform.position.y;

        for (int i = 0; i < nextWaveSize; ++i) {
            // Choose random position within spawn bound
            float spawnX = Random.Range(minX, maxX);
            float spawnZ = Random.Range(minZ, maxZ);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            // Spawn new enemy
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.GetComponentInChildren<EnemyHealthSlider>().playerHealthCanvas = playerHealthCanvas;
        }
        currentWaveSize = nextWaveSize;
    }
}
