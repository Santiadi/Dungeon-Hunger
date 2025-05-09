using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("UI")]
    public Text waveText;

    [Header("Monedas")]
    public int coins = 0;

    [Header("Objetos post-oleada")]
    public GameObject continueButtonPrefab;
    public GameObject blessingObjectPrefab;
    public Transform postWaveSpawnPoint; // Donde aparecerán los botones/objetos
    public GameObject portal;
    public GameObject portalSpawn;
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

    [Header("Enemigos")]
    public GameObject[] enemyPrefabs;
    public GameObject[] bossPrefabs;
    public GameObject[] spawnPoints;
    private bool bossSpawned = false;

    [Header("DontDestroyElements")]

    public GameObject HUD;
    public GameObject Player;

    public GameObject BlessingManager;
    public GameObject InventoryManager;
    public GameObject MusicPlayer;

    public GameObject EventSystem;

    [Header("Pausa")]
    public GameObject pauseBackground;
    public GameObject pauseText;
    private bool isPaused = false;

    [Header("Audio")]
    public AudioSource musicSource;
    private AudioClip normalWaveMusic;
    private AudioClip bossWaveMusic;


    [Header("Muerte")]
    public GameObject deathBackground;
    public GameObject deathText;




    private void Awake()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            blessingLevel = (int)(SaveSystem.GetUpgradeBonus("Aumento de ratio de bendiciones") == 0 ? 0 : SaveSystem.GetUpgradeBonus("Aumento de ratio de bendiciones"));

        }
        if (Instance == null)
        {
            FindSpawners();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(HUD);
            DontDestroyOnLoad(Player);
            DontDestroyOnLoad(BlessingManager);
            DontDestroyOnLoad(InventoryManager);
            DontDestroyOnLoad(EventSystem);
            DontDestroyOnLoad(MusicPlayer);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        pauseBackground.SetActive(isPaused);
        pauseText.SetActive(isPaused);

        // ⏸️ Pausar o reanudar música
        if (musicSource != null)
        {
            if (isPaused)
                musicSource.Pause();
            else
                musicSource.UnPause();
        }
    }


    public void ShowDeathScreen()
    {
        deathBackground.SetActive(true);
        deathText.SetActive(true);
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
        if (scene.name == "MenuScene")
        {
            Destroy(gameObject);
            return;
        }
        if (waveText)
        {
            waveText.text = "1";
        }
        LoadMapMusic();
        FindSpawners(); // Llamado al cargar una nueva escena
    }

    void FindSpawners()
    {
        if (GameObject.FindGameObjectsWithTag("Spawner") == null) return;

        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");

        postWaveSpawnPoint = GameObject.FindGameObjectWithTag("SpawnerAfterWave").transform;
        if(GameObject.FindGameObjectWithTag("PortalSpawner") == null) return;
        portalSpawn = GameObject.FindGameObjectWithTag("PortalSpawner");

    }

    void LoadMapMusic()
    {
        MapMusicConfig config = FindObjectOfType<MapMusicConfig>();

        if (config != null)
        {
            normalWaveMusic = config.normalWaveMusic;
            bossWaveMusic = config.bossWaveMusic;
            PlayWaveMusic(); // Iniciar la música desde el principio
        }
        else
        {
            Debug.LogWarning("No se encontró un MapMusicConfig en la escena.");
        }
    }


    void PlayWaveMusic()
    {
        if (musicSource == null) return;

        AudioClip targetClip = currentWave == 10 ? bossWaveMusic : normalWaveMusic;

        if (musicSource.clip != targetClip)
        {
            musicSource.clip = targetClip;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("🎵 Reproduciendo: " + targetClip.name);
        }
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

        int bossIndex = Mathf.Clamp(currentLevel - 1, 0, bossPrefabs.Length - 1);

        Instantiate(bossPrefabs[bossIndex], spawnPoints[index].transform.position, Quaternion.identity);
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
            Debug.Log("Oleada " + currentWave + " completada!");
            if (currentWave == 10)
            {
                currentWave++;
                if(GameObject.FindGameObjectWithTag("PortalSpawner") == null){return;}
                ActivePortal();
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
        currentLevel ++;
        if (currentWave <= 9)
        {
            StartCoroutine(SpawnWave());
        }
        else if (!bossSpawned)
        {
            SpawnBoss();
        }
        else if (currentWave == 11)
        {
            currentLevel++;
            currentWave = 1;
            bossSpawned = false;
            StartCoroutine(SpawnWave());
        }
        waveText.text = currentWave.ToString();
        PlayWaveMusic();

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

    void ActivePortal()
    {
        Instantiate(portal, portalSpawn.transform.position, Quaternion.Euler(0,0,-90f));
    }

    public void ReturnToMenuFromDeath()
    {
        StartCoroutine(DelayedReturnToMenu());
    }

    IEnumerator DelayedReturnToMenu()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(HUD);
        Destroy(BlessingManager);
        Destroy(InventoryManager);
        Destroy(Player); // por si aún sigue activo
        Destroy(MusicPlayer);
        deathBackground.SetActive(true);
        deathText.SetActive(true);

        SceneManager.LoadScene("MenuScene");
    }


}
