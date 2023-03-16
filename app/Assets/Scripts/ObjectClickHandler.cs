using UnityEngine;

/// <summary>
/// Class that handles click events on spawned objects.
/// </summary>
public class ObjectClickHandler : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject spawnedObject;
    public int tapCount = 0;

    /// <summary>
    /// Method that handles mouse down events on spawned objects. Right now removes the clicked object from the scene.
    /// </summary>
    public void OnMouseDown()
    {
        //objectManager.tempPopup.SetText(name);
        //objectManager.Remove(spawnedObject);
        tapCount++;
        Debug.Log(name);
        objectManager.ShowFloatingText(name, transform.position);

        if (tapCount >= 2){ 
            objectManager.ShowObjectPopUp(name);
            tapCount = 0;
        }
    }
}
