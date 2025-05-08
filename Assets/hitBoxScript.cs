using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour
{
    public float damage = 1f; // Daño del ataque
    public float knockbackForce = 5f; // Fuerza de retroceso
    public float knockbackDuration = 0.5f; // Duración del retroceso

    void Awake()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            damage = SaveSystem.GetUpgradeBonus("Incremento de daño") == 0 ? 1 : SaveSystem.GetUpgradeBonus("Incremento de daño");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.ReciveDamage(damage); // Resta el daño a la vida del enemigo
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemy.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                StartCoroutine(ApplyKnockback(enemy, knockbackDirection));
            }
        }
    }

    private IEnumerator ApplyKnockback(EnemyController enemy, Vector2 direction)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero; // Detiene el movimiento del enemigo
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse); // Aplica el retroceso

        yield return new WaitForSeconds(knockbackDuration); // Espera la duración del retroceso

        rb.velocity = Vector2.zero; // Detiene el movimiento del enemigo después del retroceso
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Detiene el movimiento del enemigo al salir del hitbox
            }
        }
    }
}
