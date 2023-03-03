using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class ObjectManager : MonoBehaviour
{
    // List of prefabs to spawn from
    [SerializeField]
    private List<GameObject> objectList = new();
    // List of spawned objects in the scene
    private List<GameObject> _spawnedObjects = new();
    
    private Camera _mainCamera;
    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;

    // Maximum number of objects that can be spawned
    [SerializeField]
    private int maxObjectCount = 5;
    
    private float y = 0f;
    

    private void Awake()
    {
        // Initialize the AR components
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        if (!_mainCamera)
        {
            _mainCamera = Camera.main;
        }
    }
    
    private void Update()
    {
        // Spawn objects every 25 frames if the maximum number of objects has not been reached.
        if (_spawnedObjects.Count < maxObjectCount && Time.frameCount % 20 == 0) SpawnObjects();
    }
    
    private void SpawnObjects()
    {
        List<ARRaycastHit> hits = new();
        // Cast ray from center of screen to detect planes
        if (_raycastManager.Raycast(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)),
                hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            
            // Get spawn position and check if it is valid
            var spawnPosition = hitPose.position;
            if (y == 0f) y = spawnPosition.y;
            else spawnPosition.y = y;
            if (!IsPointValid(spawnPosition)) return;
            
            // Align the spawned object with the detected plane
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hitPose.up);
        
            // Select prefabs from list and spawn them, adding them to the list of spawned objects.
            // TODO: Add a check to make sure the spawned object is not too close to another object.
            // TODO: Download prefabs from db as AssetBundle instead of hardcoding them.
            var objectToSpawn = objectList[Random.Range(0, objectList.Count)];
            objectToSpawn.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            var spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            _spawnedObjects.Add(spawnedObject);
            
            // Add a click handler to the spawned object
            ObjectClickHandler clickHandler = spawnedObject.AddComponent<ObjectClickHandler>();
            clickHandler.objectManager = this;
            clickHandler.enabled = true;
            clickHandler.spawnedObject = spawnedObject;
        }
    }
    
    // Check if a point on a horizontal plane.
    private bool IsPointValid(Vector3 point)
    {
        var screenPoint = _mainCamera.WorldToScreenPoint(point);
        var ray = _mainCamera.ScreenPointToRay(screenPoint);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (_raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            var plane = _planeManager.GetPlane(hits[0].trackableId);
            if (plane.alignment != PlaneAlignment.HorizontalUp) return false;
        }

        for (var i = 0; i < _spawnedObjects.Count; i++)
        {
            if (Vector3.Distance(_spawnedObjects[i].transform.position, point) < 0.5f) return false;
        }
        return true;
    }
    

    private void OnGUI()
    {
        // Display the number of spawned objects
        GUI.Label(new Rect(40, 40, 200, 50), $"Objects: {_spawnedObjects.Count}");
    }
    
    // Delete all spawned objects. Called on button click.
    public void DeleteObjects()
    {
        foreach (GameObject spawnedObject in _spawnedObjects)
        {
            Destroy(spawnedObject);
        }
        _spawnedObjects.Clear();
        y = 0f;
    }
    
    private void OnApplicationPause(bool pause)
    {
        if (pause) DeleteObjects();
    }

    public void Remove(GameObject o)
    {
        _spawnedObjects.Remove(o);
        Destroy(o);
    }
}
