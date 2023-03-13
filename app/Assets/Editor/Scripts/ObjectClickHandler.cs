using UnityEngine;

/// <summary>
/// Class that handles click events on spawned objects.
/// </summary>
public class ObjectClickHandler : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject spawnedObject;

    // Zoom settings
    public float zoomDistance = 1f;
    public float zoomFOV = 30f;

    // Private state
    private bool isZoomedIn = false;

    /// <summary>
    /// Method that handles mouse down events on spawned objects. Right now removes the clicked object from the scene.
    /// </summary>
    public void OnMouseDown()
    {
        // Get the position and forward vector of the AR camera
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        if (!isZoomedIn)
        {
            // Calculate a new position for the object in front of the camera, a bit closer to the camera
            Vector3 newPosition = cameraPosition + cameraForward * zoomDistance - cameraForward.normalized * 0.1f; // 0.5 units in front and 0.1 units closer to the camera

            // Shift the object downwards
            newPosition += Vector3.down * 0.1f;

            // Set the new position of the object
            spawnedObject.transform.position = newPosition;

            // Rotate the object to face the camera
            Vector3 direction = cameraPosition - spawnedObject.transform.position;
            spawnedObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            // Zoom in the camera
            Camera.main.fieldOfView = zoomFOV;

            isZoomedIn = true;
        }
        else
        {
            // Zoom out the camera
            Camera.main.fieldOfView = 60f;

            // Calculate a new position for the object in front of the camera, a bit further from the camera
            Vector3 newPosition = cameraPosition + cameraForward * (zoomDistance + 0.1f); // 0.1 units further away from the camera

            // Shift the object downwards
            newPosition += Vector3.down * 0.1f;

            // Set the new position of the object
            spawnedObject.transform.position = newPosition;

            // Rotate the object to face the camera
            Vector3 direction = cameraPosition - spawnedObject.transform.position;
            spawnedObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            // Zoom out the camera
            Camera.main.fieldOfView = 60f;

            isZoomedIn = false;
        }
    }
}
