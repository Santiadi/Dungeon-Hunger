using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;  // Nombre del item (por ejemplo: "Bomb")
    public float healingAmount = 0f;  // Solo para pociones

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador tocó el item: " + itemName);


            InventoryManager inventory = other.GetComponent<movement>().inventoryManager;

            if (inventory != null)
            {
                Debug.Log("Asignando al inventario: " + itemName);

                // Dentro de Item.cs
                inventory.AssignItem(itemName);

            }
            else
            {
                Debug.LogError("No se encontró el InventoryManager.");
            }

            Destroy(gameObject);

            Debug.Log(itemName + " recogido!");
        }
    }
}
