using UnityEngine;

public class UpgradesTrigger : MonoBehaviour
{
    public GameObject upgradesMenuCanvasPrefab;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            Instantiate(upgradesMenuCanvasPrefab);
            Time.timeScale = 0f;
            triggered = true;
        }
    }
}
