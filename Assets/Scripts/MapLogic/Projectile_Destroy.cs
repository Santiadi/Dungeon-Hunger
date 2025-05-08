using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 0.5f; // Puedes cambiar este valor desde el Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ProjectileDestroyer"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            movement player = other.GetComponent<movement>();
            if (player != null)
            {
                player.CharacterDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
