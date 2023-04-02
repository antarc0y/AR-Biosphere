using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Firestore;
using Firebase.Extensions;

public class ToggleBehavior : MonoBehaviour
{
    // Components
    private Image heartImage;
    private Toggle toggle;

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
            string model = objectManager.currentFocused.speciesName;
            if (isOn)
            {
                targetColor = Color.red;
                handleLiking(model);

            }
            else
            {
                targetColor = originalColor;
                handleUnliking(model);
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
    async void handleLiking(string model)
    {
        DocumentReference docRef = db.Collection("inventories").Document(uniqueIdentifier);

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        //Create user inventory if one does not exist
        if (!snapshot.Exists)
        {
            await docRef.SetAsync(new { models = new List<string>() });
        }

        docRef.UpdateAsync("models", FieldValue.ArrayUnion(model)).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Error liking model: " + task.Exception);
            }
            else
            {
                Debug.Log("Model liked successfully: " + model);
            }
        });
    }

    /// <summary>
    /// Removes the model for the list of liked models in the database for that user's device.
    /// </summary>
    void handleUnliking(string model)
    {
        DocumentReference docRef = db.Collection("inventories").Document(uniqueIdentifier);
        docRef.UpdateAsync("models", FieldValue.ArrayRemove(model)).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) {
                Debug.LogError("Error unliking model: " + task.Exception);
            } else {
                Debug.Log("Model unliked successfully: " + model);
            }
        });
    }
}
