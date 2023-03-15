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
        var species = spawnedObject.GetComponent<Species>();
        Debug.Log("Clicked on " + species.speciesName + species.description + species.link);
    }
}
