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
        // Get the position and forward vector of the AR camera
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // Calculate a new position for the object in front of the camera, a bit closer to the camera
        Vector3 newPosition = cameraPosition + cameraForward * 0.5f - cameraForward.normalized * 0.1f; // 0.5 units in front and 0.1 units closer to the camera

        // Shift the object downwards
        newPosition += Vector3.down * 0.1f;

        // Set the new position of the object
        spawnedObject.transform.position = newPosition;

        // Rotate the object to face the camera
        Vector3 direction = cameraPosition - spawnedObject.transform.position;
        spawnedObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        Debug.Log("Object moved to new position: " + newPosition.ToString());
    }
}
