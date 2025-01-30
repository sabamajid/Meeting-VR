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
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>(); // Store original button colors

    private bool serverWorking = false; // Set to true when the server is working
    private Color selectedColor = Color.green; // Change this to any color you like

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // If the server is not working, generate test buttons
        if (!serverWorking)
        {
            GenerateTestButtons();
        }
    }

    public void GenerateButtons(Agent[] agents)
    {
        if (!serverWorking)
        {
            return; // Skip if the server is not working
        }

        foreach (var agent in agents)
        {
            CreateButton(agent.name);
        }
    }

    void GenerateTestButtons()
    {
        for (int i = 1; i <= 5; i++)
        {
            CreateButton("Testing " + i);
        }
    }

    void CreateButton(string buttonName)
    {
        // Instantiate a button from the prefab
        GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);

        // Set the button text
        Text buttonText = buttonObj.GetComponentInChildren<Text>();
        buttonText.text = buttonName;

        // Get Button component
        Button btn = buttonObj.GetComponent<Button>();

        // Store the original button color
        Color originalColor = btn.image.color;
        originalColors[btn] = originalColor;

        // Add a listener to handle button clicks
        btn.onClick.AddListener(() => OnButtonClicked(buttonName, btn));
    }

    void OnButtonClicked(string buttonName, Button btn)
    {
        Debug.Log("Button clicked: " + buttonName);

        if (!selectedNames.Contains(buttonName))
        {
            // Select the button
            selectedNames.Add(buttonName);
            btn.image.color = selectedColor;
        }
        else
        {
            // Deselect the button and revert color
            selectedNames.Remove(buttonName);
            btn.image.color = originalColors[btn];
        }
    }

    // Public method to access selected names
    public List<string> GetSelectedNames()
    {
        return selectedNames;
    }
}
