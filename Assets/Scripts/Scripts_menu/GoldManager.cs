using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int currentGold; 
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);

        LoadGold();
        UpdateGoldDisplay();
    }

    void LoadGold()
    {
        currentGold = PlayerPrefs.GetInt("PlayerGold", 0);
    }

    void SaveGold()
    {
        PlayerPrefs.SetInt("PlayerGold", currentGold);
        PlayerPrefs.Save();
    }

    public bool TrySpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;  
            SaveGold();  
            UpdateGoldDisplay();  
            return true;
        }

        Debug.Log("No hay suficiente oro");
        return false; 
    }

    // Método para agregar oro
    public void AddGold(int amount)
    {
        Debug.Log("Añadiendo oro: " + amount);
        currentGold += amount;  
        SaveGold();  
        UpdateGoldDisplay();
    }

    void UpdateGoldDisplay()
    {
        if (goldText != null)
            goldText.text = currentGold.ToString(); 
    }

    public int GetCurrentGold()
    {
        return currentGold;  
    }
}
