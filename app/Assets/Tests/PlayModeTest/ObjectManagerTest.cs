using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.XR.ARFoundation;

public class ObjectManagerTest
{
    // A Test behaves as an ordinary method
    public class ObjectManagerTests
    {
        private ObjectManager _objectManager;

        [SetUp]
        public void Setup()
        {
            // Initialize object manager with necessary components
            var gameObject = new GameObject();
            _objectManager = gameObject.AddComponent<ObjectManager>();
            _objectManager.GetComponent<ARRaycastManager>();
            _objectManager.GetComponent<ARPlaneManager>();
            _objectManager.enabled = true;
        }

        [Test]
        public void DeleteObjects_RemovesAllSpawnedObjects()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                var objectToSpawn = new GameObject();
                var spawnedObject = Object.Instantiate(objectToSpawn);
                _objectManager.AddObject(spawnedObject);
            }

            // Act
            _objectManager.DeleteObjects();

            // Assert
            Assert.AreEqual(_objectManager.ObjectCount, 0);
        }

        [Test]
        public void Remove_RemovesSpecifiedObject()
        {
            // Arrange
            var objectToSpawn = new GameObject();
            var spawnedObject = Object.Instantiate(objectToSpawn);
            _objectManager.AddObject(spawnedObject);

            // Act
            _objectManager.Remove(spawnedObject);

            // Assert
            Assert.AreEqual(_objectManager.ObjectCount, 0);
        }
    }
}
