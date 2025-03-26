using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Import TextMeshPro

public class Meeting_Calling : MonoBehaviour
{
    public static Meeting_Calling instance;
    private static string BASE_URL = "https://agent-meet-backend.chillkro.com";
    public TextMeshProUGUI debugText; // Reference to TextMeshPro UI

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    [System.Serializable]
    public class MeetingRequest
    {
        public string title;
        public string description;
        public string start_time;
        public string end_time;
        public string summary;
        public int[] agent_ids;
        public int[] user_ids;
    }

    [System.Serializable]
    public class MeetingResponse
    {
        public string message;
        public MeetingData data;
    }

    [System.Serializable]
    public class MeetingData
    {
        public int id;
        public string title;
        public string description;
        public string start_time;
        public string end_time;
        public string created_at;
        public string updated_at;
        public bool is_ended;
        public int user_id;
        public string meeting_id;
        public string meeting_status;
        public string meeting_link;
        public string summary;
    }

    public void CreateMeeting(string title, string description, string startTime, string endTime, string summary, int[] agentIds, int[] userIds)
    {
        DebugLog("Creating Meeting...");
        MeetingRequest request = new MeetingRequest
        {
            title = title,
            description = description,
            start_time = startTime,
            end_time = endTime,
            summary = summary,
            agent_ids = agentIds,
            user_ids = userIds
        };

        StartCoroutine(PostCreateMeetingRequest(request));
    }

    private IEnumerator PostCreateMeetingRequest(MeetingRequest request)
    {
        string json = JsonUtility.ToJson(request);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "/api/meeting/create_meeting/", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            string accessToken = PlayerPrefs.GetString("access_token", "");
            if (!string.IsNullOrEmpty(accessToken))
            {
                www.SetRequestHeader("Authorization", "Bearer " + accessToken);
            }
            else
            {
                DebugLog("Authorization token not found!");
                yield break;
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                DebugLog("Meeting Created Successfully: " + www.downloadHandler.text);
                MeetingResponse response = JsonUtility.FromJson<MeetingResponse>(www.downloadHandler.text);
                DebugLog("Meeting Link: " + response.data.meeting_link);
            }
            else
            {
                DebugLog("Meeting Creation Failed: " + www.error);
                DebugLog("Response: " + www.downloadHandler.text);
            }
        }
    }

    private void DebugLog(string message)
    {
        Debug.Log(message);
        if (debugText != null)
        {
            debugText.text += message + "\n"; // Append message to TextMeshPro
        }
    }
}
