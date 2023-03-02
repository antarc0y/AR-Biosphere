//MIT License - Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
// Made changes as required from the base code.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class GPSLocation : MonoBehaviour
{
    public RawImage rawMap; //RawImage to display the map
    private string apiKey = "AIzaSyBHqm9CstJzpzXe6H8jtioNzXGZbgOXMzk"; //Google API Key

    // Default values
    private static float lat =1.0f;
    private static float lon = 1.0f;
    private int zoom = 15;
    private enum resolution { low = 1, high = 2 };
    private resolution mapResolution = resolution.high;
    private enum type { roadmap, satellite, gybrid, terrain };
    private type mapType = type.roadmap; //Ability to change resolution and type
    private string url = ""; //URL to get the map
    private int mapWidth, mapHeight; //Width and height of the map
    private Rect rect;
    //private bool mapIsLoading = false; //not used. Can be used to know that the map is loading 

    void Start()
    {
        /*
        Get the RawImage component (with its dimensions) and invoke the enumerators to get the location and the map
        */
        GameObject obj = GameObject.Find("Map");
        rawMap = obj.GetComponent<RawImage>();
        rect = rawMap.GetComponent<RectTransform>().rect;
        mapWidth = (int)rect.width;
        mapHeight = (int)rect.height;
        UpdateLatLonMap();
    }

    public void UpdateLatLonMap()
    {
        /*
        Coroutines to get the user location using gps and update the map
        */
        StartCoroutine(InputLocation());
        StartCoroutine(GetGoogleMap());
    }

    public float[] GetLatLon()
    {
        return new float[] {lat, lon};
    }

    private IEnumerator InputLocation()
    {
        /*
        Get the user location using gps
        */

        //Check if the location service is enabled
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start();
            int maxWait = 20; //Max time to wait for the location to be determined
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0 ) //Wait for the location to be determined
            {
                yield return new WaitForSeconds(1.0f); //Wait for 1 second
                maxWait--;
            }
            
            //If the location is not determined after 20 seconds, stop the coroutine
            if (maxWait < 1) 
            {
                Debug.Log( "Timed Out" );
                yield break;
            }
            
            //If the location is not determined, stop the coroutine
            if (Input.location.status == LocationServiceStatus.Failed) 
            {
                Debug.Log("Unable to determine device location");
            }
            
            //If the location is determined, get the latitude and longitude
            else 
            {
                lat = Input.location.lastData.latitude;
                lon = Input.location.lastData.longitude;
            }
            
            Input.location.Stop(); //Stop the location service
        }

        else
        {
            Debug.Log("Location Services are not enabled"); 
        }

    }

    private IEnumerator GetGoogleMap()
    {
        /*
        Get the map from the Google Maps API
        */

        //If the location is not determined, get the location
        if (lat == 1.0f && lon == 1.0f) 
        {
            yield return StartCoroutine(InputLocation());
        }

        //Get the map from the Google Maps API
        url =   "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon + 
                "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + 
                mapResolution + "&maptype=" + mapType + "&markers=" + lat + "," + lon +"&key=" + apiKey;

        //mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url); //Get the map from the url
        yield return www.SendWebRequest(); //Wait for the map to be downloaded

        // Check for errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        
        //If there are no errors, display the map
        else
        {
            //mapIsLoading = false;
            rawMap.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

}