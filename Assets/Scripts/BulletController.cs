using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DeathDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }



    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log(collision.name);
    //     movement player = collision.GetComponent<movement>();

    //     player.CharacterDamage(1);
    //     if(collision.tag == "Player")
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
