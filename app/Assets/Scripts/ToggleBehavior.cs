using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections;

public class ToggleBehavior : MonoBehaviour
{
    // disclamer: since specie models are only displayed one at a time using the same dialog box components
    // all models share the same Like toggle object
    // thus if user likes a model, and closes the dialog box to view another model
    // the Like toggle needs to be switched off by the script again during switching model view

    // Components
    private Image heartImage;
    public Toggle toggle;
    // this bool variable is to prevent the toggle event listener from being called
    // when the script needs to turn it off when the user views a different model
    public bool toggleChangedByUser = true;


    // Like button brief expand variables
    public float scaleDuration = 0.2f;
    public float heartInflateScaleAmount = 0.2f;
    public float originalHeartScale;

    // Color change variables
    public float fadeDuration = 0.5f;
    private Color targetColor;
    private Color originalColor;
    

    // Database-related variables
    private string uniqueIdentifier;
    FirebaseFirestore db;

    // Needed to detect the current focused item
    private static ObjectManager objectManager;


    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        heartImage = GetComponent<Image>();
        originalHeartScale = heartImage.transform.localScale.x;

        originalColor = heartImage.color;
        targetColor = originalColor;

        uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;    // Unique device identifier

        if (!objectManager)
        {
            objectManager = FindObjectOfType<ObjectManager>();
        }

        db = FirebaseFirestore.DefaultInstance;
    }

    /// <summary>
    /// Handles the switching of the like button's value. Changes color to red if on, white if off.
    /// </summary>
    void OnToggleValueChanged(bool isOn)
    {
        if (heartImage != null)
        {
            if (isOn)
            {
                targetColor = Color.red;
                if (toggleChangedByUser)
                    handleLiking();

            }
            else
            {
                targetColor = originalColor;
                if (toggleChangedByUser)
                    handleUnliking();
            }

            // This is responsible for the brief-expand effect when clicking on the like button. It simply works by animating the heart getting bigger, and when that animation finishes, it makes the heart smaller again also via an animation.
            heartImage.transform.DOScale(originalHeartScale + heartInflateScaleAmount, scaleDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                heartImage.transform.DOScale(originalHeartScale, scaleDuration).SetEase(Ease.OutQuad);
            });

            // Animation for changing heart color
            heartImage.CrossFadeColor(targetColor, fadeDuration, true, true);
        }
    }

    /// <summary>
    /// Adds the model to the list of liked models in the database for that user's device. This is Union operation,
    /// meaning that models already in the list will not be added again.
    /// </summary>
    async void handleLiking()
    {
        string modelName = objectManager.currentFocused.speciesName;

        DocumentReference docRef = db.Collection("inventories").Document(uniqueIdentifier);

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        //Create user inventory if one does not exist
        if (!snapshot.Exists)
        {
            await docRef.SetAsync(new { models = new List<string>() });
        }

        docRef.UpdateAsync("models", FieldValue.ArrayUnion(modelName)).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Error liking model: " + task.Exception);
            }
            else
            {
                objectManager.currentFocused.isLiked = true;
                Debug.Log("Model liked successfully: " + modelName);
            }
        });
    }

    /// <summary>
    /// Removes the model for the list of liked models in the database for that user's device.
    /// </summary>
    void handleUnliking()
    {
        string modelName = objectManager.currentFocused.speciesName;

        DocumentReference docRef = db.Collection("inventories").Document(uniqueIdentifier);
        docRef.UpdateAsync("models", FieldValue.ArrayRemove(modelName)).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) {
                Debug.LogError("Error unliking model: " + task.Exception);
            } else {
                objectManager.currentFocused.isLiked = false;
                Debug.Log("Model unliked successfully: " + modelName);
            }
        });
    }
}
