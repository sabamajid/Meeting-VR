using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownScrollPopulator : MonoBehaviour
{
    public TMP_Text textMesh;  // Assign your TextMeshProUGUI field in the Inspector
    public GameObject buttonPrefab;  // Assign your button prefab
    public Transform contentPanel;  // Assign the Content panel in the Scroll View

    private string lastText = "";

    void Update()
    {
        if (textMesh.text != lastText) // Detect text change
        {
            lastText = textMesh.text;
            PopulateScrollView(lastText);
        }
    }

    void PopulateScrollView(string newText)
    {
        GameObject newButton = Instantiate(buttonPrefab, contentPanel);
        newButton.GetComponentInChildren<TMP_Text>().text = newText;
    }
}
