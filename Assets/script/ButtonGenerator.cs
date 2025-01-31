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

    private Color selectedColor = Color.green;
    private Color defaultColor = Color.white;

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
        btn.onClick.AddListener(() => OnButtonClicked(agent, btn));
    }

    void OnButtonClicked(Agent agent, Button btn)
    {
        Debug.Log("Button clicked for agent: " + agent.name);

        // Add or remove from selected names list
        if (!selectedNames.Contains(agent.name))
        {
            selectedNames.Add(agent.name);
            btn.GetComponent<Image>().color = selectedColor; // Change to green
        }
        else
        {
            selectedNames.Remove(agent.name);
            btn.GetComponent<Image>().color = defaultColor; // Change back to default
            Debug.Log($"{agent.name} is deselected.");
        }
    }

    // Public method to access selected names
    public List<string> GetSelectedNames()
    {
        return selectedNames;
    }
}
