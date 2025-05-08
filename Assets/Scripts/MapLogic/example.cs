using UnityEngine;
using UnityEngine.SceneManagement;

public class example : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GoToNextScene();
    }

    public void GoToNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Map 1")
        {
            SceneManager.LoadScene("Map 2");
        }
        else if (currentScene == "Map 2")
        {
            SceneManager.LoadScene("Map 3");
        }
        else
        {
            Debug.Log("No hay más mapas definidos.");
        }
    }
}
