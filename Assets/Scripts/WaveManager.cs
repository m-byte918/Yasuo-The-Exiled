using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public Canvas playerHealthCanvas;

    public int currentEnemyCount = 0;
    private int currentWaveSize = 1;
    private int currentWave = 0;

    public void spawnNextWave() {
        ++currentWave;
        ++currentWaveSize; // Scale wave by 1

        GameObject currentWaveTrigger = GameObject.Find("SpawnZone " + currentWave);
        GameObject nextWaveTrigger = GameObject.Find("SpawnZone " + (currentWave + 1));

        if (!currentWaveTrigger || !nextWaveTrigger) {
            // No start or end of waves
            return;
        }
        // Enemy spawn bound
        Transform cwtTransform = currentWaveTrigger.transform;
        float minX = cwtTransform.position.x - cwtTransform.lossyScale.x / 2f;
        float maxX = cwtTransform.position.x + cwtTransform.lossyScale.x / 2f;
        float minZ = cwtTransform.position.z - cwtTransform.lossyScale.z / 2f;
        float maxZ = cwtTransform.position.z + cwtTransform.lossyScale.z / 2f;
        float spawnY = GameObject.Find("Player").transform.position.y;

        for (int i = 0; i < currentWaveSize; ++i) {
            // Choose random position within spawn bound
            float spawnX = Random.Range(minX, maxX);
            float spawnZ = Random.Range(minZ, maxZ);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            // Spawn new enemy
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.GetComponentInChildren<EnemyHealthSlider>().playerHealthCanvas = playerHealthCanvas;
        }
    }
}
