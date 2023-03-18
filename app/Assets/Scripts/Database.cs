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
    private FirebaseFirestore db;
    
    /// <summary>
    /// List of prefabs to spawn from
    /// </summary>
    private List<GameObject> _landPrefabs, _waterPrefabs;
    
    /// <summary>
    /// Dictionary containing species info
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> _info;

    private void Awake()
    {
        // Initialize Firebase
        var storage = FirebaseStorage.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        
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
        db.Collection("locations").Document(location).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error getting location doc: " + task.Exception);
            }
            else
            {
                var snapshot = task.Result;
                if (snapshot.Exists)
                {
                    species = snapshot.GetValue<List<string>>("species");
                    GetSpecies();
                }
                else Debug.Log("No such document!");
            }
        });
        
        _landPrefabs = landPrefabs;
        _waterPrefabs = waterPrefabs;
        _info = info;
    }
    
    /// <summary>
    /// Get species info for each species at location.
    /// </summary>
    private void GetSpecies()
    {
        foreach (var speciesName in species)
        {
            db.Collection("species").Document(speciesName).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted) Debug.Log("Error getting species doc: " + task.Exception);
                else
                {
                    var snapshot = task.Result;
                    if (snapshot.Exists) LoadInfo(snapshot, speciesName);
                    else Debug.Log("No such document!");
                }
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
        var description = snapshot.GetValue<string>("description");
        var isLand = snapshot.GetValue<bool>("isLand");
        var link = snapshot.GetValue<string>("link");

        // Create dictionary for species info
        _info[assetName] = new Dictionary<string, string>()
        {
            {"name", speciesName},
            {"description", description},
            {"link", link}
        };
        
        // Download model from Firebase Storage
        _reference.Child(speciesName).GetDownloadUrlAsync().ContinueWithOnMainThread( t => {
            if (!t.IsFaulted && !t.IsCanceled) {
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
            // Get downloaded asset bundle
            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            var x = bundle.GetAllAssetNames();
            var asset = bundle.LoadAsset<GameObject>(assetName);
            
            // Add to list of prefabs
            if (isLand) _landPrefabs.Add(asset);
            else _waterPrefabs.Add(asset);
        }
    }
}
