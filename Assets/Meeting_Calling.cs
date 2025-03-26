using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Meeting_Calling : MonoBehaviour
{
    public static Meeting_Calling instance;
    private static readonly string BASE_URL = "https://agent-meet-backend.chillkro.com";
    public TextMeshProUGUI debugText; // Reference to TextMeshPro UI for debugging

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #region Data Structures

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

    [System.Serializable]
    public class MeetingListResponse
    {
        public List<MeetingData> meetings;
    }

    #endregion

    #region Meeting API Methods

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

        using (UnityWebRequest www = new UnityWebRequest($"{BASE_URL}/api/meeting/create_meeting/", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            string accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                DebugLog("Authorization token not found!");
                yield break;
            }

            www.SetRequestHeader("Authorization", "Bearer " + accessToken);
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

    public void GetAllMeetings(System.Action<List<MeetingData>> callback)
    {
        StartCoroutine(GetAllMeetingsRequest(callback));
    }

    private IEnumerator GetAllMeetingsRequest(System.Action<List<MeetingData>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{BASE_URL}/api/meeting/get_all_meetings/"))
        {
            string accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                DebugLog("Authorization token not found!");
                yield break;
            }

            www.SetRequestHeader("Authorization", "Bearer " + accessToken);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                MeetingListResponse response = JsonUtility.FromJson<MeetingListResponse>(www.downloadHandler.text);
                callback?.Invoke(response.meetings);
            }
            else
            {
                DebugLog("Failed to fetch meetings: " + www.error);
            }
        }
    }

    #endregion

    #region Utility Methods

    private string GetAccessToken()
    {
#if UNITY_EDITOR
        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0b2tlbl90eXBlIjoiYWNjZXNzIiwiZXhwIjoxNzQzNDAzNjYxLCJpYXQiOjE3NDI5NzE2NjEsImp0aSI6Ijc0MzE5OGE5ZTkzYTQ3MzVhMDU2OWRiNjMzODNlODA3IiwidXNlcl9pZCI6NH0.a8dydYxMEv6v4ptWcpAEo_tPg_E3bnMGIBgkXGYZIe8"; // Use dummy token in the editor
#else
    return PlayerPrefs.GetString("access_token", "");
#endif
    }


    private void DebugLog(string message)
    {
        Debug.Log(message);
        if (debugText != null)
        {
            debugText.text += message + "\n"; // Append message to TextMeshPro UI
        }
    }

    #endregion
}
