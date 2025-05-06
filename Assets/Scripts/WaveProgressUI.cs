using UnityEngine;
using UnityEngine.UI;

public class WaveProgressUI : MonoBehaviour
{
    public Slider waveSlider;

    private int totalEnemiesInWave = 10;  // por ejemplo
    private int enemiesKilled = 0;

    public void SetWave(int totalEnemies)
    {
        totalEnemiesInWave = totalEnemies;
        enemiesKilled = 0;
        waveSlider.value = 0f;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        waveSlider.value = (float)enemiesKilled / totalEnemiesInWave;
    }
}
