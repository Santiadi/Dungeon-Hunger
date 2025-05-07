using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSpawner : MonoBehaviour
{
    public static UpgradeSpawner Instance;

    [Header("Referencias")]
    public Transform upgradeGrid;
    public GameObject upgradeItemPrefab;
    public List<UpgradeData> upgrades;

    private List<UpgradeItemUI> spawnedUpgrades = new List<UpgradeItemUI>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadUpgradesProgress();
        SpawnUpgrades();
    }

    void SpawnUpgrades()
    {
        foreach (UpgradeData data in upgrades)
        {
            GameObject obj = Instantiate(upgradeItemPrefab, upgradeGrid);

            // Buscar hijos por nombre exacto
            Transform iconTransform = obj.transform.Find("Icon");
            Transform frameTransform = obj.transform.Find("Frame");
            Transform tickContainer = obj.transform.Find("TickContainer");

            if (iconTransform == null || frameTransform == null || tickContainer == null)
            {
                Debug.LogError("Uno de los elementos no se encontró en el prefab.");
                continue;
            }

            Image iconImage = iconTransform.GetComponent<Image>();
            Image frameImage = frameTransform.GetComponent<Image>();

            iconImage.sprite = data.icon;
            frameImage.sprite = data.frame;

            // Obtener componente UI para ticks
            UpgradeItemUI ui = obj.GetComponent<UpgradeItemUI>();
            if (ui != null)
            {
                ui.iconImage = iconImage;
                ui.frameImage = frameImage;
                ui.tickContainer = tickContainer;
                ui.SetTicks(data.currentLevel, data.maxLevel);
                ui.SetUpgradeData(data);

                spawnedUpgrades.Add(ui);
            }
        }

        // AJUSTAR ALTO DEL GRID DINÁMICAMENTE
        GridLayoutGroup grid = upgradeGrid.GetComponent<GridLayoutGroup>();
        int totalItems = upgrades.Count;
        int columns = grid.constraintCount;
        int rows = Mathf.CeilToInt((float)totalItems / columns);

        float cellHeight = grid.cellSize.y;
        float spacingY = grid.spacing.y;
        float paddingTop = grid.padding.top;
        float paddingBottom = grid.padding.bottom;

        float totalHeight = paddingTop + paddingBottom + rows * cellHeight + (rows - 1) * spacingY;

        RectTransform rt = upgradeGrid.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    void LoadUpgradesProgress()
    {
        foreach (var upgradeData in upgrades)
        {
            UpgradeDetailsUI.Instance.LoadUpgradeProgress(upgradeData);
        }
    }


    // ✅ Refrescar los ticks visuales después de una mejora
    public static void RefreshAllTicks()
    {
        if (Instance == null) return;

        foreach (var ui in Instance.spawnedUpgrades)
        {
            if (ui != null && ui.upgradeData != null)
            {
                ui.SetTicks(ui.upgradeData.currentLevel, ui.upgradeData.maxLevel);
            }
        }
    }
}
