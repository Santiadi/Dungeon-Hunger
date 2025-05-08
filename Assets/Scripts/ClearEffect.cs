using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    public void Activate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int enemiesToRemove = Mathf.FloorToInt(enemies.Length / 2f);

        for (int i = 0; i < enemiesToRemove; i++)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }

        Debug.Log("☠️ Clear activado: " + enemiesToRemove + " enemigos eliminados.");
    }
}
