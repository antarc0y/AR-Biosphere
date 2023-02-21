using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectList = new();
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new();
    private List<GameObject> spawnedObjects = new();
    private Camera _mainCamera;

    [SerializeField]
    private int maxObjectCount = 5;
    // private int currentObjectCount;
    private float _minDistance = 0.5f;
    private float _maxDistance = 5.0f;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        if (!_mainCamera) _mainCamera = Camera.main;
    }

    void Update()
    {
        // Expensive operation?
        if (spawnedObjects.Count > 0) return;
        SpawnObjects();
    }
    
    public void SpawnObjects()
    {
        if (raycastManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2f), hits,
                TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            while (spawnedObjects.Count < maxObjectCount)
            {
                // Spawn the object randomly around the detected plane
                Vector3 spawnPosition = hitPose.position + Random.insideUnitSphere * Random.Range(_minDistance, _maxDistance);
                spawnPosition.y = hitPose.position.y;
                
                // Align the spawned object with the detected plane
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hitPose.up);

                
                GameObject objectToSpawn = objectList[Random.Range(0, objectList.Count)];
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
                spawnedObjects.Add(spawnedObject);
            }
        }
    }
}
