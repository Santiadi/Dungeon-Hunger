using UnityEngine;

public class SwordSlashEffect : MonoBehaviour
{
    [Header("Configuración por defecto")]
    public int defaultUses;
    public float slashHeight;

    [Header("Prefab del proyectil slash")]
    public GameObject slashPrefab;

    [Header("Estado actual (debug)")]
    public int slashesRemaining;

    private movement playerMovement;


    void Awake()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            defaultUses = (int)(SaveSystem.GetUpgradeBonus("Numero de cortes de espada") == 0 ? 3 : SaveSystem.GetUpgradeBonus("Numero de cortes de espada"));
            slashHeight = SaveSystem.GetUpgradeBonus("Aumento de corte de espada") == 0 ? 0.5f : SaveSystem.GetUpgradeBonus("Aumento de corte de espada");
            Debug.Log("Usos de espada: " + defaultUses + ", Altura: " + slashHeight);

        }
    }


    void Start()
    {
        playerMovement = GetComponent<movement>();
        slashesRemaining = 0; // Asegurarse que comience desactivado
    }

    public void Activate(int uses, float height)
    {
        slashesRemaining = uses;
        slashHeight = height;
        Debug.Log("✨ Sword Slash activado con " + uses + " usos. Altura: " + height);
    }

    public void TryShootSlash(Vector2 shootDir)
    {
        if (slashesRemaining <= 0) return;

        slashesRemaining--;

        Vector3 spawnPos = transform.position + (Vector3)shootDir.normalized;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg);

        GameObject slash = Instantiate(slashPrefab, spawnPos, rotation);

        SlashProjectile slashScript = slash.GetComponent<SlashProjectile>();
        if (slashScript != null)
        {
            slashScript.SetDirection(shootDir);
        }

        // Escala visual del proyectil (altura)
        slash.transform.localScale = new Vector3(
            slash.transform.localScale.x,
            slashHeight,
            slash.transform.localScale.z
        );

        Debug.Log("⚔️ Slash disparado. Usos restantes: " + slashesRemaining);
    }

    public bool IsActive() => slashesRemaining > 0;
}
