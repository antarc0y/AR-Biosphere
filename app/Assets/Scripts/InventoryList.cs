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
    List<string> likedStrings;
    private List<Species> inventory = new List<Species>();
    int loadedSpeciesCount = 0; // Need to track how many have been loaded so far, only want to display buttons once ALL species have been loaded.
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
                    likedStrings = snapshotInventory.GetValue<List<string>>("models");
                    GetSpeciesInfo(likedStrings);
                }
                else
                {
                    likedStrings = new List<string>();    // If the user's inventory document is not found, that means his collection is empty and he does not have one
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
                if (snapshot.Exists)
                {
                    LoadInfo(snapshot, speciesName);
                }
                else
                {
                    Debug.Log("No such document!");
                }
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

        loadedSpeciesCount++;
        // Check if all species have been loaded
        if (loadedSpeciesCount == likedStrings.Count)
        {
            displayButtons();
        }
    }

    private void displayButtons() {
        // Loop through the inventory items and create a button for each item
        float buttonHeight = ((RectTransform)buttonPrefab.transform).rect.height;
        float y = -buttonHeight / 2f;
        foreach (var species in inventory)
        {
            // Create a new button instance from the prefab
            GameObject button = Instantiate(buttonPrefab, listViewTransform);

            // Set the text of the button to display the model name and link
            TMPro.TextMeshProUGUI modelNameText = button.transform.Find("Model Name").GetComponent<TMPro.TextMeshProUGUI>();
            modelNameText.text = species.speciesName;

            // Set the position of the button based on its height
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(0f, y);
            y -= buttonHeight + 25;

            // Add an onClick listener to the button
            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => {
                Application.OpenURL(species.link);
            });

        }
    }
}
