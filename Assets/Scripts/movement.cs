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

    public int hearts = 5;

    public GameObject swordHitBox;
    public Rigidbody2D rb;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
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
            anim.SetBool("Attack", true);
            Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }

        // Vector3 tempVect = new Vector3(input.x, input.y, 0);
        // transform.Translate(tempVect * speed * Time.deltaTime);
        if (hearts == 0)
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

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

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

        // GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);

        // Rigidbody2D bulletRb = bullet.AddComponent<Rigidbody2D>();
        // bulletRb.gravityScale = 0;


        swordHitBox.SetActive(true);
        swordHitBox.transform.position = transform.position + (Vector3)shootDirection;
        // bulletRb.velocity = shootDirection * bulletSpeed;
        StartCoroutine(DisableSwordHitbox());
    }

    IEnumerator DisableSwordHitbox()
    {
        yield return new WaitForSeconds(0.1f);
        swordHitBox.SetActive(false);
        anim.SetBool("Attack", false);
    }

    public void CharacterDamage(int damage)
    {
        hearts -= damage;
    }
}