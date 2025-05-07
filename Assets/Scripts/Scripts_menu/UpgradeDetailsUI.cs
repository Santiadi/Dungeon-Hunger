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
        Debug.Log("LEVEL: " + level);

        if (level >= currentUpgrade.maxLevel)
        {
            Debug.Log("Ya está en el nivel máximo");
            return;
        }

        int cost = currentUpgrade.costPerLevel[level];

        if (coins >= cost)
        {
            coins -= cost;  // Resta el oro
            Debug.Log("SE COMPRO: " + currentUpgrade);
            currentUpgrade.currentLevel++;
            SaveUpgradeProgress(currentUpgrade);
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
        BlessData data = SaveSystem.LoadUpgrade(upgradeData);
        data.AddBlessing(upgradeData.upgradeName, upgradeData.currentLevel, upgradeData.bonusPerLevel[upgradeData.currentLevel - 1]);
        SaveSystem.SaveUpgrades(data);
    }

    public void LoadUpgradeProgress(UpgradeData upgradeData)
    {
        upgradeData.currentLevel = SaveSystem.GetUpgradeLevel(upgradeData.upgradeName);
    }

}
