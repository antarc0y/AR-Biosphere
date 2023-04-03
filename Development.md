This is a brief developer's guide to the AR Biosphere project. It assumes you have little to no knowledge of Unity.
# Environment
This project was built with Unity 2021.03.19f1. You should use the same version of Unity or higher to avoid compatibility issues.

This project also uses Firebase, which is not included in the repository because of size and dependency issues. To import it to your local project, follow the instructions in the [Firebase Unity SDK](https://firebase.google.com/docs/unity/setup) documentation.
(Step 1-3 are already done in the repository, so you only need to follow step 4.)

# Project Structure
The Unity project is located in the `app/` folder.
Most of the workflow is done in the `app/Assets/` folder.
## Scenes
You can think of a scene in Unity like an activity in Android. A scene contains a collection of GameObjects organized into a hierarchy
Some of the notable objects in the hierarchy include:
 * `Camera`: The camera that renders the scene. 
 * `AR Session Origin` and `AR Session`: These objects manage the AR scene and contain an AR camera.
 * `Canvas`: The UI canvas that contains all the UI elements. Should go with an `EventSystem` object.
 * Custom objects: These are the objects that we create in the project ourselves.
## Prefabs
Prefabs are reusable objects that can be instantiated in the scene. They are useful for creating objects that are used multiple times in the scene.

In the context of this project, prefabs are used to create the 3D models that are placed in the scene. See `Assets/Scripts/ObjectManager.cs` to see how prefabs are placed in the scene.

### Asset Bundles
In this project we do not put prefabs directly in the scene. Instead, we use asset bundles to load the prefabs into the scene.
Asset bundles are stored in Firebase Storage and downloaded to the device and unpacked when needed. This is done in `Assets/Scripts/Database.cs`.


To create an Asset Bundle, go to `Assets/BuildAssetBundles` in the Unity Editor. This will create the bundles in the `app/Assets/AssetBundles` folder for both Android and iOS. You can then upload them to Firebase Storage.
The `Assets/Editor/BuildAssetBundles.cs` script is used for the build process.  

Tutorial: https://youtu.be/twzfpuaM-Js

**Note**: the current script for database does not handle offline connection and the layout of the database requires a large number of reads. This is something that could be improved.

# Scripts
Runtime C# scripts can be found in the `Assets/Scripts` folder. 

Classes that inherit from `MonoBehaviour` can be attached to GameObjects in the scene. 
Instances of these classes cannot be created directly via a constructor, but are created by Unity when the GameObject is instantiated in the scene.

### Tests

# UI
### Animations

