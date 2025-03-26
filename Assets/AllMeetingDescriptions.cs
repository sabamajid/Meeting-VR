using UnityEngine;
using TMPro;

public class AllMeetingDescriptions : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI startTimeText;
    public TextMeshProUGUI endTimeText;
    public TextMeshProUGUI descriptionText;

    public GameObject meetingDetailsPanel; // Reference to the details screen

    public TextMeshProUGUI detailsTitle;
    public TextMeshProUGUI detailsStartTime;
    public TextMeshProUGUI detailsEndTime;
    public TextMeshProUGUI detailsDescription;

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (meetingDetailsPanel != null)
        {
            meetingDetailsPanel.SetActive(false);
        }
    }

    public void SetMeetingData(string title, string startTime, string endTime, string description)
    {
        titleText.text = title;
        startTimeText.text = string.IsNullOrEmpty(startTime) ? "N/A" : startTime;
        endTimeText.text = string.IsNullOrEmpty(endTime) ? "N/A" : endTime;
        descriptionText.text = description;
    }

    // Call this when a meeting is clicked
    public void ShowMeetingDetails()
    {
        if (meetingDetailsPanel != null)
        {
            meetingDetailsPanel.SetActive(true);
            detailsTitle.text = titleText.text;
            detailsStartTime.text = startTimeText.text;
            detailsEndTime.text = endTimeText.text;
            detailsDescription.text = descriptionText.text;
        }
    }

    public void CloseMeetingDetails()
    {
        if (meetingDetailsPanel != null)
        {
            meetingDetailsPanel.SetActive(false);
        }
    }
}
