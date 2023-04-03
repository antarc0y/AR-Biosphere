using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private RectTransform listViewTransform;
    [SerializeField] private GameObject buttonPrefab;

    // Database-related variables
    private string uniqueIdentifier;
    FirebaseFirestore db;

    private void Start()
    {
        DocumentReference docRef = db.Collection("inventories").Document(uniqueIdentifier);

        // Get the inventory list from Firebase
        docRef.GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to retrieve inventory list from Firebase.");
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            IDictionary<string, object> inventoryDict = snapshot.ToDictionary();

            // Loop through the inventory items and create a button for each item
            float buttonHeight = ((RectTransform)buttonPrefab.transform).rect.height;
            float y = -buttonHeight / 2f;
            foreach (var kvp in inventoryDict)
            {
                string modelName = kvp.Key;
                string link = kvp.Value.ToString();

                // Create a new button instance from the prefab
                GameObject button = Instantiate(buttonPrefab, listViewTransform);

                // Set the text of the button to display the model name and link
                Text modelNameText = button.transform.Find("modelName").GetComponent<Text>();
                modelNameText.text = modelName;

                Text linkText = button.transform.Find("link").GetComponent<Text>();
                linkText.text = link;

                // Set the position of the button based on its height
                RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
                buttonRectTransform.anchoredPosition = new Vector2(0f, y);
                y -= buttonHeight;
            }
        });
    }
}
