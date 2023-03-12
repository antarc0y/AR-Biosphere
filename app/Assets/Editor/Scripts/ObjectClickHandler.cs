using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Class that handles click events on spawned objects.
/// </summary>
public class ObjectClickHandler : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject spawnedObject;

    // Depth of Field settings
    public float focusDistance = 0.5f;
    public float aperture = 2f;

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

            // Blur the background
            PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
            DepthOfField depthOfField = volume.profile.GetSetting<DepthOfField>();
            depthOfField.active = true;
            depthOfField.focusDistance.value = focusDistance;
            depthOfField.aperture.value = aperture;

            // Zoom in the camera
            Camera.main.fieldOfView = zoomFOV;

            isZoomedIn = true;
        }
        else
        {
            // Zoom out the camera
            Camera.main.fieldOfView = 60f;

            // Unblur the background
            PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
            DepthOfField depthOfField = volume.profile.GetSetting<DepthOfField>();
            depthOfField.active = false;

            // Calculate a new position for the object and set it
            Vector3 newPosition = objectManager.GetRandomPosition();
            spawnedObject.transform.position = newPosition;

            // Rotate the object to face the camera
            Vector3 direction = cameraPosition - spawnedObject.transform.position;
            spawnedObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            isZoomedIn = false;
        }
    }
}
