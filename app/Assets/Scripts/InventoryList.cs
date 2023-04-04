using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private RectTransform listViewTransform;
    [SerializeField] private GameObject buttonPrefab;
    private static ObjectManager objectManager;
    private List<Species> inventory = new List<Species>();
    private FirebaseFirestore db;
    private string uniqueIdentifier;


    private void Start()
    {
        uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;    // Unique device identifier
        db = FirebaseFirestore.DefaultInstance;
        GetLikedSpecies();
    }

       /// <summary>
    /// Get species info for each species at location.
    /// </summary>
    private void GetLikedSpecies()
    {
        // Query Firestore for models array in the test document of the inventories collection
        db.Collection("inventories").Document(uniqueIdentifier).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted) Debug.Log("Error getting inventory doc: " + task.Exception);
            else
            {
                var snapshotInventory = task.Result;
                if (snapshotInventory.Exists)
                {
                    List<string> likedStrings = snapshotInventory.GetValue<List<string>>("models");
                    GetSpeciesInfo(likedStrings);
                }
                else
                {
                    List<string> likedStrings = new List<string>();    // If the user's inventory document is not found, that means his collection is empty and he does not have one
                    Debug.Log("No existing inventory for device " + uniqueIdentifier);
                    GetSpeciesInfo(likedStrings);
                }
            }
        });
    }

    /// <summary>
    /// Get species info for each species at location.
    /// </summary>
    private void GetSpeciesInfo(List<string> likedStrings)
    {
        foreach (var speciesName in likedStrings)
        {
            db.Collection("species").Document(speciesName).Listen(snapshot =>
            {
                if (snapshot.Exists) LoadInfo(snapshot, speciesName);
                else Debug.Log("No such document!");
            });
        }
    }

    /// <summary>
    /// Load info from Firestore and download model from Firebase Storage.
    /// </summary>
    /// <param name="snapshot"> DocumentSnapshot to load info from</param>
    /// <param name="speciesName"> Name of species prefab to download</param>
    private void LoadInfo(DocumentSnapshot snapshot, string speciesName)
    {
        var binomial = snapshot.GetValue<string>("binomial");
        var description = snapshot.GetValue<string>("description");
        var link = snapshot.GetValue<string>("link");

        Species species = new Species();
        species.SetInfo(speciesName, binomial, description, link, true);
        inventory.Add(species);
    }

    private void displayButtons() {
        // Loop through the inventory items and create a button for each item
        float buttonHeight = ((RectTransform)buttonPrefab.transform).rect.height;
        float y = -buttonHeight / 2f;
        foreach (var species in inventory)
        {
            Debug.Log("You have liked: "+species.speciesName);
            // Create a new button instance from the prefab
            GameObject button = Instantiate(buttonPrefab, listViewTransform);

            // Set the text of the button to display the model name and link
            Text modelNameText = button.transform.Find("modelName").GetComponent<Text>();
            modelNameText.text = species.speciesName;

            Text linkText = button.transform.Find("link").GetComponent<Text>();
            linkText.text = species.link;

            // Set the position of the button based on its height
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(0f, y);
            y -= buttonHeight;
        }
    }
}
