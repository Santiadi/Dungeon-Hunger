using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    // Estadísticas base del jugador
    public float health = 100f;  // Salud comienza en 100
    public float damage = 10f;   // Daño comienza en 10
    public float speed = 5f;     // Velocidad comienza en 5
    public int bombs = 1;        // Número de bombas
    public int swordCuts = 3;    // Número base de cortes de espada
    public float blessingChance = 0.5f;  // Probabilidad base de bendición
    public int goldMultiplier = 1; // Multiplicador de oro

    // Array de mejoras que tiene el jugador
    public UpgradeData[] upgrades;

    void Awake()
    {
        // Asegurarnos de que haya una única instancia de PlayerStats en la escena
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Cargar las estadísticas del jugador guardadas
        LoadPlayerStats();
    }

    // Cargar estadísticas guardadas desde PlayerPrefs
    void LoadPlayerStats()
    {
        health = PlayerPrefs.GetFloat("PlayerHealth", 100f);  // Si no existe, comienza en 100
        damage = PlayerPrefs.GetFloat("PlayerDamage", 10f);    // Si no existe, comienza en 10
        speed = PlayerPrefs.GetFloat("PlayerSpeed", 5f);       // Si no existe, comienza en 5
        bombs = PlayerPrefs.GetInt("PlayerBombs", 1);          // Número base de bombas
        swordCuts = PlayerPrefs.GetInt("PlayerSwordCuts", 3);  // Número base de cortes de espada
        blessingChance = PlayerPrefs.GetFloat("PlayerBlessingChance", 0.5f); // Probabilidad base de bendición
        goldMultiplier = PlayerPrefs.GetInt("PlayerGoldMultiplier", 1); // Multiplicador de oro

        // Cargar el progreso de las mejoras
        foreach (var upgrade in upgrades)
        {
            LoadUpgradeProgress(upgrade); 
        }
    }

    // Guardar las estadísticas del jugador en PlayerPrefs
    void SavePlayerStats()
    {
        PlayerPrefs.SetFloat("PlayerHealth", health);
        PlayerPrefs.SetFloat("PlayerDamage", damage);
        PlayerPrefs.SetFloat("PlayerSpeed", speed);
        PlayerPrefs.SetInt("PlayerBombs", bombs);
        PlayerPrefs.SetInt("PlayerSwordCuts", swordCuts);
        PlayerPrefs.SetFloat("PlayerBlessingChance", blessingChance);
        PlayerPrefs.SetInt("PlayerGoldMultiplier", goldMultiplier);
        PlayerPrefs.Save();  
    }

    // Aplicar los efectos de una mejora al jugador
    public void ApplyUpgradeEffects(UpgradeData upgradeData)
    {
        int level = upgradeData.currentLevel;

        if (level >= 0 && level < upgradeData.bonusPerLevel.Length)
        {
            float bonus = upgradeData.bonusPerLevel[level];

            // Aquí aplicamos las mejoras específicas de cada habilidad
            switch (upgradeData.upgradeName)
            {
                case "VelocidadMovimiento":
                    speed = bonus;  // Cambiar velocidad según el bonus
                    break;

                case "VelocidadDeAtaque":
                    // Reducir el tiempo de cooldown de ataque según el bonus
                    break;

                case "SaludMaxima":
                    health = bonus;  // Establecer salud máxima según el bonus
                    break;

                case "DanioBoss":
                    damage = bonus;  // Aumentar daño del boss
                    break;

                case "SuerteOro":
                    blessingChance = bonus;  // Probabilidad de encontrar oro
                    break;

                case "MasOro":
                    goldMultiplier = Mathf.RoundToInt(bonus);  // Multiplicador de oro
                    break;

                case "Revive":
                    // Habilitar la función de revivir si se mejora
                    break;

                case "AumentarAreaExplosion":
                    // Cambiar el área de explosión según el nivel
                    break;

                case "AumentarTamanoCorteEspada":
                    // Mejorar el largo del corte de espada
                    break;

                case "NumeroBombas":
                    bombs = Mathf.RoundToInt(bonus);  // Establecer número de bombas
                    break;

                case "NumeroCorteEspada":
                    swordCuts = Mathf.RoundToInt(bonus);  // Establecer número de cortes
                    break;

                case "AumentarRatioBendicion":
                    blessingChance = bonus;  // Mejorar la probabilidad de bendición
                    break;
            }

            // Guardar los cambios después de aplicar la mejora
            SavePlayerStats();
        }
    }

    // Guardar el progreso de una mejora en PlayerPrefs
    public void SaveUpgradeProgress(UpgradeData upgradeData)
    {
        PlayerPrefs.SetInt("UpgradeLevel_" + upgradeData.upgradeName, upgradeData.currentLevel);
        PlayerPrefs.Save();
    }

    // Cargar el progreso de una mejora desde PlayerPrefs
    void LoadUpgradeProgress(UpgradeData upgradeData)
    {
        int savedLevel = PlayerPrefs.GetInt("UpgradeLevel_" + upgradeData.upgradeName, 0);  // Cargar el nivel guardado (0 si no se encuentra)
        upgradeData.currentLevel = savedLevel;
    }
}
