using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MeetingInputUIManager : MonoBehaviour
{
    [Header("SignUp Input Area")]
    public InputField titleInputField;
    public InputField descriptionInputField;
    public InputField summaryInputField;

    public InputField StartDate;
    public InputField EndDate;  


    public void CreateMeeting()
    {
        if (string.IsNullOrEmpty(titleInputField.text))
        {
            Debug.Log("Title field cannot be empty.");
            return;
        }
        if (string.IsNullOrEmpty(descriptionInputField.text))
        {
            Debug.Log("Description field cannot be empty.");
            return;
        }
        if (string.IsNullOrEmpty(summaryInputField.text))
        {
            Debug.Log("Summary field cannot be empty.");
            return;
        }

        Debug.Log("Meeting Created: " + titleInputField.text);

    }
}