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
    private int numeroDeBombas = 1;

    private Blessing currentBlessing;


    void Awake()
    {
        numeroDeBombas = (int)(SaveSystem.GetUpgradeBonus("Numero de bombas") == 0 ? 1f : SaveSystem.GetUpgradeBonus("Numero de bombas"));
    }


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

        InventoryManager inventory = GameObject.FindGameObjectWithTag("Player")
                                              .GetComponent<movement>()
                                              .inventoryManager;

        movement playerMovement = GameObject.FindGameObjectWithTag("Player")
                                           .GetComponent<movement>();

        switch (blessing.blessingName)
        {
            case "Bomb":
            case "Small Potion":
            case "Medium Potion":
            case "Max Potion":
            case "Clear":
            case "Sword Slash": 
                TrySpawnItem(blessing.blessingName);
                break;


            case "Extra Slot":
                if (inventory != null && inventory.CanUnlockSlot2())
                {
                    inventory.UnlockSlot2ByBlessing();
                }
                break;

            case "Healing +0.5":
                playerMovement?.Heal(0.5f);
                break;

            case "Healing +1":
                playerMovement?.Heal(1f);
                break;

            case "Max Healing":
                playerMovement?.Heal(5f);
                break;

            case "Coins +2":
                GameManager.Instance?.AddCoins(2);
                break;

            case "Coins +5":
                GameManager.Instance?.AddCoins(5);
                break;

            case "Coins +10":
                GameManager.Instance?.AddCoins(10);
                break;

            case "Extra Life":
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    movement playerScript = player?.GetComponent<movement>();

                    if (playerScript != null)
                    {
                        playerScript.revive += 1f;
                        Debug.Log("Revive aumentado. Ahora tiene: " + playerScript.revive);
                    }
                    break;
                }

            case "Ghost Heart":
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    movement playerScript = player?.GetComponent<movement>();

                    if (playerScript != null)
                    {
                        playerScript.ghostHearts += 1f;
                        playerScript.hudHearts?.UpdateHearts(playerScript.currentHearts, playerScript.maxHearts, playerScript.ghostHearts);
                        Debug.Log("Ghost Heart agregado. Total: " + playerScript.ghostHearts);
                    }
                    break;
                }

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
            Vector3 basePos = GameObject.FindGameObjectWithTag("Player").transform.position;

            int cantidad = 1;

            if (itemName == "Bomb")
            {
                cantidad = numeroDeBombas;
            }

            for (int i = 0; i < cantidad; i++)
            {
                Vector3 offset = new Vector3(1.5f, i * 0.2f, 0); // 👈 muy poca separación vertical
                Instantiate(prefab, basePos + offset, Quaternion.identity);
            }


        }

        else
        {
            Debug.LogError("❌ No se encontró el prefab para: " + itemName);
        }
    }



}
