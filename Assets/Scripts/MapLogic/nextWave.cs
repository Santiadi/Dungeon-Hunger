using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class nextWave : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Items");
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceWave();
            Destroy(this.gameObject);
            if (GameObject.FindGameObjectWithTag("UpgradeTrigger"))
            {
                Destroy(GameObject.FindGameObjectWithTag("UpgradeTrigger"));
            }
        }
        if (items == null)
        {
            Destroy(this.gameObject);
            return;
        }
        foreach (GameObject item in items)
        {
            Destroy(item);
        }


    }
}
