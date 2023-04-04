using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private RectTransform listViewTransform;
    [SerializeField] private GameObject buttonPrefab;

    private Database db;

    private void Start()
    {
        if (!db)
        {
            db = FindObjectOfType<Database>();
        }
        
        // Loop through the inventory items and create a button for each item
        float buttonHeight = ((RectTransform)buttonPrefab.transform).rect.height;
        float y = -buttonHeight / 2f;
        foreach (var modelName in db.inventory)
        {
            Debug.Log(modelName);
            // Create a new button instance from the prefab
            GameObject button = Instantiate(buttonPrefab, listViewTransform);

            // Set the text of the button to display the model name and link
            Text modelNameText = button.transform.Find("modelName").GetComponent<Text>();
            modelNameText.text = modelName;

            Text linkText = button.transform.Find("link").GetComponent<Text>();
            linkText.text = "google.com";

            // Set the position of the button based on its height
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(0f, y);
            y -= buttonHeight;
        }
    }
}
