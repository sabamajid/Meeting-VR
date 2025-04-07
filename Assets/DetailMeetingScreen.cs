using UnityEngine;
using TMPro;

public class DetailMeetingScreen : MonoBehaviour
{
    public static DetailMeetingScreen instance;

    public GameObject screenPanel;
    public TextMeshProUGUI titleText, startTimeText, endTimeText, descriptionText, meetingLinkText;

    private string meetingLink; // store it internally too in case you want to open/copy later

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        screenPanel.SetActive(false);
    }

    public void ShowMeetingDetails(string title, string startTime, string endTime, string description, string meetingLink)
    {
        titleText.text = title;
        startTimeText.text = string.IsNullOrEmpty(startTime) ? "N/A" : startTime;
        endTimeText.text = string.IsNullOrEmpty(endTime) ? "N/A" : endTime;
        descriptionText.text = description;

        this.meetingLink = meetingLink;
        meetingLinkText.text = string.IsNullOrEmpty(meetingLink) ? "No meeting link" : meetingLink;

        screenPanel.SetActive(true);

        GameObject allMeetingsPanel = GameObject.FindWithTag("All Meetings");
        if (allMeetingsPanel != null)
        {
            allMeetingsPanel.SetActive(false);
        }
    }

    public void HideMeetingDetails()
    {
        screenPanel.SetActive(false);
    }

    // OPTIONAL: Call this from a button click to open the link
    public void OpenMeetingLink()
    {
        if (!string.IsNullOrEmpty(meetingLink))
        {
            Application.OpenURL(meetingLink);
        }
        else
        {
            Debug.LogWarning("No meeting link to open.");
        }
    }
}
