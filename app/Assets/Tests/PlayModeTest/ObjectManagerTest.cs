using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Tests.PlayModeTest
{
    // Test ObjectManager script
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
            // Initialize objects and add to the list
            for (int i = 0; i < 5; i++)
            {
                var objectToSpawn = new GameObject();
                var spawnedObject = Object.Instantiate(objectToSpawn);
                _objectManager.AddObject(spawnedObject);
            }
            
            // Assert
            Assert.AreEqual(_objectManager.ObjectCount, 5);
    
            // Remove all objects
            _objectManager.DeleteObjects();
    
            // Assert
            Assert.AreEqual(_objectManager.ObjectCount, 0);
        }
    
        [Test]
        public void Remove_RemovesSpecifiedObject()
        {
            // Initialize 1 object and add to the list
            var objectToSpawn = new GameObject();
            var spawnedObject = Object.Instantiate(objectToSpawn);
            _objectManager.AddObject(spawnedObject);
    
            // Assert
            Assert.AreEqual(_objectManager.ObjectCount, 1);
            
            // Remove specified object
            _objectManager.Remove(spawnedObject);
    
            // Assert
            Assert.AreEqual(_objectManager.ObjectCount, 0);
        }
    }

}
