using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBoxScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // Aquí puedes aplicar el daño al enemigo o cualquier otra lógica que necesites
                Destroy(collision.gameObject); // Destruye el enemigo al entrar en la hitbox
            }

        }
    }
}
