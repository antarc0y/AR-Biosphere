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
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    

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
            // Store the original position and rotation of the object
            originalPosition = spawnedObject.transform.position;
            originalRotation = spawnedObject.transform.rotation;

            // Make the object a child of the camera and set its position and rotation relative to the camera
            spawnedObject.transform.SetParent(Camera.main.transform);
            spawnedObject.transform.localPosition = Vector3.forward * zoomDistance;
            spawnedObject.transform.localRotation = Quaternion.identity;

            // Zoom in the camera
            Camera.main.fieldOfView = zoomFOV;

            isZoomedIn = true;
        }
        else
        {
            // Zoom out the camera
            Camera.main.fieldOfView = -zoomFOV;

            // Make the object not a child of the camera and restore its original position and rotation
            spawnedObject.transform.SetParent(null);
            spawnedObject.transform.position = originalPosition;
            spawnedObject.transform.rotation = originalRotation;

            isZoomedIn = false;
        }
    }
}
