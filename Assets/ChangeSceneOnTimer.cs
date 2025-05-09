using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{

    public float changeTime;
    public string sceneName;

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
