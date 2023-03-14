using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Storage;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    private string _url = "";
    private StorageReference _reference;
    private void Awake()
    {
        // _url = Path.Combine(Application.persistentDataPath, "chicken-Android");
        var storage = FirebaseStorage.DefaultInstance;
        _reference = storage.GetReferenceFromUrl("gs://ar-biosphere-cfeaa.appspot.com/Android/chicken-Android");
    }

    public void GetAssetBundle(UnityAction<GameObject> callback)
    {
        // Fetch the download URL
        _reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) => {
            if (!task.IsFaulted && !task.IsCanceled) {
                Debug.Log("Download URL: " + task.Result);
                _url += task.Result.ToString();
            }
        });
        StartCoroutine(DownloadFile(callback));
    }

    IEnumerator DownloadFile(UnityAction<GameObject> callback)
    {
        if (_url == "")
        {
            yield return new WaitUntil(() => _url != "");
        }
        Debug.Log("starting download");
        var www = UnityWebRequestAssetBundle.GetAssetBundle(_url, 1, 0);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Get downloaded asset bundle
            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            var asset = bundle.LoadAsset<GameObject>("ChickenBrown");
            Debug.Log($"asset: {asset == null}");
            callback(asset);
        }
    }
}
