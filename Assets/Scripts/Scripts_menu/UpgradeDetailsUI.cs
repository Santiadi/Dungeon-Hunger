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
        int coins = GoldManager.Instance.GetCurrentGold();
        Debug.Log("Botón de comprar fue clickeado");

        int level = currentUpgrade.currentLevel;

        if (level >= currentUpgrade.maxLevel)
        {
            Debug.Log("Ya está en el nivel máximo");
            return;
        }

        int cost = currentUpgrade.costPerLevel[level];

        if (coins >= cost)
        {
            coins -= cost;  // Resta el oro
            SaveUpgradeProgress(currentUpgrade);
            currentUpgrade.currentLevel++;
            UpgradeSpawner.RefreshAllTicks();
            SaveGame(coins);  // Guarda el juego

            // Llamada para actualizar el oro
            GoldManager.Instance.UpdateGoldMenu();
            ShowDetails(currentUpgrade);
            Debug.Log("Compra realizada");
        }
        else
        {
            Debug.Log("No hay suficiente oro");
        }
    }


    public void SaveGame(int coins)
    {
        PlayerData data = new PlayerData(coins);
        SaveSystem.SavePlayer(data);
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
