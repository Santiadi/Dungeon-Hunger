using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Monedas
    public int coins = 0;

    [Header("Oleadas")]
    public int totalEnemiesInWave;
    public int enemiesSpawned = 0;
    public int enemiesKilled = 0;
    public int currentLevel = 1;
    public int currentWave = 1;

    public Slider waveSlider; // Asigna este en el Inspector

    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Transform[] spawnPoints;

    private bool bossSpawned = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }

        if (Instance != this)
        {
            Debug.LogError("El GameManager no se inicializó correctamente");
            return;
        }

        LoadGame(); 
    }

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    // MONEDAS
    public void AddCoins(int amount)
    {
        coins += amount;
        SaveGame();

        HUDCoins hud = FindObjectOfType<HUDCoins>();
        if (hud != null)
        {
            hud.UpdateCoins(coins);
        }
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData(coins);
        SaveSystem.SavePlayer(data);
    }

    public void UpdateGoldDisplay()
    {
        HUDCoins hud = FindObjectOfType<HUDCoins>();
        if (hud != null)
        {
            hud.UpdateCoins(coins);
        }
    }


    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        coins = data.coins;
        UpdateGoldDisplay();
    }

    // OLEADAS
    IEnumerator SpawnWave()
    {
        enemiesKilled = 0;
        enemiesSpawned = 0;

        totalEnemiesInWave = CalculateEnemiesPerWave();
        StartWave(totalEnemiesInWave);

        for (int i = 0; i < totalEnemiesInWave; i++)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(1f); // Delay entre enemigos
        }

        yield return new WaitUntil(() => enemiesKilled >= totalEnemiesInWave);
        AdvanceWave();
    }

    void StartWave(int totalEnemies)
    {
        totalEnemiesInWave = totalEnemies;
        enemiesKilled = 0;

        if (waveSlider != null)
        {
            waveSlider.value = 0f;
        }
    }

    int CalculateEnemiesPerWave()
    {
        if (currentWave == 10) return 0; // Oleada 10 es el jefe
        return 3 + (currentLevel - 1) * 5 + currentWave;
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoints[index].position, Quaternion.identity);
    }

    void SpawnBoss()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(bossPrefab, spawnPoints[index].position, Quaternion.identity);
        bossSpawned = true;
        StartWave(1); // Para actualizar slider con 1 jefe
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;

        if (waveSlider != null && totalEnemiesInWave > 0)
        {
            waveSlider.value = (float)enemiesKilled / totalEnemiesInWave;
        }

        if (enemiesKilled >= totalEnemiesInWave)
        {
            Debug.Log("Oleada completada");
            // Avanza automáticamente desde la corrutina
        }
    }

    void AdvanceWave()
    {
        if (currentWave < 10)
        {
            currentWave++;
            StartCoroutine(SpawnWave());
        }
        else if (!bossSpawned)
        {
            SpawnBoss();
        }
        else
        {
            currentLevel++;
            currentWave = 1;
            bossSpawned = false;
            StartCoroutine(SpawnWave());
        }
    }
}
