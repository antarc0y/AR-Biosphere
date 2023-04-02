using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public static Animator transition;
    public float transitionTime = 1f;
    // Start is called before the first frame update
    //void Start()
    //{
    // transition = GetComponent<Animator>();

    // Log the Animator component to the console
    // Debug.Log("Animator component: " + transition);
    //}

    void Start()
    {
        GameObject sceneLoader = GameObject.Find("SceneLoader");     
        transition = sceneLoader.GetComponent<Animator>();
        Debug.Log(transition);
    }

    static public void GoToNextScene() 
    {
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
        
    }

    static public void GoToPreviousScene()
    {
        // play animation
        transition.SetTrigger("start");
        // load scene
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
