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

    // Mostrar información incluso cuando la habilidad esté al máximo
    if (level >= data.maxLevel)
    {
        // Deshabilitar el botón de compra si la habilidad está al máximo
        buyButton.interactable = false;

        // Mostrar mensaje de "Max" o similar en lugar del coste
        costText.text = "Max";  // O puedes poner algo como "Ya al máximo"
    }
    else
    {
        // Si no está al máximo, mostrar el coste real
        int cost = data.costPerLevel[level];
        costText.text = cost.ToString();

        // Si no hay suficiente oro, deshabilitar el botón de compra
        if (GoldManager.Instance.currentGold < cost)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    panel.SetActive(true);  // Mostrar el panel con la información
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
            return; 

        int cost = currentUpgrade.costPerLevel[level];

        if (GoldManager.Instance.TrySpendGold(cost))  
        {
            Debug.Log("Compra realizada");
            PlayerStats.Instance.ApplyUpgradeEffects(currentUpgrade);
            currentUpgrade.currentLevel++; 
            SaveUpgradeProgress(currentUpgrade);  
            ShowDetails(currentUpgrade); 
            UpgradeSpawner.RefreshAllTicks();  
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
