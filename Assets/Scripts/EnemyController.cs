using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Player;
    public GameObject dropPrefab; // Arrástralo desde el Inspector


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

            transform.Translate(direction * 2 * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "playerDamage")
        {
            Destroy(gameObject);
        }
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