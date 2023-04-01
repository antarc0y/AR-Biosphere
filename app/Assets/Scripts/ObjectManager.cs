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
    private List<GameObject> landModels = new(), waterModels = new();
    
    /// <summary>
    /// List of spawned objects in the scene
    /// </summary>
    private List<GameObject> _spawnedObjects = new();
    
    [SerializeField]
    private SwitchToggle switchToggle;
    
    private Camera _mainCamera;
    private ARRaycastManager _raycastManager;

    // todo: make private? what type?
    //public TextMeshProUGUI tempPopup;
    
    private Database _database;
    
    private Dictionary<string, Dictionary<string, string>> _speciesInfo = new();

    /// <summary>
    /// Maximum number of objects that can be spawned
    /// </summary>
    [SerializeField]
    private int maxObjectCount = 5;
    public bool popUpIsBeingShown = false;
    public Species currentFocused;
    
    /// <summary>
    /// y position of the spawned objects. This is used to ensure that the objects are spawned on the same plane.
    /// </summary>
    private float _y = 0f;

    public GameObject FloatingTextPrefab;
    public Animator objectPopUp;
    public TextMeshProUGUI objectPopUpText;

    public ObjectClickHandler clickHandler;


    private void Start()
    {
        // Initialize the AR components
        _raycastManager = GetComponent<ARRaycastManager>();
        _database = GetComponent<Database>();
        
        _database.SetUp(landModels, waterModels, _speciesInfo);
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
            if (switchToggle.IsOn) SpawnObjects(false);
            else SpawnObjects(true);
        }
    }
    
    /// <summary>
    /// Method that spawns objects in the scene in a random location on a detected plane.
    /// </summary>
    private void SpawnObjects(bool isLand)
    {
        List<ARRaycastHit> hits = new();
        var objectList = isLand ? landModels : waterModels;
        if (objectList.Count == 0) return;

        // Cast ray from a random point within the screen to detect planes
        if (_raycastManager.Raycast(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)),
                hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            // Get spawn position and check if it is valid
            var spawnPosition = hitPose.position;
            if (_y == 0f) _y = spawnPosition.y;
            else spawnPosition.y = _y;
            if (!IsPointValid(spawnPosition)) return;

            // Generate a random rotation around the y-axis only
            Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Align the spawned object with the detected plane
            spawnRotation = Quaternion.FromToRotation(Vector3.up, hitPose.up) * spawnRotation;

            // Select prefabs from list and spawn them, adding them to the list of spawned objects.
            var objectToSpawn = objectList[Random.Range(0, objectList.Count)];
            objectToSpawn.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            var spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            AddObject(spawnedObject);

            // Add a click handler to the spawned object
            clickHandler = spawnedObject.AddComponent<ObjectClickHandler>();
            clickHandler.spawnedObject = spawnedObject;
            
            // Add a species component to the spawned object
            var species = spawnedObject.AddComponent<Species>();
            var modelName = spawnedObject.name.Replace("(Clone)", "");
            species.SetInfo(
                _speciesInfo[modelName]["name"],
                _speciesInfo[modelName]["binomial"],
                _speciesInfo[modelName]["description"], 
                _speciesInfo[modelName]["link"],
                float.Parse(_speciesInfo[modelName]["focusDistance"])
                );
        }
    }

    public void ShowFloatingText(string text, Vector3 position)
    {
        if (FloatingTextPrefab) {
            var go = Instantiate(FloatingTextPrefab, position, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = text;
        }
    }

    public void ShowObjectPopUp(Species species)
    {
        popUpIsBeingShown = true;
        currentFocused = species;

        string formattedText = species.speciesNameCapitalized + " (<i>" + species.binomial + "</i>)\n" +
                               species.description + "\n" +
                               "More info:" + species.link;
        objectPopUpText.SetText(formattedText);
        objectPopUp.SetBool("visible", true);
    }


    public void HideObjectPopUp()
    {
        clickHandler.unfocusModel(false);
        popUpIsBeingShown = false;
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
        if (popUpIsBeingShown) 
        {
            HideObjectPopUp();
        }
        
        foreach (var spawnedObject in _spawnedObjects)
        {
            Destroy(spawnedObject);
        }
        _spawnedObjects.Clear();
        _y = 0f;
        // Debug.Log("Object count after deletion: " + _spawnedObjects.Count);
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
