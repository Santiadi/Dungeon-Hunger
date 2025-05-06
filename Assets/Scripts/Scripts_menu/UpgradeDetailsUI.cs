using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeDetailsUI : MonoBehaviour
{
    public static UpgradeDetailsUI Instance;

    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Button buyButton;

    private UpgradeData currentUpgrade;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false); 
    }

    public void ShowDetails(UpgradeData data)
    {
        currentUpgrade = data;
        nameText.text = data.upgradeName;
        descriptionText.text = data.description;

        int level = data.currentLevel;

        if (level >= data.maxLevel)
        {
            buyButton.interactable = false;
            costText.text = "Max";  
        }
        else
        {
            int cost = data.costPerLevel[level];
            costText.text = cost.ToString();

            if (GameManager.Instance != null && GameManager.Instance.coins < cost)
            {
                buyButton.interactable = false;
            }
            else
            {
                buyButton.interactable = true;
            }
        }

        panel.SetActive(true);  
    }



    public void Hide()
    {
        panel.SetActive(false);  
    }

public void OnBuyClicked()
{
    Debug.Log("Botón de comprar fue clickeado");

    int level = currentUpgrade.currentLevel;

    if (level >= currentUpgrade.maxLevel)
    {
        Debug.Log("Ya está en el nivel máximo");
        return; 
    }

    int cost = currentUpgrade.costPerLevel[level];

    if (GameManager.Instance.coins >= cost)  
    {
        GameManager.Instance.coins -= cost;  // Resta el oro
        SaveUpgradeProgress(currentUpgrade); 
        currentUpgrade.currentLevel++; 
        UpgradeSpawner.RefreshAllTicks(); 
        GameManager.Instance.SaveGame();  // Guarda el juego

        // Llamada para actualizar el oro
        GoldManager.Instance.UpdateGoldMenu();
        GameManager.Instance.UpdateGoldDisplay();
        ShowDetails(currentUpgrade);
        Debug.Log("Compra realizada");
    }
    else
    {
        Debug.Log("No hay suficiente oro");
    }
}





        public void SaveUpgradeProgress(UpgradeData upgradeData)
    {
        PlayerPrefs.SetInt("UpgradeLevel_" + upgradeData.upgradeName, upgradeData.currentLevel);
        PlayerPrefs.Save();
    }

        public void LoadUpgradeProgress(UpgradeData upgradeData)
    {
        int savedLevel = PlayerPrefs.GetInt("UpgradeLevel_" + upgradeData.upgradeName, 0);
        upgradeData.currentLevel = savedLevel;
    }

}
