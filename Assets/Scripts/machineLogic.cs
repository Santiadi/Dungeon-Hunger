using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineLogic : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float fireDelay = 0.5f; // medio segundo entre disparos
    private float lastFireTime;

    void Update()
    {
        if (Time.time > lastFireTime + fireDelay)
        {
            FireBullet();
            lastFireTime = Time.time;
        }
    }

    void FireBullet()
    {
        Vector2 direction = Vector2.right;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody2D bulletRb = bullet.AddComponent<Rigidbody2D>();
        bulletRb.gravityScale = 0;
        bulletRb.velocity = direction * 2;
    }

}
