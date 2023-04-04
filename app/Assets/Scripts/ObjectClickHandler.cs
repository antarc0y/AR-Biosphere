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
    public Species species;
    public GameObject spawnedObject;
    public int tapCount = 0;
    public float doubleClickThreshold = 0.3f;

    // Focus settings
    private float focusAnimationDuration = 0.5f;
    private bool isFocused = false;
    private Vector3 originalPosition;   // Both this and originalRotation are used to remember original position and rotation when the user focuses on a model
    private Quaternion originalRotation;
    
    // Position and rotation of the original prefab.
    // This is different from originalPosition and originalRotation, which represent where the model is in the relative scene.
    // Used to set viewing distance and rotation of the model when the user focuses on it.
    private Vector3 prefabPosition;
    private Quaternion prefabRotation;
    private Camera mainCamera;

    // Double click tracker
    private float lastClickTime = 0f;
    
    private void Start()
    {
        if (!objectManager) objectManager = FindObjectOfType<ObjectManager>();
        mainCamera = Camera.main;
        species = GetComponentInParent<Species>();
    }
    
    /// <summary>
    /// Set up the components of the object, called after creation.
    /// </summary>
    /// <param name="o"> The GameObject that this handler is attached to</param>
    /// <param name="position"> The position of the prefab that spawned the parent GameObject</param>
    /// <param name="rotation"> The rotation of the prefab that spawned the parent GameObject</param>
    public void SetUp(GameObject o, Vector3 position, Quaternion rotation)
    {
        spawnedObject = o;
        prefabPosition = position;
        prefabRotation = rotation;
    }
    
    /// <summary>
    /// Method that handles mouse down events on spawned objects. Right now removes the clicked object from the scene.
    /// </summary>
    public void OnMouseDown()
    {
        species = spawnedObject.GetComponent<Species>();

        var timeSinceLastClick = Time.time - lastClickTime;

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

        if (tapCount == 1) HandleSingleClick(); 
        tapCount = 0;
    }

    private void HandleSingleClick()
    {
        if (!isFocused)
        {
            objectManager.ShowFloatingText(species.speciesName, transform.position);
        }
    }

    private void HandleDoubleClick()
    {
        if (!isFocused)
        {
            if (objectManager.clickHandler != null)
            {
                objectManager.clickHandler.UnfocusModel(false);
            }
            objectManager.clickHandler = this;
            focusModel();
        }
        else UnfocusModel(true);
    }

    public void focusModel()
    {
        objectManager.ShowObjectPopUp(species.speciesName, species.binomial, species.description, species.link);

        //Store original position and rotation for when the user zooms out
        originalPosition = spawnedObject.transform.position;
        originalRotation = spawnedObject.transform.rotation;

        // Animate zooming into the position of the prefab
        spawnedObject.transform.SetParent(mainCamera.transform); // set the spawnedObject as a child of the main camera
        spawnedObject.transform.DOLocalMove(prefabPosition, focusAnimationDuration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                isFocused = true;
            });
        
        spawnedObject.transform.DOLocalRotateQuaternion(prefabRotation, focusAnimationDuration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);
    }

    public void UnfocusModel(bool calledFromDoubleClick)
    {
        // unfocusModel can be called from HideObjectPopup leading to infinite recursion, this if statement prevents that.
        if (calledFromDoubleClick)
        {
            objectManager.HideObjectPopUp();    
        }

        // Animate zooming out into the original position of the model
        spawnedObject.transform.SetParent(null);
        spawnedObject.transform.DOLocalMove(originalPosition, focusAnimationDuration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                isFocused = false;
            });

        spawnedObject.transform.DOLocalRotateQuaternion(originalRotation, focusAnimationDuration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);
        
        objectManager.clickHandler = null;
    }

    public void LinkClicked() {
        Application.OpenURL(species.link);
    }
}