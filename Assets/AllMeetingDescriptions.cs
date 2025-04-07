using UnityEngine;
using TMPro;
using UnityEngine.UI; // Needed for button interaction

public class AllMeetingDescriptions : MonoBehaviour
{
    public TextMeshProUGUI titleText, startTimeText, endTimeText, descriptionText;
    public Button openDetailButton; // Assign this in the Inspector;

    private string title, startTime, endTime, description;

    public void SetMeetingData(string title, string startTime, string endTime, string description)
    {
        this.title = title;
        this.startTime = startTime;
        this.endTime = endTime;
        this.description = description;

        titleText.text = title;
        startTimeText.text = string.IsNullOrEmpty(startTime) ? "N/A" : startTime;
        endTimeText.text = string.IsNullOrEmpty(endTime) ? "N/A" : endTime;
        descriptionText.text = description;

        openDetailButton.onClick.AddListener(OpenDetailScreen);

       

    }

    private void OpenDetailScreen()
    {
        DetailMeetingScreen.instance.ShowMeetingDetails(title, startTime, endTime, description);
       
    }
}
