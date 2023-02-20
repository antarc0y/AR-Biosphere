using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToNextScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); 
    }

    public void GoToPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // if back button (Android navigation) is pressed
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                GoToPreviousScene();
            }
        }
    }
}
