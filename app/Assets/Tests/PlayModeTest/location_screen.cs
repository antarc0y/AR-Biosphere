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
        public Mouse mouse;
        public GameObject camera_button;
        public GameObject back_button;
        public override void Setup()
        {
            base.Setup();
            SceneManager.LoadScene("user_location");
            mouse = InputSystem.AddDevice<Mouse>();
        }

        public void ClickUI(GameObject uiElement)
        {
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
            back_button = GameObject.Find("Canvas/BackButton");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("user_location"));

            ClickUI(back_button);
            yield return new WaitForSeconds(2f);
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("home_screen"));
        }

        [UnityTest]
        public IEnumerator Camera_button()
        {
            camera_button = GameObject.Find("Canvas/CameraButton");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("user_location"));
            ClickUI(camera_button);
            yield return new WaitForSeconds(2f);
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("BlankAR"));
        }

    }
}