using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;  // Nombre del item (por ejemplo: "Bomb")
    public float healingAmount = 0f;  // Solo para pociones

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = other.GetComponent<movement>().inventoryManager;

            if (inventory != null && inventory.AssignItem(itemName))
            {
                Destroy(gameObject); // solo se destruye si se agregó
            }
        }
    }

}
