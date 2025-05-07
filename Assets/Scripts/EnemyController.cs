using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Player;
    public GameObject dropPrefab; // Arrástralo desde el Inspector

    public float velocity;
    public float damage;


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

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            transform.Translate(direction * velocity * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (movement.Instance != null)
            {
                movement.Instance.CharacterDamage(damage);
                Destroy(this.gameObject);
            }

        }
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        // CAMBIAR A CHANCE COMO EL BLESS
        // 0.20f = 20% de probabilidad de soltar el objeto
        // 0.25f = 25% de probabilidad de soltar el objeto
        // 0.30f = 30% de probabilidad de soltar el objeto

        if (dropPrefab != null && Random.value <= 0.15f)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }
    }

    public void SetProperties(float damage, float velocity) {
        this.velocity = velocity;
        this.damage = damage;
    }

}