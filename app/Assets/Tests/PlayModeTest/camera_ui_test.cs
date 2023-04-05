using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEditor.Animations;
using JetBrains.Annotations;

namespace blankAR_ui_test
{
    public class ui_test : InputTestFixture
    {
        public Mouse mouse;
        public Toggle toggle;

        public override void Setup()
        {
            //setup the scene to AR_scene and add a mouse for clickability
            base.Setup();
            SceneManager.LoadScene("AR_Scene");
            mouse = InputSystem.AddDevice<Mouse>();
            //GameObject popup_panel = GameObject.Find("Canvas/DialogUI");
            //popup_panel.SetActive(false);
        }

        public void ClickUI(GameObject uiElement)
        {
            //click on an element by finding its position with respectr to the camera in scene.
            Camera camera = GameObject.Find("AR Session Origin/AR Camera").GetComponent<Camera>();
            Vector3 screenpos = camera.WorldToScreenPoint(uiElement.transform.position);
            //Vector2 screenpos = uiElement.AddComponent<RectTransform>().anchoredPosition;
            Set(mouse.position, screenpos);
            Click(mouse.leftButton);
        }
        [UnityTest]
        public IEnumerator back_button_test()
        {
            //the back button to navigate from AR_scene to user_location
            GameObject back_button = GameObject.Find("Canvas/BackButton");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("AR_Scene"));

            ClickUI(back_button);
            yield return new WaitForSeconds(2f);
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("user_location"));
        }

        [UnityTest]
        public IEnumerator RefreshBubbleTest()
        {
            // Find the object in the game hierarchy with the ObjectManager component
            var objectManagerGO = GameObject.Find("AR Session Origin");
            var _objectManager = objectManagerGO.GetComponent<ObjectManager>();

            // Spawn 5 objects using the existing ObjectManager instance
            for (int i = 0; i < 5; i++)
            {
                var objectToSpawn = new GameObject();
                var spawnedObject = Object.Instantiate(objectToSpawn);
                _objectManager.AddObject(spawnedObject);
            }

            // Find the refresh button and click it
            GameObject refresh_bubble = GameObject.Find("Canvas/RefreshBubble");
            ClickUI(refresh_bubble);
            Debug.Log("Object count after refresh: " + _objectManager.ObjectCount);

            // Wait for the objects to be destroyed
            yield return new WaitForSeconds(5f);

            // Check that the _objectManager.ObjectCount property is updated correctly
            Assert.AreEqual(0, _objectManager.ObjectCount);
        }

        [UnityTest]
        public IEnumerator switch_button_test()
        {
            // Arrange
            GameObject switch_handler = GameObject.Find("Canvas/SwitchHandler/Switch background/Toggle Button");
            Toggle switch_value = GameObject.Find("Canvas/SwitchHandler").GetComponent<Toggle>();

            // Assert land
            Assert.IsTrue(!switch_value.isOn);

            // Act
            ClickUI(switch_handler);
            yield return new WaitForSeconds(2f);

            // Assert water
            Assert.IsTrue(switch_value.isOn);
        }

        [UnityTest]
        public IEnumerator info_button_open()
        {
            //tests the opening of the UI popup.
            GameObject info_button = GameObject.Find("Canvas/InformationButton");
            GameObject di = GameObject.Find("Canvas/DialogUI");
            ClickUI(info_button);
            yield return new WaitForSeconds(2f);
            //assert if the ui popup is open
            Assert.IsTrue(di.activeSelf);
        }



    }
}
