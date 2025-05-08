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
            Debug.Log("Suerte oro: " + chanceToDrop);
            Debug.Log("Mas oro: " + goldChanceDrop);
        }
    }
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

    }


    public void ReciveDamage(float damage)
    {
        if (anim != null)
        {
            anim.SetTrigger("Hurt"); 
        }
        hearts -= damage;
        if (hearts <= 0)
        {
            Destroy(this.gameObject);
        }
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
                Debug.Log("Monedas sueltas: " + goldChanceDrop);
                for (int i = 0; i <= goldChanceDrop; i++)
                {
                    Debug.Log("Monedas sueltas: " + i);
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