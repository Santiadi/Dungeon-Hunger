using UnityEngine;

public class SlashProjectile : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 1.5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir.normalized * speed;

        // Opcional: rotar el sprite hacia la dirección
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // ✅ destruye al enemigo
            Debug.Log("Slash golpeó y destruyó a: " + other.name);
            // ❌ NO se destruye el slash
        }
    }

}
