using UnityEngine;
using System.Collections;  

public class Bomb : MonoBehaviour
{
    public float explosionTime = 3f;  
    public float explosionRadius;  
    public float explosionDamage = 10f;  // Da�o que causa la bomba
    public GameObject radiusVisualPrefab;
    private GameObject radiusVisualInstance;
    public string enemyTag = "Enemy";  
    private Animator bombAnimator; 

    private CircleCollider2D explosionCollider;


    void Awake()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            explosionRadius = SaveSystem.GetUpgradeBonus("Aumento de area de explosion de la bomba") == 0 ? 1f : SaveSystem.GetUpgradeBonus("Aumento de area de explosion de la bomba");
        }
    }

    void Start()
    {

        bombAnimator = GetComponent<Animator>();

        explosionCollider = gameObject.AddComponent<CircleCollider2D>();
        explosionCollider.radius = explosionRadius;
        explosionCollider.isTrigger = true;  

        explosionCollider.enabled = false;

        StartCoroutine(ExplosionCountdown());

        if (radiusVisualPrefab != null)
        {
            radiusVisualInstance = Instantiate(radiusVisualPrefab, transform.position, Quaternion.identity);
            radiusVisualInstance.transform.SetParent(transform);
            radiusVisualInstance.transform.localScale = Vector3.one * explosionRadius * 2f; // Doble radio = diámetro
            radiusVisualInstance.SetActive(true);
        }

    }


    private IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionTime);
        bombAnimator?.SetTrigger("Explode");
        yield return new WaitForSeconds(0.4f); // Delay final de animaci�n

        Explode();
    }


    private void Explode()
    {
        Debug.Log("¡La bomba ha explotado!");

        explosionCollider.enabled = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag(enemyTag))
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.ReciveDamage(explosionDamage); 
                Destroy(enemy.gameObject);
                Debug.Log("Enemigo destruido: " + enemy.name);
            }
        }

        if (radiusVisualInstance != null)
        {
            radiusVisualInstance.SetActive(false);
            Destroy(radiusVisualInstance);
        }

        Destroy(gameObject);
    }


    // Dibujar el radio de la explosi�n en la escena para debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}



