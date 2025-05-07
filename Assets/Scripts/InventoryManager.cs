using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;

    public Sprite potionSprite;  
    public Sprite bombSprite;   

    private bool inventory2Unlocked = false;
    private string[] inventory = new string[2];

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseItem(0); 
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory2Unlocked) 
            {
                UseItem(1); 
            }
        }
        if (Input.GetKeyDown(KeyCode.M)) // Tecla M para desbloquear el inventario 2
        {
            UnlockInventory2();
        }
    }

    public void UnlockInventory2()
    {
        if (!inventory2Unlocked)
        {
            inventory2Unlocked = true;
            slot2.SetActive(true);
        }
    }

    public void AssignItemToSlot(int slot, string item)
    {
        if (slot >= 0 && slot < inventory.Length)
        {

            if (!string.IsNullOrEmpty(inventory[0]) && inventory2Unlocked)
            {

                inventory[1] = item;
                Debug.Log("Asignando al slot 2: " + item);
                UpdateSlotUI(1); 
            }
            else
            {

                inventory[0] = item;
                Debug.Log("Asignando al slot 1: " + item);
                UpdateSlotUI(0); 
            }
        }
        else
        {
            Debug.LogError("Slot no válido: " + slot); 
        }
    }

    private void UpdateSlotUI(int slot)
    {
        GameObject slotObj = (slot == 0) ? slot1 : slot2;
        Transform iconTransform = slotObj.transform.Find("Icon");

        if (iconTransform == null)
        {
            Debug.LogError("No se ha encontrado el objeto 'Icon' dentro de " + slotObj.name);
            return;
        }

        Image iconImage = iconTransform.GetComponent<Image>();

        if (iconImage == null)
        {
            Debug.LogError("El componente Image no se encuentra en el objeto 'Icon' dentro de " + slotObj.name);
            return;
        }

        if (!string.IsNullOrEmpty(inventory[slot]))
        {
            Debug.Log("Actualizando sprite del inventario en el slot " + slot + " con el item: " + inventory[slot]);

            // Asignar el sprite correcto basado en el nombre del item
            Sprite itemSprite = null;

            if (inventory[slot] == "Small_Potion")
            {
                itemSprite = potionSprite;  // Asignar el sprite de la poción
            }
            else if (inventory[slot] == "Bomb")
            {
                itemSprite = bombSprite;  // Asignar el sprite de la bomba
            }

            if (itemSprite != null)
            {
                iconImage.sprite = itemSprite;
                iconImage.enabled = true;
            }
            else
            {
                Debug.LogError("Sprite no encontrado para el item: " + inventory[slot]);
            }
        }
        else
        {
            iconImage.enabled = false;  // Ocultar la imagen si el slot está vacío
        }
    }

    public void UseItem(int slot)
    {
        if (!string.IsNullOrEmpty(inventory[slot]))
        {
            Debug.Log("Usando item de inventario " + (slot + 1) + ": " + inventory[slot]);

            if (inventory[slot] == "Small_Potion")
            {
                // Aplicar curación
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                movement playerScript = player.GetComponent<movement>();
                playerScript.Heal(0.5f); 

                inventory[slot] = "";
                UpdateSlotUI(slot); 
            }

            if (inventory[slot] == "Bomb")
            {
                // Colocar la bomba en el escenario
                PlaceBomb();


                inventory[slot] = "";
                UpdateSlotUI(slot);
            }
        }
        else
        {
            Debug.Log("No hay item en el inventario " + (slot + 1));
        }
    }

    private void PlaceBomb()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); 
        Vector3 placePosition = player.transform.position;

        GameObject bombWithLogic = Instantiate(Resources.Load("Bomb") as GameObject, placePosition, Quaternion.identity);

        bombWithLogic.layer = 2;  

        Debug.Log("Bomba colocada en: " + placePosition);
    }

}
