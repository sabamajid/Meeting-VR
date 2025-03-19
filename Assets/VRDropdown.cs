using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRDropdown : MonoBehaviour
{
    public Button dropdownButton; // Button to open dropdown
    public TextMeshProUGUI selectedText; // Displays selected option
    public GameObject dropdownPanel; // The Scroll View panel
    public Transform optionsContainer; // Content inside Scroll View
    public GameObject optionPrefab; // Prefab for each option button

    private bool isDropdownOpen = false;
    private List<GameObject> spawnedOptions = new List<GameObject>();

    void Start()
    {
        dropdownButton.onClick.AddListener(ToggleDropdown);
        dropdownPanel.SetActive(false);

        // Initialize the dropdown with options
        List<string> options = new List<string> { "Option 1", "Option 2", "Option 3" };
        InitializeDropdown(options);
    }

    public void InitializeDropdown(List<string> options)
    {
        // Clear previous options
        foreach (var option in spawnedOptions)
        {
            Destroy(option);
        }
        spawnedOptions.Clear();

        // Check if prefab is assigned
        if (optionPrefab == null)
        {
            Debug.LogError("Option Prefab is missing! Assign it in the Inspector.");
            return;
        }

        // Generate options
        foreach (string option in options)
        {
            GameObject newOption = Instantiate(optionPrefab, optionsContainer);
            TextMeshProUGUI optionText = newOption.GetComponentInChildren<TextMeshProUGUI>();
            Button optionButton = newOption.GetComponent<Button>();

            if (optionText != null)
                optionText.text = option;

            if (optionButton != null)
            {
                optionButton.onClick.RemoveAllListeners(); // Prevent duplicate event calls
                optionButton.onClick.AddListener(() => SelectOption(option));
            }
            else
            {
                Debug.LogError("Option prefab is missing a Button component!");
            }

            spawnedOptions.Add(newOption);
            newOption.SetActive(true);
        }

        // Refresh UI
        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsContainer.GetComponent<RectTransform>());
    }

    // Function to update selected text and close dropdown
    void SelectOption(string optionText)
    {
        selectedText.text = optionText;
        dropdownPanel.SetActive(false); // Close dropdown immediately
        isDropdownOpen = false; // Ensure dropdown state is updated
    }

    void ToggleDropdown()
    {
        isDropdownOpen = !isDropdownOpen;
        dropdownPanel.SetActive(isDropdownOpen);
    }
}
