using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
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

    private List<ARRaycastHit> _hits = new();
    
    private Camera _mainCamera;
    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;

    // Maximum number of objects that can be spawned
    [SerializeField]
    private int maxObjectCount = 5;
    
    // Distance for random spawn location
    private float _minDistance = 0.5f;
    private float _maxDistance = 3.0f;

    private void Awake()
    {
        // Initialize the AR components
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        if (!_mainCamera)
        {
            _mainCamera = Camera.main;
            _mainCamera.enabled = true;
        }
    }

    private void Update()
    {
        // Spawn objects every 25 frames if the maximum number of objects has not been reached.
        if (_spawnedObjects.Count == maxObjectCount) return;
        if (Time.frameCount % 25 == 0) SpawnObjects();
    }
    
    private void SpawnObjects()
    {
        // Cast ray from center of screen to detect planes
        if (_raycastManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2f), _hits,
                TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _hits[0].pose;

            // Determine spawn location randomly around the detected plane, within the screen boundaries.
            Vector3 spawnPosition = new Vector3();
            do
            {
                spawnPosition = hitPose.position + Random.insideUnitSphere * Random.Range(_minDistance, _maxDistance);
                spawnPosition.y = hitPose.position.y;
            } while(!IsPointWithinScreen(spawnPosition));
            
            // Align the spawned object with the detected plane
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hitPose.up);
        
            // Select prefabs from list and spawn them, adding them to the list of spawned objects.
            // TODO: Add a check to make sure the spawned object is not too close to another object.
            // TODO: Download prefabs from db as AssetBundle instead of hardcoding them.
            GameObject objectToSpawn = objectList[Random.Range(0, objectList.Count)];
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            _spawnedObjects.Add(spawnedObject);
        }
    }
    
    // Check if a point is within the screen boundaries and on a horizontal plane.
    private bool IsPointWithinScreen(Vector3 point)
    {
        var screenPoint = _mainCamera.WorldToScreenPoint(point);
        var isPointInScreen = screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
               screenPoint.y >= 0 && screenPoint.y <= Screen.height &&
               screenPoint.z > 0;
        if (isPointInScreen)
        {
            var ray = _mainCamera.ScreenPointToRay(screenPoint);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (_raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                var plane = _planeManager.GetPlane(hits[0].trackableId);
                return plane.alignment == PlaneAlignment.HorizontalUp;
            }
        }
        return false;
    }

    // Delete all spawned objects. Called on button click.
    public void DeleteObjects()
    {
        foreach (GameObject spawnedObject in _spawnedObjects)
        {
            Destroy(spawnedObject);
        }
        _spawnedObjects.Clear();
    }
}
