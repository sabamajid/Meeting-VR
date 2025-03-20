using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

        // Wait for 0.4 seconds before fetching agents
        StartCoroutine(DelayedAgentFetch(0.4f));
    }

    private IEnumerator DelayedAgentFetch(float delay)
    {
        Debug.Log("Starting DelayedAgentFetch...");
        yield return new WaitForSeconds(delay);
        Debug.Log("Fetching agents after delay...");
        agentCalling.instance.GetAgent(PopulateDropdown);
    }

    public void PopulateDropdown(List<agentCalling.Agent> agents)
    {
        Debug.Log("PopulateDropdown() called with " + (agents != null ? agents.Count : 0) + " agents");

        if (agents == null || agents.Count == 0)
        {
            Debug.LogError("No agents found inside PopulateDropdown!");
            return;
        }

        // Extract agent names
        List<string> options = new List<string>();
        foreach (var agent in agents)
        {
            options.Add(agent.name);
        }

        // Populate the dropdown with fetched agent names
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
                optionButton.onClick.RemoveAllListeners();
                optionButton.onClick.AddListener(() => SelectOption(option));
            }
            else
            {
                Debug.LogError("Option prefab is missing a Button component!");
            }

            spawnedOptions.Add(newOption);
            newOption.SetActive(true);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsContainer.GetComponent<RectTransform>());
    }

    void SelectOption(string optionText)
    {
        selectedText.text = optionText;
        dropdownPanel.SetActive(false);
        isDropdownOpen = false;
    }

    void ToggleDropdown()
    {
        isDropdownOpen = !isDropdownOpen;
        dropdownPanel.SetActive(isDropdownOpen);
    }
}
