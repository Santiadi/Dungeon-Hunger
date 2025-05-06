using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Player;
    public GameObject dropPrefab; // Arrástralo desde el Inspector

    public float velocity;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {

            Vector2 direction = (Player.position - transform.position).normalized;

            transform.Translate(direction * velocity * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (movement.Instance != null)
            {
                movement.Instance.CharacterDamage(0.5f);
                Destroy(this.gameObject);
            }

        }
        Destroy(this.gameObject);

    }

    void OnDestroy()
    {
        if (dropPrefab != null && Random.value <= 0.15f)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }
    }
}