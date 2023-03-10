using UnityEngine;

/// <summary>
/// Class that handles click events on spawned objects.
/// </summary>
public class ObjectClickHandler : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject spawnedObject;

    /// <summary>
    /// Method that handles mouse down events on spawned objects. Right now removes the clicked object from the scene.
    /// </summary>
    public void OnMouseDown()
    {
        objectManager.tempPopup.SetText(name);
        objectManager.Remove(spawnedObject);
        Debug.Log(name);
    }
}
