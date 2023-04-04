using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private RectTransform listViewTransform;
    [SerializeField] private GameObject buttonPrefab;
    private static ObjectManager objectManager;
    private List<Species> inventory;

    private Database db;

    private void Start()
    {
        if (!objectManager) objectManager = FindObjectOfType<ObjectManager>();
        inventory = objectManager.likedObjects;
        Debug.Log(inventory.Count);

        
        // Loop through the inventory items and create a button for each item
        float buttonHeight = ((RectTransform)buttonPrefab.transform).rect.height;
        float y = -buttonHeight / 2f;
        foreach (var species in inventory)
        {
            Debug.Log(species.speciesName);
            // Create a new button instance from the prefab
            GameObject button = Instantiate(buttonPrefab, listViewTransform);

            // Set the text of the button to display the model name and link
            Text modelNameText = button.transform.Find("modelName").GetComponent<Text>();
            modelNameText.text = species.speciesName;

            Text linkText = button.transform.Find("link").GetComponent<Text>();
            linkText.text = "google.com";

            // Set the position of the button based on its height
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(0f, y);
            y -= buttonHeight;
        }
    }
}
