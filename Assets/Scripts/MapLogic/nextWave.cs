using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class nextWave : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceWave();
        }

        // Eliminar todos los objetos con el tag "Item"
        GameObject[] items = GameObject.FindGameObjectsWithTag("Items");
        if( GameObject.FindGameObjectWithTag("UpgradeTrigger")){
            Destroy(GameObject.FindGameObjectWithTag("UpgradeTrigger"));
        }
        if (items == null){ 
            Destroy(this.gameObject);
            return;
        }
        foreach (GameObject item in items)
        {
            Destroy(item);
        }

        Destroy(this.gameObject); // Destruye el trigger
    }
}
