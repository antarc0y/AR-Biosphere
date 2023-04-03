using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public static Animator transition;
    public float transitionTime = 1f;

    void Start()
    {
        GameObject sceneLoader = GameObject.Find("SceneLoader");     
        transition = sceneLoader.GetComponent<Animator>();
    }

    static public void GoToNextScene() 
    {
        GameObject sceneLoader = GameObject.Find("SceneLoader");
        transition = sceneLoader.GetComponent<Animator>();

        if (transition != null)
        {
            
            // play animation
            transition.SetTrigger("start");

            // load scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       else
        {
            Debug.LogWarning("animator is not initialized");
        }

        // re-assign the animator since its now at a new scene
        transition = sceneLoader.GetComponent<Animator>();

    }

    static public void GoToPreviousScene()
    {
        GameObject sceneLoader = GameObject.Find("SceneLoader");
        transition = sceneLoader.GetComponent<Animator>();

        if (transition != null)
        {
            // play animation
            transition.SetTrigger("start");
            // load scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            Debug.LogWarning("animator is not initialized");
        }
        // re-assign the animator since its now at a new scene
        transition = sceneLoader.GetComponent<Animator>();
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
