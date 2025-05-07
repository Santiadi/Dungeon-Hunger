using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlessingCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image backgroundImage;
    public Image iconImage;
    public Text nameText;

    private Vector3 originalScale;
    public float hoverScale = 1.1f;

    private Blessing currentBlessing;

    private void Start()
    {
        originalScale = transform.localScale;


    }

    public void Setup(Blessing blessing)
    {
        currentBlessing = blessing;

        var manager = FindObjectOfType<BlessingManager>();
        backgroundImage.sprite = manager.GetCardSprite(blessing.rarity);

        iconImage.sprite = blessing.icon;
        nameText.text = blessing.blessingName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ApplyBlessing(currentBlessing);
        Time.timeScale = 1f;

        // Destruir el menú
        Transform current = transform;
        while (current != null && current.name != "UpgradesMenuCanvas(Clone)")
            current = current.parent;

        if (current != null)
            Destroy(current.gameObject);

        GameObject trigger = GameObject.Find("UpgradesTrigger");
        if (trigger != null)
            Destroy(trigger);
    }

    private void ApplyBlessing(Blessing blessing)
    {
        Debug.Log($"Aplicando bendición: {blessing.blessingName}");

        switch (blessing.blessingName)
        {
            case "Bomb":
            case "Small Potion":
            case "Medium Potion":
            case "Max Potion":
                TrySpawnItem(blessing.blessingName);
                break;

            case "Extra Slot":
                InventoryManager inventory = GameObject.FindGameObjectWithTag("Player")
                                                      .GetComponent<movement>()
                                                      .inventoryManager;

                if (inventory != null && inventory.CanUnlockSlot2())
                {
                    inventory.UnlockSlot2ByBlessing();
                }
                else
                {
                    Debug.LogWarning("La bendición 'Extra Slot' ya fue usada.");
                }
                break;

            default:
                Debug.LogWarning("No hay lógica definida para esta bendición.");
                break;
        }
    }



    private void TrySpawnItem(string itemName)
    {
        Debug.Log("Intentando cargar: BlessingItems/" + itemName.Replace(" ", ""));

        GameObject prefab = Resources.Load<GameObject>("BlessingItems/" + itemName.Replace(" ", ""));

        if (prefab != null)
        {
            Vector3 spawnPosition = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.right * 1.5f;

            GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Debug.Log($"[{itemName}] instanciado en la posición: {spawnPosition}");
        }
        else
        {
            Debug.LogError("❌ No se encontró el prefab para: " + itemName);
        }
    }



}
