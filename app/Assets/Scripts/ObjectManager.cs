using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;
using TMPro;

/// <summary>
/// Class that handles spawning and deleting objects in the scene.
/// </summary>
public class ObjectManager : MonoBehaviour
{
    /// <summary>
    /// List of prefabs to spawn from
    /// </summary>
    [SerializeField]
    private List<GameObject> objectList = new();
    
    /// <summary>
    /// List of spawned objects in the scene
    /// </summary>
    private List<GameObject> _spawnedObjects = new();
    
    [SerializeField]
    private SwitchToggle switchToggle;
    
    private Camera _mainCamera;
    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;

    // todo: make private? what type?
    //public TextMeshProUGUI tempPopup;
    

    /// <summary>
    /// Maximum number of objects that can be spawned
    /// </summary>
    [SerializeField]
    private int maxObjectCount = 5;
    
    /// <summary>
    /// y position of the spawned objects. This is used to ensure that the objects are spawned on the same plane.
    /// </summary>
    private float y = 0f;

    public GameObject FloatingTextPrefab;
    public Animator objectPopUp;
    public TextMeshProUGUI objectPopUpText;


    private void Awake()
    {
        // Initialize the AR components
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        //tempPopup.SetText("animal name here");
        Debug.Log($"{switchToggle == null}");
        if (!_mainCamera)
        {
            _mainCamera = Camera.main;
        }
    }
    
    private void Update()
    {
        // Spawn objects every 20 frames if the maximum number of objects has not been reached and surface is water
        if (_spawnedObjects.Count < maxObjectCount && Time.frameCount % 20 == 0)
        {
            if (switchToggle.IsOn)
            {
                SpawnObjects(objectList.GetRange(0, 5));
            }
            else
            {
                SpawnObjects(objectList.GetRange(objectList.Count - 5, 5));
            }
            
        }
    }
    
    /// <summary>
    /// Method that spawns objects in the scene in a random location on a detected plane.
    /// </summary>
    private void SpawnObjects(List<GameObject> objectList)
    {
        List<ARRaycastHit> hits = new();
        // Cast ray from a random point within the screen to detect planes
        if (_raycastManager.Raycast(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)),
                hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            // Get spawn position and check if it is valid
            var spawnPosition = hitPose.position;
            if (y == 0f) y = spawnPosition.y;
            else spawnPosition.y = y;
            if (!IsPointValid(spawnPosition)) return;

            // Generate a random rotation around the y-axis only
            Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Align the spawned object with the detected plane
            spawnRotation = Quaternion.FromToRotation(Vector3.up, hitPose.up) * spawnRotation;

            // Select prefabs from list and spawn them, adding them to the list of spawned objects.
            // TODO: Download prefabs from db as AssetBundle instead of hardcoding them.
            var objectToSpawn = objectList[Random.Range(0, objectList.Count)];
            objectToSpawn.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            var spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            AddObject(spawnedObject);

            // Add a click handler to the spawned object
            var clickHandler = spawnedObject.AddComponent<ObjectClickHandler>();
            clickHandler.enabled = true;
            clickHandler.objectManager = this;
            clickHandler.spawnedObject = spawnedObject;
        }
    }

    public void ShowFloatingText(string text, Vector3 position)
    {
        if (FloatingTextPrefab) {
            var go = Instantiate(FloatingTextPrefab, position, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = text;
        }
    }

    public void ShowObjectPopUp(string name)
    {   
        objectPopUpText.SetText("Random info about " + name + ".");
        objectPopUp.SetBool("visible", true);
    }

    public void HideObjectPopUp()
    {
        objectPopUp.SetBool("visible", false);
    }

    
    /// <summary>
    /// Method that checks if a point is a valid location for spawning an object.
    /// </summary>
    /// <param name="point"> <c>Vector3</c> representing the point to check </param>
    /// <returns> <c>bool</c> whether the point is valid</returns>
    private bool IsPointValid(Vector3 point)
    {
        // Check if the point is too close to an existing object
        foreach (var t in _spawnedObjects)
        {
            if (Vector3.Distance(t.transform.position, point) < 0.5f) return false;
        }
        return true;
    }
    
    
    /// <summary>
    /// Method that deletes all spawned objects in the scene and resets the y position.
    /// </summary>
    public void DeleteObjects()
    {
        foreach (var spawnedObject in _spawnedObjects)
        {
            Destroy(spawnedObject);
        }
        _spawnedObjects.Clear();
        y = 0f;
    }
    
    /// <summary>
    /// Method that removes an object from the list of spawned objects and destroys it from the scene.
    /// </summary>
    /// <param name="obj"> GameObject to remove</param>
    public void Remove(GameObject obj)
    {
        _spawnedObjects.Remove(obj);
        Destroy(obj);
    }
    

    private void OnApplicationPause(bool pause)
    {
        if (pause) DeleteObjects();
    }
    
    public int ObjectCount => _spawnedObjects.Count;
    public void AddObject(GameObject obj) => _spawnedObjects.Add(obj);
}
