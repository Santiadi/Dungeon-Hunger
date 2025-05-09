using UnityEngine;

public class coinLogic : MonoBehaviour
{
    public AudioClip coinClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins(1);

            if (coinClip != null)
            {
                AudioSource.PlayClipAtPoint(coinClip, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
