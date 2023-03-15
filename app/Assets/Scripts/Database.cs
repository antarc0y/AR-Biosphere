using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine;
using Firebase.Storage;
using Firebase.Firestore;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    private StorageReference _reference;
    private string location = "ualberta";
    private List<string> species;
    private FirebaseFirestore db;
    private List<GameObject> _landPrefabs, _waterPrefabs;
    private Dictionary<string, Dictionary<string, string>> _info;

    private void Awake()
    {
        var storage = FirebaseStorage.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        
#if UNITY_IOS
            _reference = storage.GetReferenceFromUrl("gs://ar-biosphere-cfeaa.appspot.com/iOS/");
#else
            _reference = storage.GetReferenceFromUrl("gs://ar-biosphere-cfeaa.appspot.com/Android/");
#endif
    }

    public void SetUp(UnityAction callback, List<GameObject> landPrefabs, List<GameObject> waterPrefabs, Dictionary<string, Dictionary<string, string>> info)
    {
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
                    GetSpecies(callback);
                }
                else Debug.Log("No such document!");
            }
        });
        
        _landPrefabs = landPrefabs;
        _waterPrefabs = waterPrefabs;
        _info = info;
    }
    
    private void GetSpecies(UnityAction callback)
    {
        foreach (var speciesName in species)
        {
            db.Collection("species").Document(speciesName).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted) Debug.Log("Error getting species doc: " + task.Exception);
                else
                {
                    var snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        var assetName = snapshot.GetValue<string>("assetName");
                        var description = snapshot.GetValue<string>("description");
                        var isLand = snapshot.GetValue<bool>("isLand");
                        var link = snapshot.GetValue<string>("link");

                        _info[assetName] = new Dictionary<string, string>()
                        {
                            {"name", speciesName},
                            {"description", description},
                            {"link", link}
                        };
                        
                        _reference.Child(speciesName)
                            .GetDownloadUrlAsync().ContinueWithOnMainThread( t => {
                                if (!task.IsFaulted && !task.IsCanceled) {
                                    Debug.Log("Download URL: " + task.Result);
                                    var url = t.Result.ToString();
                                    StartCoroutine(DownloadFile(url, assetName, isLand));
                                }
                            });
                    }
                    else Debug.Log("No such document!");
                }
            });
        }
        callback();
    }

    private IEnumerator DownloadFile(string url, string assetName, bool isLand)
    {
        Debug.Log("assetName: " + assetName);
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
            var asset = bundle.LoadAsset<GameObject>(assetName);
            if (isLand) _landPrefabs.Add(asset);
            else _waterPrefabs.Add(asset);
        }
    }
}
