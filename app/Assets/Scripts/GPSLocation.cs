//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class GPSLocation : MonoBehaviour
{
    public RawImage rawMap;
    private string apiKey = "AIzaSyBHqm9CstJzpzXe6H8jtioNzXGZbgOXMzk";
    private float lat = 53.54708384949384f; //Edmonton downtown roger's place
    private float lon = -113.4978218588264f;
    private int zoom = 15;
    private enum resolution { low = 1, high = 2 };
    private resolution mapResolution = resolution.high;
    private enum type { roadmap, satellite, gybrid, terrain };
    private type mapType = type.roadmap;
    private string url = "";
    private int mapWidth = 640;
    private int mapHeight = 640;
    //private bool mapIsLoading = false; //not used. Can be used to know that the map is loading 

    void Start()
    {
        // https://stackoverflow.com/a/48709735
        // this script is attached to both GPSButton and Map; specify using Map
        GameObject obj = GameObject.Find("Map");
        rawMap = obj.GetComponent<RawImage>();
        StartCoroutine(GetGoogleMap());
    }

    public void onGPSClick(){
        lat = 53.51890438485273f;
        lon = -113.51914288581222f; //Try testing manually with these coordinates first before working with the GPS
        GameObject obj = GameObject.Find("Map");
        rawMap = obj.GetComponent<RawImage>();
        //StartCoroutine(InputLocation());
        StartCoroutine(GetGoogleMap());
    }


    private IEnumerator InputLocation()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start();
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0 )
            {
                yield return new WaitForSeconds(1.0f);
                maxWait--;
            }
            
            if (maxWait < 1)
            {
                Debug.Log( "Timed Out" );
                yield break;
            }
            
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
            }
            
            else
            {
                lat = Input.location.lastData.latitude;
                lon = Input.location.lastData.longitude;
            }
            
            Input.location.Stop();
        }

        else
        {
            Debug.Log("Location Services are not enabled");
        }

    }


    private IEnumerator GetGoogleMap()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + mapResolution + "&maptype=" + mapType + "&key=" + apiKey;
        //mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            //mapIsLoading = false;
            rawMap.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

}