using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Monedas")]
    public int coins = 0;

    [Header("Objetos post-oleada")]
    public GameObject continueButtonPrefab;
    public GameObject blessingObjectPrefab;
    public Transform postWaveSpawnPoint; // Donde aparecerán los botones/objetos
    public GameObject portal;
    private bool nextWave = false;

    [Header("Bendiciones")]
    public int blessingLevel = 0; // Nivel de mejora de bendiciones (0 a 2)



    [Header("Oleadas")]
    public int totalEnemiesInWave;
    public int enemiesSpawned = 0;
    public int enemiesKilled = 0;
    public int currentLevel = 1;
    public int currentWave = 1;
    public Slider waveSlider;

    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public GameObject[] spawnPoints;
    private bool bossSpawned = false;

    [Header("DontDestroyElements")]

    public GameObject HUD;
    public GameObject Player;

    private void Awake()
    {
        if (Instance == null)
        {
            FindSpawners();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(HUD);
            DontDestroyOnLoad(Player);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

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

    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        coins = data.coins;
    }


    void OnEnable()
    {
        // Suscribirse al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Desuscribirse para evitar fugas de memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSpawners(); // Llamado al cargar una nueva escena
    }

    void FindSpawners()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");

        postWaveSpawnPoint = GameObject.FindGameObjectWithTag("SpawnerAfterWave").transform;

        Debug.Log("Spawners encontrados: " + spawnPoints.Length);

        // Puedes guardar las referencias o inicializar lógica aquí
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

        int enemyIndex = Mathf.Clamp(currentLevel - 1, 0, enemyPrefabs.Length - 1);

        Instantiate(enemyPrefabs[enemyIndex], spawnPoints[index].transform.position, Quaternion.identity);
    }

    void SpawnBoss()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(bossPrefab, spawnPoints[index].transform.position, Quaternion.identity);
        bossSpawned = true;
        StartWave(1);
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
            Debug.Log("OLEADA COMPLETADA: " + currentWave);
            if (currentWave == 10)
            {
                Instantiate(portal, postWaveSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                currentWave++;

                ShowPostWaveOptions();
            }
        }
    }

    public void AdvanceWave()
    {
        Debug.Log("OLEADA ACTUAL: " + currentWave);
        if (currentWave <= 9)
        {
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
    void ShowPostWaveOptions()
    {

        if (continueButtonPrefab != null && postWaveSpawnPoint != null)
        {
            Instantiate(continueButtonPrefab, postWaveSpawnPoint.position, Quaternion.identity);
        }
        TryShowBlessing();
    }
    void TryShowBlessing()
    {
        float[] chances = { 0.5f, 0.75f, 1.0f };
        float chance = chances[Mathf.Clamp(blessingLevel, 0, 2)];

        if (Random.value <= chance)
        {
            if (blessingObjectPrefab != null && postWaveSpawnPoint != null)
            {
                Vector3 offset = new Vector3(2f, 0, 0); // Separar visualmente
                Instantiate(blessingObjectPrefab, postWaveSpawnPoint.position + offset, Quaternion.identity);
            }
        }
    }
    public bool SetWaveState(bool state) => this.nextWave = state;
}
