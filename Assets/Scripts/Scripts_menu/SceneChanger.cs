using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GameScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();  
        }

        SceneManager.LoadScene("Map 1");
    }

    public void UpgradesScene()
    {
        SceneManager.LoadScene("UpgradeScene"); 
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");  
    }
}
