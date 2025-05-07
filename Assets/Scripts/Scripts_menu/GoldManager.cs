using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateGoldMenu(); // Asegura que la UI se actualice al inicio
    }

    // Método para gastar oro
    public bool TrySpendGold(int amount)
    {
        if (GameManager.Instance.coins >= amount)
        {
            GameManager.Instance.coins -= amount;  // Resta el oro en GameManager
            UpdateGoldMenu();  // Actualiza la UI
            return true;
        }

        Debug.Log("No hay suficiente oro");
        return false;
    }

    public void UpdateGoldMenu()
    {
        goldText.text = GetCurrentGold().ToString();
    }

    // Obtiene el oro actual
    public int GetCurrentGold()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        return data.coins;
    }
}
