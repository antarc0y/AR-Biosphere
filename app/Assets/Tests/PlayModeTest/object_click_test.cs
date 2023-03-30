using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using JetBrains.Annotations;

namespace click_object_test
{
    public class ui_test : InputTestFixture
    {
        public Mouse mouse;
        public GameObject spawned_obj;

        public override void Setup()
        {
            base.Setup();
            SceneManager.LoadScene("BlankAR");
            mouse = InputSystem.AddDevice<Mouse>();
            //GameObject popup_panel = GameObject.Find("Canvas/DialogUI");
            //popup_panel.SetActive(false);
        }

        public void ClickUI(GameObject uiElement)
        {
            Camera camera = GameObject.Find("AR Session Origin/AR Camera").GetComponent<Camera>();
            Vector3 screenpos = camera.WorldToScreenPoint(uiElement.transform.position);
            //Vector2 screenpos = uiElement.AddComponent<RectTransform>().anchoredPosition;
            Set(mouse.position, screenpos);
            Click(mouse.leftButton);
        }
        [UnityTest]
        public IEnumerator click_object_name()
        {
            var objectManagerGO = GameObject.Find("AR Session Origin");
            

            var objectToSpawn = new GameObject();
            var spawnedObject = Object.Instantiate(objectToSpawn);
            //ObjectManager _objectManager = new ObjectManager;
            //_objectManager.GetComponent<ARRaycastManager>();
            //_objectManager.GetComponent<ARPlaneManager>();
            //_objectManager.enabled = true;

            Camera camera = GameObject.Find("AR Session Origin/AR Camera").GetComponent<Camera>();
            Vector3 screenpos = camera.WorldToScreenPoint(spawnedObject.transform.position);

            // Get spawn position and check if it is valid
            Set(mouse.position,screenpos);
            Click(mouse.leftButton);
            yield return new WaitForSeconds(2f);
            //var species = spawnedObject.GetComponent<Species>();
            //Debug.Log(species);
            Assert.NotNull(spawnedObject.name);
            //Assert.That();
        }

    }
}