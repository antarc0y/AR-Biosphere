using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using UnityEngine;
using Firebase.Storage;
using Firebase.Firestore;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    private StorageReference _reference;

    /// <summary>
    /// Location of the user. Hardcoded for now.
    /// </summary>
    private const string location = "ualberta";

    /// <summary>
    /// Name of the species to spawn
    /// </summary>
    private List<string> species;

    /// <summary>
    /// Names of the liked species
    /// </summary>
    public List<string> inventory;

    private FirebaseFirestore db;

    /// <summary>
    /// List of prefabs to spawn from
    /// </summary>
    private List<GameObject> _landPrefabs, _waterPrefabs;

    /// <summary>
    /// Dictionary containing species info
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> _info;

    private string uniqueIdentifier;

    private void Awake()
    {
        uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;    // Unique device identifier

        // Initialize Firebase
        var storage = FirebaseStorage.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        
        // Use this if models are not downloaded/spawned
        // if(Caching.ClearCache()) Debug.Log("Cache cleared");
        // else Debug.Log("Cache not cleared");
        
#if UNITY_IOS
            _reference = storage.GetReferenceFromUrl("gs://ar-biosphere-cfeaa.appspot.com/iOS/");
#else
        _reference = storage.GetReferenceFromUrl("gs://ar-biosphere-cfeaa.appspot.com/Android/");
#endif
    }

    /// <summary>
    /// Called from ObjectManager.cs. Downloads info from Firestore and models from Firebase Storage.
    /// </summary>
    /// <param name="landPrefabs">List of prefabs to populate for land species </param>
    /// <param name="waterPrefabs"> List of prefabs to populate for water species</param>
    /// <param name="info"> Dictionary of information for each species</param>
    public void SetUp(List<GameObject> landPrefabs, List<GameObject> waterPrefabs,
        Dictionary<string, Dictionary<string, string>> info)
    {
        // Get names of species at location
        db.Collection("locations").Document(location)
            .Listen(snapshot =>
            {
                species = snapshot.GetValue<List<string>>("species");
                GetSpeciesWithLikeChecking();
            });

        _landPrefabs = landPrefabs;
        _waterPrefabs = waterPrefabs;
        _info = info;
    }

    /// <summary>
    /// Get species info for each species at location.
    /// </summary>
    private void GetSpeciesWithLikeChecking()
    {
        // Query Firestore for models array in the test document of the inventories collection
        db.Collection("inventories").Document(uniqueIdentifier).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted) Debug.Log("Error getting inventory doc: " + task.Exception);
            else
            {
                var snapshotInventory = task.Result;
                if (snapshotInventory.Exists)
                {
                    inventory = snapshotInventory.GetValue<List<string>>("models");
                    Debug.Log(inventory[0]);
                    GetSpecies(inventory);
                }
                else
                {
                    inventory = new List<string>();    // If the user's inventory document is not found, that means his collection is empty and he does not have one
                    Debug.Log("No existing inventory for device " + uniqueIdentifier);
                    GetSpecies(inventory);
                }
            }
        });
    }

    /// <summary>
    /// Get species info for each species at location.
    /// </summary>
    private void GetSpecies(List<string> inventory)
    {
        foreach (var speciesName in species)
        {
            db.Collection("species").Document(speciesName).Listen(snapshot =>
            {
                if (snapshot.Exists) LoadInfo(snapshot, speciesName);
                else Debug.Log("No such document!");
            });
        }
    }

    /// <summary>
    /// Load info from Firestore and download model from Firebase Storage.
    /// </summary>
    /// <param name="snapshot"> DocumentSnapshot to load info from</param>
    /// <param name="speciesName"> Name of species prefab to download</param>
    private void LoadInfo(DocumentSnapshot snapshot, string speciesName)
    {
        var assetName = snapshot.GetValue<string>("assetName");
        var binomial = snapshot.GetValue<string>("binomial");
        var description = snapshot.GetValue<string>("description");
        var isLand = snapshot.GetValue<bool>("isLand");
        var link = snapshot.GetValue<string>("link");

        // Create dictionary for species info
        _info[assetName] = new Dictionary<string, string>()
        {
            {"name", speciesName},
            {"binomial", binomial},
            {"description", description},
            {"link", link},
            {"isLiked", inventory.Contains(speciesName).ToString()}
        };

        // Download model from Firebase Storage
        _reference.Child(speciesName.Replace(' ', '_')).GetDownloadUrlAsync()
            .ContinueWithOnMainThread( t => {
                if (!t.IsFaulted && !t.IsCanceled)
                {
                    var url = t.Result.ToString();
                    StartCoroutine(DownloadFile(url, assetName, isLand));
                }
        });
    }

    /// <summary>
    /// Coroutine to download model from Firebase Storage.
    /// </summary>
    /// <param name="url"> Download URL</param>
    /// <param name="assetName"> Name of asset to load from AssetBundle</param>
    /// <param name="isLand"> Whether species is on land</param>
    /// <returns></returns>
    private IEnumerator DownloadFile(string url, string assetName, bool isLand)
    {
        var www = UnityWebRequestAssetBundle.GetAssetBundle(url, 1, 0);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Get downloaded asset bundle. Note that asset bundles are downloaded and stored in app cache, so they will not be downloaded again.
            // Unity checks the cache before downloading, differentiating between different versions by version number or hash. 
            // See UnityWebRequestAssetBundle.GetAssetBundle for more info.
            // Cache can be cleared by calling Caching.ClearCache() or in the app settings.
            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            var x = bundle.GetAllAssetNames();
            var asset = bundle.LoadAsset<GameObject>(assetName);
            
            if (asset == null) Debug.Log($"Null asset: {assetName}, {x[0]}");
            else
            {
                // Add to list of prefabs
                if (isLand) _landPrefabs.Add(asset);
                else _waterPrefabs.Add(asset);
                bundle.Unload(false);
            }
        }
    }
}
