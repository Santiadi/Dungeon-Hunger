using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectile; 
    public float projectileSpeed = 5f; 

    public float minShootDelay = 0.5f;
    public float maxShootDelay = 3f;   

    public int minProjectilesPerBurst = 1; 
    public int maxProjectilesPerBurst = 5; 

    public float timeBetweenProjectiles = 0.2f; 

    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {

            float delay = Random.Range(minShootDelay, maxShootDelay);
            yield return new WaitForSeconds(delay);


            int projectilesToShoot = Random.Range(minProjectilesPerBurst, maxProjectilesPerBurst + 1);

            for (int i = 0; i < projectilesToShoot; i++)
            {
                Shoot();

                yield return new WaitForSeconds(timeBetweenProjectiles);
            }
        }
    }

    void Shoot()
    {
        GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);

        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * projectileSpeed;
        }
    }
}
