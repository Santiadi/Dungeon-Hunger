using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{

    public float changeTime;
    public string sceneName;

    void Start()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is not set. Please set the scene name in the inspector.");
            return;
        }
        
        // Optional: Load the scene immediately if you want to skip the timer
        // SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Space))
        {
            changeTime = 0;
        }
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
        
    }

    
}
