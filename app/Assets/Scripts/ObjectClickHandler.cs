using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Class that handles click events on spawned objects.
/// </summary>
public class ObjectClickHandler : MonoBehaviour
{
    private static ObjectManager objectManager;
    private Species species;
    public GameObject spawnedObject;
    public int tapCount = 0;
    public float doubleClickThreshold = 0.3f;

    private void Start()
    {
        if (!objectManager)
        {
            objectManager = FindObjectOfType<ObjectManager>();
        }
        species = GetComponentInParent<Species>();
        Debug.Log($"objectmanager is {objectManager == null}");
    }

    // Zoom settings
    public float zoomDistance = 1.5f;
    public float zoomDuration = 0.5f;

    // Blur settings
    public Image blurBackground;
    public float blurStrength = 0.5f;
    public float blurSize = 4f;

    // Private state
    private bool isZoomedIn = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    private float lastClickTime = 0f;

    /// <summary>
    /// Method that handles mouse down events on spawned objects. Right now removes the clicked object from the scene.
    /// </summary>
    public void OnMouseDown()
    {
        var species = spawnedObject.GetComponent<Species>();
        Debug.Log("Clicked on " + species.speciesName + species.description + species.link);

        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick < doubleClickThreshold)
        {
            // Handle double click
            HandleDoubleClick();
            tapCount = 0;
        }
        else
        {
            // Handle single click
            tapCount++;
            StartCoroutine(DoubleClickCoroutine());
        }

        lastClickTime = Time.time;
    }

    private IEnumerator DoubleClickCoroutine()
    {
        yield return new WaitForSeconds(doubleClickThreshold);

        if (tapCount == 1)
        {
            // Handle single click
            HandleSingleClick();
        }

        tapCount = 0;
    }

    private void HandleSingleClick()
    {
        if (!isZoomedIn)
        {
            objectManager.ShowFloatingText(species.speciesName, transform.position);
        }
    }

    private void HandleDoubleClick()
    {
        if (!isZoomedIn)
        {
            objectManager.ShowObjectPopUp(species.speciesName, species.binomial, species.description, species.link);

            //Store original position and rotation for when the user zooms out
            originalPosition = spawnedObject.transform.position;
            originalRotation = spawnedObject.transform.rotation;

            // Animate zooming in
            spawnedObject.transform.SetParent(Camera.main.transform); // set the spawnedObject as a child of the main camera
            spawnedObject.transform.DOLocalMove(Vector3.forward * zoomDistance, zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isZoomedIn = true;
                });

            spawnedObject.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 180f, 0f), zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true);

            // Enable the blur effect
            if (blurBackground != null)
            {
                blurBackground.material.SetFloat("_BlurStrength", blurStrength);
                blurBackground.material.SetFloat("_BlurSize", blurSize);
                blurBackground.gameObject.SetActive(true);
            }
        }
        else
        {
            objectManager.HideObjectPopUp();

            // Animate zooming out
            spawnedObject.transform.SetParent(null);
            spawnedObject.transform.DOLocalMove(originalPosition, zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isZoomedIn = false;
                });

            spawnedObject.transform.DOLocalRotateQuaternion(originalRotation, zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true);

            // Disable the blur effect
            if (blurBackground != null)
            {
                blurBackground.gameObject.SetActive(false);
            }
        }
    }
}
