using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace home_screen_ui_tests
{
    public class ui_test: InputTestFixture
    {
        public Mouse mouse;
        public GameObject c_button;
        public override void Setup()
        {
            base.Setup();
            SceneManager.LoadScene("home_screen");
            mouse = InputSystem.AddDevice<Mouse>();
        }

        public void ClickUI(GameObject uiElement)
        {
            Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Vector3 screenpos = camera.WorldToScreenPoint(uiElement.transform.position);
            //Vector3 mousep = Input.mousePosition;
            Set(mouse.position, screenpos);
            Click(mouse.leftButton);
        }
        [UnityTest]
        public IEnumerator continue_button_test()
        {
            c_button = GameObject.Find("Canvas/Begin");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("home_screen"));
            
            ClickUI(c_button);
            yield return new WaitForSeconds(2f);
            Assert.That(SceneManager.GetActiveScene().name,Is.EqualTo("user_location"));
        }

    }
}
