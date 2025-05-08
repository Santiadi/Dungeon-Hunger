using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene("Map 2");
    }
}
