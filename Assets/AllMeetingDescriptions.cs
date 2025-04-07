using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AllMeetingDescriptions : MonoBehaviour
{
    public TextMeshProUGUI titleText, startTimeText, endTimeText, descriptionText;
    public Button openDetailButton;

    private string title, startTime, endTime, description;
    private string meetingLink;

    public void SetMeetingData(string title, string startTime, string endTime, string description, string meetingLink)
    {
        this.title = title;
        this.startTime = startTime;
        this.endTime = endTime;
        this.description = description;
        this.meetingLink = meetingLink;

        titleText.text = title;
        startTimeText.text = string.IsNullOrEmpty(startTime) ? "N/A" : startTime;
        endTimeText.text = string.IsNullOrEmpty(endTime) ? "N/A" : endTime;
        descriptionText.text = description;

        openDetailButton.onClick.RemoveAllListeners();
        openDetailButton.onClick.AddListener(() =>
        {
            DetailMeetingScreen.instance.ShowMeetingDetails(title, startTime, endTime, description, meetingLink);
        });
    }
}
