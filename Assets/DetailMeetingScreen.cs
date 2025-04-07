using UnityEngine;
using TMPro;

public class DetailMeetingScreen : MonoBehaviour
{
    public static DetailMeetingScreen instance;

    public GameObject screenPanel; // Assign the Detail Meeting Screen in the Inspector
    public TextMeshProUGUI titleText, startTimeText, endTimeText, descriptionText;

    private void Awake()
    {
        instance = this;  // Singleton instance
        screenPanel.SetActive(false); // Hide screen at start
    }

    public void ShowMeetingDetails(string title, string startTime, string endTime, string description)
    {
        titleText.text = title;
        startTimeText.text = string.IsNullOrEmpty(startTime) ? "N/A" : startTime;
        endTimeText.text = string.IsNullOrEmpty(endTime) ? "N/A" : endTime;
        descriptionText.text = description;

        screenPanel.SetActive(true); // Show screen
        GameObject allMeetingsPanel = GameObject.FindWithTag("All Meetings");

        if (allMeetingsPanel != null)
        {
            allMeetingsPanel.SetActive(false);
        }
        else
        {
            Debug.Log("No GameObject found with the tag 'All Meetings'");
        }
    }

    public void HideMeetingDetails()
    {
        screenPanel.SetActive(false); // Hide screen when close button is clicked
    }
}
