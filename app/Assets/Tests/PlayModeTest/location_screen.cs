using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace location_ui_test
{
    public class ui_test : InputTestFixture
    {
        //setup mouse, camerabutton, backbutton
        public Mouse mouse;
        public GameObject camera_button;
        public GameObject back_button;
        public override void Setup()
        {
            //setup current scene as user_location and add mouse
            base.Setup();
            SceneManager.LoadScene("user_location");
            mouse = InputSystem.AddDevice<Mouse>();
        }

        public void ClickUI(GameObject uiElement)
        {
            // find uiElement position with respect to camera and click on it.
            Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Vector3 screenpos = camera.WorldToScreenPoint(uiElement.transform.position);
            //Debug.Log(screenpos.x + " "+ screenpos.y);
            //Vector3 mousep = Input.mousePosition;
            Set(mouse.position, screenpos);
            Click(mouse.leftButton);
        }
        [UnityTest]
        public IEnumerator back_button_test()
        {
            //navigate from user_location to home_screen
            back_button = GameObject.Find("Canvas/Back Button");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("user_location"));

            ClickUI(back_button);
            yield return new WaitForSeconds(2f);
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("home_screen"));
        }

        [UnityTest]
        public IEnumerator Camera_button()
        {
            //navigate from user_screen to AR_scene
            camera_button = GameObject.Find("Canvas/CameraButton");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("user_location"));
            ClickUI(camera_button);
            yield return new WaitForSeconds(2f);
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("AR_Scene"));
        }

    }
}
