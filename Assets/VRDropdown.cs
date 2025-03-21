using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VRDropdown : MonoBehaviour
{
    public enum DropdownType { User, Agent }
    public DropdownType dropdownType;  // Set this in the Inspector

    public Button dropdownButton;
    public TextMeshProUGUI selectedText;
    public GameObject dropdownPanel;
    public Transform optionsContainer;
    public GameObject optionPrefab;

    private bool isDropdownOpen = false;
    private List<GameObject> spawnedOptions = new List<GameObject>();

    void Start()
    {
        dropdownButton.onClick.AddListener(ToggleDropdown);
        dropdownPanel.SetActive(false);

        // Identify the dropdown type and fetch data accordingly
        if (dropdownType == DropdownType.Agent)
        {
            StartCoroutine(DelayedAgentFetch(0.4f));
        }
        else if (dropdownType == DropdownType.User)
        {
            StartCoroutine(DelayedUserFetch(0.4f));
        }
    }

    private IEnumerator DelayedAgentFetch(float delay)
    {
        yield return new WaitForSeconds(delay);
        agentCalling.instance.GetAgent(PopulateDropdownForAgents);
    }

    private IEnumerator DelayedUserFetch(float delay)
    {
        yield return new WaitForSeconds(delay);
        UserCalling.instance.GetUsers(PopulateDropdownForUsers);
    }

    // Populate dropdown for Agents
    public void PopulateDropdownForAgents(List<agentCalling.Agent> agents)
    {
        if (agents == null || agents.Count == 0)
        {
            Debug.LogError("No agents found!");
            return;
        }

        List<string> options = new List<string>();
        foreach (var agent in agents)
        {
            options.Add(agent.name);
        }
        InitializeDropdown(options);
    }

    // Populate dropdown for Users
    public void PopulateDropdownForUsers(List<UserCalling.User> users)
    {
        if (users == null || users.Count == 0)
        {
            Debug.LogError("No users found!");
            return;
        }

        List<string> options = new List<string>();
        foreach (var user in users)
        {
            options.Add(user.first_name + " " + user.last_name); // Full name for users
        }
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
            Debug.LogError("Option Prefab is missing!");
            return;
        }

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
