using System.Collections;
using UnityEngine;

public class nextWave : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceWave();
        }

        Destroy(this.gameObject);
    }
}
