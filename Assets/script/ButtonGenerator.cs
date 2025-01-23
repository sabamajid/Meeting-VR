using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static JSON;

public class ButtonGenerator : MonoBehaviour
{
    public static ButtonGenerator instance;

    public GameObject buttonPrefab;
    public Transform buttonParent;
    private List<string> selectedNames = new List<string>(); // List to store selected names

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GenerateButtons(Agent[] agents)
    {
        foreach (var agent in agents)
        {
            CreateButton(agent);
        }
    }

    void CreateButton(Agent agent)
    {
        // Instantiate a button from the prefab
        GameObject button = Instantiate(buttonPrefab, buttonParent);

        // Set the button text to the agent's name
        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.text = agent.name;

        // Add a listener to handle button clicks
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(() => OnButtonClicked(agent));
    }

    void OnButtonClicked(Agent agent)
    {
        Debug.Log("Button clicked for agent: " + agent.name);

        // Add to the selected names list
        if (!selectedNames.Contains(agent.name))
        {
            selectedNames.Add(agent.name);
        }
        else
        {
            Debug.Log($"{agent.name} is already selected.");
        }
    }

    // Public method to access selected names
    public List<string> GetSelectedNames()
    {
        return selectedNames;
    }
}
