using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Firebase;
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
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        heartImage = GetComponent<Image>();
        originalHeartScale = heartImage.transform.localScale.x;

        originalColor = heartImage.color;
        targetColor = originalColor;

        uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;    // Unique device identifier
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
                handleLiking();

            }
            else
            {
                targetColor = originalColor;
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

    void handleLiking()
    {
        DocumentReference docRef = db.Collection("inventories").Document(uniqueIdentifier);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists) {
                Dictionary<string, object> city = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city) {
                Debug.Log("AAAAAAAAAAAAAAAAAA" + pair.ToString());
                }
            } else {
                Debug.Log("Document" + snapshot.Id + "does not exist!");
            }
        });
    }
}
