using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ProjectileDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}
