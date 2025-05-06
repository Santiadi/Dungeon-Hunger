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
    if (GameManager.Instance != null && goldText != null)  // Asegúrate de que GameManager.Instance no sea null
    {
        goldText.text = GameManager.Instance.coins.ToString();
    }
    else
    {
        Debug.LogError("GameManager no está inicializado correctamente.");
    }
}

    // Obtiene el oro actual
    public int GetCurrentGold()
    {
        return GameManager.Instance.coins;
    }
}
