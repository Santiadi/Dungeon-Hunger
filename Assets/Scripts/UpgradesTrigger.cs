using UnityEngine;

public class UpgradesTrigger : MonoBehaviour
{
    public GameObject upgradesMenuCanvasPrefab;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            GameObject canvas = Instantiate(upgradesMenuCanvasPrefab);
            var manager = FindObjectOfType<BlessingManager>();
            var blessings = manager.GetRandomBlessings(3);

            canvas.GetComponent<BlessingsMenuUI>().Setup(blessings);

            Time.timeScale = 0f;
            triggered = true;
        }
    }
}
