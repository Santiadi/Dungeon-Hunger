using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Player;
    public GameObject dropPrefab; // Arrástralo desde el Inspector

    public bool isBoss = false;
    public float velocity;
    public float damage;
    public GameObject maxPotionPrefab;
    public float hearts;
    private float chanceToDrop = 0.15f; // 15% de probabilidad de soltar el objeto
    private float goldChanceDrop = 1f; // Cuantas monedas suelta el enemigo

    private Animator anim;
    private bool canMove = true;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Awake()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            chanceToDrop = SaveSystem.GetUpgradeBonus("Suerte oro") == 0 ? 0.15f : SaveSystem.GetUpgradeBonus("Suerte oro");
            goldChanceDrop = SaveSystem.GetUpgradeBonus("Mas oro") == 0 ? 1f : SaveSystem.GetUpgradeBonus("Mas oro");
        }
    }
    void Update()
    {
        if (Player != null && canMove)
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

        if (isBoss && collision.tag == "Player")
        {
            if (movement.Instance != null)
            {
                movement.Instance.CharacterDamage(damage);
                ReciveDamage(1);
            }
        }
        if (collision.tag == "Player")
        {
            if (movement.Instance != null)
            {
                movement.Instance.CharacterDamage(damage);
                ReciveDamage(0);
            }

        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBoss && collision.gameObject.CompareTag("Player"))
        {
            if (movement.Instance != null)
            {
                movement.Instance.CharacterDamage(damage);
                ReciveDamage(1);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (movement.Instance != null)
            {
                movement.Instance.CharacterDamage(damage);
                ReciveDamage(0);
            }

        }
    }
    public void ReciveDamage(float damage)
    {
        Debug.Log("Vida actual: " + hearts);
        Debug.Log("Daño recibido: " + damage);

        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        hearts -= damage;
        Debug.Log("Vida después del daño: " + hearts);
        StartCoroutine(ApplyKnockback());

        if (hearts <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private IEnumerator ApplyKnockback()
    {
        canMove = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null && Player != null)
        {
            Vector2 knockbackDir = (transform.position - Player.position).normalized;
            float knockbackForce = 5f;
            rb.velocity = knockbackDir * knockbackForce;
        }

        yield return new WaitForSeconds(0.5f);

        if (rb != null)
            rb.velocity = Vector2.zero;

        canMove = true;
    }


    void OnDestroy()
    {

        if (isBoss)
        {
            Instantiate(maxPotionPrefab, transform.position, Quaternion.identity);

            for (int i = 0; i < 5; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 0.2f; // Separación leve entre monedas
                Vector2 spawnPos = (Vector2)transform.position + offset;
                Instantiate(dropPrefab, spawnPos, Quaternion.identity);
            }


        }
        else
        {
            if (dropPrefab != null && Random.value <= chanceToDrop)
            {
                for (int i = 0; i < goldChanceDrop; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * 0.2f; // Separación leve entre monedas
                    Vector2 spawnPos = (Vector2)transform.position + offset;
                    Instantiate(dropPrefab, spawnPos, Quaternion.identity);
                }


            }

        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }
    }

    public void SetProperties(float damage, float velocity)
    {
        this.velocity = velocity;
        this.damage = damage;
    }

}