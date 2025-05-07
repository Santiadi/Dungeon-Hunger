using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLogic : MonoBehaviour
{
    public GameObject batPrefab;

    [Header("Oleadas")]
    public int enemiesPerWave = 5;
    public float spawnDelay = 1f;
    public float waveDelay = 5f;
    
    private int enemiesSpawned = 0;
    private int currentWave = 1;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        enemiesSpawned = 0;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Instantiate(batPrefab, transform.position, Quaternion.identity);
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnDelay);
        }

        // Espera antes de la siguiente oleada
        yield return new WaitForSeconds(waveDelay);

        currentWave++;
        enemiesPerWave += 2; // Incrementar dificultad
        StartCoroutine(SpawnWave());
    }
}
