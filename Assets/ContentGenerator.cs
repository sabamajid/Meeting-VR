using System.Collections.Generic;
using UnityEngine;

public class ContentGenerator : MonoBehaviour
{
    public GameObject meetingPrefab; // Assign the meeting prefab in the inspector
    public Transform contentParent; // Parent object to hold the instantiated prefabs

    void Start()
    {
        // Fetch meetings and generate UI elements
        Meeting_Calling.instance.GetAllMeetings(OnMeetingsReceived);
    }

   
    // Callback function to handle the response
    void OnMeetingsReceived(List<Meeting_Calling.MeetingData> meetings)
    {
        // Clear existing content before adding new meetings
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var meeting in meetings)
        {
            // Instantiate the prefab
            GameObject meetingGO = Instantiate(meetingPrefab, contentParent);

            // Assign meeting data to the prefab's script
            AllMeetingDescriptions meetingDesc = meetingGO.GetComponent<AllMeetingDescriptions>();
            if (meetingDesc != null)
            {
                meetingDesc.SetMeetingData(meeting.title, meeting.start_time, meeting.end_time, meeting.description);
            }
        }
    }
}
