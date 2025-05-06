using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeItemUI : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public Image frameImage;
    public Transform tickContainer;
    public Sprite tickFullSprite;
    public Sprite tickEmptySprite;

    public UpgradeData upgradeData;

    public void SetTicks(int currentLevel, int maxLevel)
    {
        foreach (Transform child in tickContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxLevel; i++)
        {
            GameObject tick = new GameObject("Tick", typeof(RectTransform), typeof(Image));
            tick.transform.SetParent(tickContainer, false);

            Image img = tick.GetComponent<Image>();
            img.sprite = i < currentLevel ? tickFullSprite : tickEmptySprite;
            img.rectTransform.sizeDelta = new Vector2(22, 22);
        }
    }

    public void SetUpgradeData(UpgradeData data)
    {
        upgradeData = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgradeData == null)
        {
            Debug.LogError("No se ha asignado upgradeData en este UpgradeItemUI.");
            return;
        }

        if (UpgradeDetailsUI.Instance == null)
        {
            Debug.LogError("UpgradeDetailsUI.Instance es null. ¿Está el objeto en escena y activo?");
            return;
        }

        UpgradeDetailsUI.Instance.ShowDetails(upgradeData);
    }
}
