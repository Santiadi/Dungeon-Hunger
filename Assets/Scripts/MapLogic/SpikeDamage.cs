using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float damageAmount = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            movement player = other.GetComponent<movement>();
            if (player != null)
            {
                player.CharacterDamage(damageAmount);
            }
        }
    }
}
