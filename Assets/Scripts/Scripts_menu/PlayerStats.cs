using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    // Estadísticas base del jugador
    public float health = 0f;         // Salud comienza en 0
    public float damage = 0f;         // Daño comienza en 0
    public float speed = 0f;          // Velocidad comienza en 0
    public int bombs = 0;             // Número de bombas en 0
    public int swordCuts = 0;         // Número base de cortes de espada en 0
    public float blessingChance = 0f; // Probabilidad base de bendición en 0
    public int goldMultiplier = 0;    // Multiplicador de oro en 0

    // Array de mejoras que tiene el jugador
    public UpgradeData[] upgrades;

    void Awake()
    {
        // Asegurarnos de que haya una única instancia de PlayerStats en la escena
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Ya no se carga desde PlayerPrefs
        // Las estadísticas deben inicializarse por defecto o desde JSON si lo implementas
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

            // Ya no se guarda en PlayerPrefs
        }
    }
}
