using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class nav_test
{
    // A Test behaves as an ordinary method

    [UnityTest]
    public IEnumerator home_screenloadcheck()
    {
        // check if home_screen is loaded and set active
        SceneManager.LoadScene("home_screen");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("home_screen"));

        Scene scene = SceneManager.GetActiveScene();
        Assert.AreEqual("home_screen",scene.name);
    }

    [UnityTest]
    public IEnumerator BlankArloadcheck()
    {
        // check if blankAR is loaded and set active
        SceneManager.LoadScene("BlankAR");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BlankAR"));

        Scene scene = SceneManager.GetActiveScene();
        Assert.AreEqual("BlankAR",scene.name);
    }

    [UnityTest]
    public IEnumerator user_location_loadcheck()
    {
        // check if user_location is loaded and set active
        SceneManager.LoadScene("user_location");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("user_location"));

        Scene scene = SceneManager.GetActiveScene();
        Assert.AreEqual("user_location",scene.name);
    }

    [UnityTest]
    public IEnumerator home_screen_to_maps_navcheck()
    {
        // check if we can navigate from home_screen to user_location
        SceneManager.LoadScene("home_screen");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("home_screen"));

        Scene scene = SceneManager.GetActiveScene();
        Navigation.GoToNextScene();

        yield return null;
        Debug.Log(SceneManager.GetActiveScene().name);
        Assert.AreEqual("user_location",SceneManager.GetActiveScene().name);
        
    }

    [UnityTest]
    public IEnumerator maps_to_camera_navcheck()
    {
        // check if we can navigate from user_location to BlankAR
        SceneManager.LoadScene("user_location");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("user_location"));

        Scene scene = SceneManager.GetActiveScene();
        Navigation.GoToNextScene();

        yield return null;
        Debug.Log(SceneManager.GetActiveScene().name);
        Assert.AreEqual("BlankAR",SceneManager.GetActiveScene().name);
        
    }
    [UnityTest]
    public IEnumerator maps_to_home_screen_navcheck()
    {
        // check if we can navigate from user_location back to home_screen
        SceneManager.LoadScene("user_location");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("user_location"));

        Scene scene = SceneManager.GetActiveScene();
        Navigation.GoToPreviousScene();

        yield return null;
        Debug.Log(SceneManager.GetActiveScene().name);
        Assert.AreEqual("home_screen",SceneManager.GetActiveScene().name);
        
    }

    [UnityTest]
    public IEnumerator camera_to_map_navcheck()
    {
        // check if we can navigate from BlankAR back to user_location
        SceneManager.LoadScene("BlankAR");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BlankAR"));

        Scene scene = SceneManager.GetActiveScene();
        Navigation.GoToPreviousScene();

        yield return null;
        Debug.Log(SceneManager.GetActiveScene().name);
        Assert.AreEqual("user_location",SceneManager.GetActiveScene().name);
        
    }

    [UnityTest]

    public IEnumerator info_button_open()
    {
        SceneManager.LoadScene("BlankAR");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BlankAR"));

        DialogUI dialogui = GameObject.Find("Canvas/DialogUI").GetComponent<DialogUI>();
        dialogui.open_popup();
        yield return null;
        Assert.IsTrue(dialogui.status);
    }

    [UnityTest]
    public IEnumerator info_button_close()
    {
        SceneManager.LoadScene("BlankAR");
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BlankAR"));

        DialogUI dialogui = GameObject.Find("Canvas/DialogUI").GetComponent<DialogUI>();
        dialogui.open_popup();
        yield return null;
        Assert.IsTrue(dialogui.status);
        dialogui.close_popup();
        yield return null;
        Assert.IsFalse(dialogui.status);
    }
}
