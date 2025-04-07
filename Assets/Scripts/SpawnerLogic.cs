using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLogic : MonoBehaviour
{
    public GameObject batPrefab;

    public float fireDelay = 0.5f; 
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
        Instantiate(batPrefab, transform.position, transform.rotation);

    }
}
