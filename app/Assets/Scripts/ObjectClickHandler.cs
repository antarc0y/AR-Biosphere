using UnityEngine;

/// <summary>
/// Class that handles click events on spawned objects.
/// </summary>
public class ObjectClickHandler : MonoBehaviour
{
    private static ObjectManager objectManager;
    public GameObject spawnedObject;

    private void Start()
    {
        if (!objectManager)
        {
            objectManager = GetComponentInParent<ObjectManager>();
        }
    }

    /// <summary>
    /// Method that handles mouse down events on spawned objects. Right now removes the clicked object from the scene.
    /// </summary>
    public void OnMouseDown()
    {
        var species = spawnedObject.GetComponent<Species>();
        Debug.Log("Clicked on " + species.speciesName + species.description + species.link);
    }
}
