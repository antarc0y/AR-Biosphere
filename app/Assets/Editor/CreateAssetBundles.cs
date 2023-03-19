using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles {

    public static string assetBundleDirectory = "Assets/AssetBundles/";

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles() {

        // if main directory doesnt exist create it
        if (Directory.Exists(assetBundleDirectory)) {
            Directory.Delete(assetBundleDirectory, true);
        }
        
        var iOSDirectory = Path.Combine(assetBundleDirectory, "iOS");
        var androidDirectory = Path.Combine(assetBundleDirectory, "Android");
        Directory.CreateDirectory(assetBundleDirectory);
        Directory.CreateDirectory(iOSDirectory);    
        Directory.CreateDirectory(androidDirectory);
        
        //create bundles for all platform (use IOS for editor support on MAC but must be on IOS build platform)
        BuildPipeline.BuildAssetBundles(iOSDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
        Debug.Log("IOS bundle created...");

        BuildPipeline.BuildAssetBundles(androidDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        Debug.Log("Android bundle created...");
        
        AssetDatabase.Refresh();
        Debug.Log("Process complete!");
    }
}