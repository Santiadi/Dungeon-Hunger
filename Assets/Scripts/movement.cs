using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class movement : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float speed = 100;
    private float lastFire;
    public float fireDelay;

    private Vector2 input;

    public float currentHearts = 3f; // Vida actual del jugador
    public int maxHearts = 3;        // Vida m�xima posible (de 3 a 5)

    public GameObject swordHitBox;
    public Rigidbody2D rb;

    private Animator anim;

    // Referencia al HUD
    public HUD_Hearts hudHearts;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Asegura que la vida no exceda la m�xima
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        // Inicializa el HUD con la vida inicial
        if (hudHearts != null)
        {
            hudHearts.UpdateHearts(currentHearts, maxHearts);
        }
    }

    void Update()
    {
        ProcessInputs();
        Animate();

        float shootHor = Input.GetAxisRaw("ShootHorizontal");
        float shootVer = Input.GetAxisRaw("ShootVertical");

        if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
        {
            anim.SetFloat("AttackX", shootHor);
            anim.SetFloat("AttackY", shootVer);
            anim.SetTrigger("AttackTrigger");
            Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }

        if (currentHearts <= 0)
        {
            Debug.Log("muerto");
        }

    }

    void FixedUpdate()
    {
        Vector2 movement = input * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void ProcessInputs()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        input.Normalize();
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitud", input.magnitude);
    }

    void Shoot(float x, float y)
    {
        Vector2 shootDirection = new Vector2(x, y);
        if (shootDirection.magnitude > 0)
        {
            shootDirection.Normalize();
        }

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        swordHitBox.SetActive(true);
        swordHitBox.transform.position = transform.position + (Vector3)shootDirection;
    }

    public  void DisableSwordHitbox()
    {
        swordHitBox.SetActive(false);
    }

    public void CharacterDamage(float damage)
    {
        currentHearts -= damage;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        if (hudHearts != null)
        {
            hudHearts.UpdateHearts(currentHearts, maxHearts);
        }

        if (currentHearts <= 0)
        {
            Debug.Log("muerto");
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHearts = Mathf.Clamp(maxHearts + amount, 1, 5);
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        if (hudHearts != null)
        {
            hudHearts.UpdateHearts(currentHearts, maxHearts);
        }
    }
}