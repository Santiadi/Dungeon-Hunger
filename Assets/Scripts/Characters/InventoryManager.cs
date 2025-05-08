using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public Sprite itemSprite;
    public float healAmount; // 0 si no cura (ej: bomba)
}

[System.Serializable]
public class InventorySlot
{
    public string itemName = "";
    public int quantity = 0;
}

public class InventoryManager : MonoBehaviour
{
    [Header("Slots visuales")]
    public GameObject slot1;
    public GameObject slot2;

    [Header("Datos de ítems")]
    public List<InventoryItemData> itemDataList;

    private Dictionary<string, InventoryItemData> itemDataDict;

    private InventorySlot[] inventory = new InventorySlot[2]
    {
        new InventorySlot(),
        new InventorySlot()
    };

    private bool inventory2Unlocked = false;
    private bool slot2BlessingUsed = false;

    private void Start()
    {
        itemDataDict = new Dictionary<string, InventoryItemData>();
        foreach (var item in itemDataList)
        {
            if (!itemDataDict.ContainsKey(item.itemName))
            {
                itemDataDict[item.itemName] = item;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) UseItem(0);
        if (Input.GetKeyDown(KeyCode.E) && inventory2Unlocked) UseItem(1);
    }

    // Acceso externo desde bendiciones
    public bool CanUnlockSlot2()
    {
        return !slot2BlessingUsed;
    }

    public void UnlockSlot2ByBlessing()
    {
        if (!slot2BlessingUsed)
        {
            inventory2Unlocked = true;
            slot2.SetActive(true);
            slot2BlessingUsed = true;

            Debug.Log("🔓 Slot 2 desbloqueado por bendición.");

            UpdateSlotUI(1);
        }
        else
        {
            Debug.LogWarning("⚠️ Ya usaste la bendición de Slot Extra.");
        }
    }


    // Usado si quieres desbloquear manualmente con una tecla
    public void UnlockInventory2()
    {
        if (!inventory2Unlocked)
        {
            inventory2Unlocked = true;
            slot2.SetActive(true);
        }
    }

    public void AssignItem(string item)
    {
        // Agregar a stack existente
        for (int i = 0; i < inventory.Length; i++)
        {
            if (i == 1 && !inventory2Unlocked) continue;

            if (inventory[i].itemName == item)
            {
                inventory[i].quantity++;
                UpdateSlotUI(i);
                return;
            }
        }

        // Buscar slot vacío disponible
        for (int i = 0; i < inventory.Length; i++)
        {
            if (i == 1 && !inventory2Unlocked) continue;

            if (string.IsNullOrEmpty(inventory[i].itemName))
            {
                inventory[i].itemName = item;
                inventory[i].quantity = 1;
                UpdateSlotUI(i);
                return;
            }
        }

        Debug.Log("Inventario lleno o slot bloqueado. No se pudo agregar: " + item);
    }

    public void UseItem(int slot)
    {
        var data = inventory[slot];

        if (string.IsNullOrEmpty(data.itemName) || !itemDataDict.ContainsKey(data.itemName))
        {
            Debug.Log("Slot vacío o ítem inválido.");
            return;
        }

        var itemInfo = itemDataDict[data.itemName];

        if (itemInfo.healAmount > 0f)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player?.GetComponent<movement>()?.Heal(itemInfo.healAmount);
        }
        else if (data.itemName == "BombExplode")
        {
            PlaceBomb();
        }
        else if (data.itemName == "Clear")
        {
            ClearEffect clear = new GameObject("ClearEffectTemp").AddComponent<ClearEffect>();
            clear.Activate();
            Destroy(clear.gameObject); // Limpiar el objeto temporal
        }
        else if (data.itemName == "Sword Slash")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SwordSlashEffect slashEffect = player.GetComponent<SwordSlashEffect>();

            if (slashEffect != null)
            {
                // Usa lo que está en el Inspector
                slashEffect.Activate(slashEffect.defaultUses, slashEffect.slashHeight);
            }

            data.itemName = "";
            data.quantity = 0;
            UpdateSlotUI(slot);
        }

        data.quantity--;
        if (data.quantity <= 0)
        {
            data.itemName = "";
            data.quantity = 0;
        }

        UpdateSlotUI(slot);
    }

    private void UpdateSlotUI(int slot)
    {
        GameObject slotObj = (slot == 0) ? slot1 : slot2;

        Transform iconTransform = slotObj.transform.Find("Icon");
        Transform quantityTransform = slotObj.transform.Find("Quantity");

        if (iconTransform == null || quantityTransform == null)
        {
            Debug.LogError("Faltan 'Icon' o 'Quantity' en " + slotObj.name);
            return;
        }

        Image iconImage = iconTransform.GetComponent<Image>();
        Text quantityText = quantityTransform.GetComponent<Text>();

        var data = inventory[slot];

        if (string.IsNullOrEmpty(data.itemName))
        {
            iconImage.enabled = false;
            quantityText.text = "";
            return;
        }

        iconImage.sprite = itemDataDict[data.itemName].itemSprite;
        iconImage.enabled = true;
        quantityText.text = data.quantity > 1 ? data.quantity.ToString() : "";
    }

    private void PlaceBomb()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 pos = player.transform.position;
        GameObject prefab = Resources.Load<GameObject>("BombExplode");

        if (prefab != null)
        {
            Instantiate(prefab, pos, Quaternion.identity).layer = 2;
        }
        else
        {
            Debug.LogError("Prefab 'Bomb' no encontrado.");
        }
    }
}
