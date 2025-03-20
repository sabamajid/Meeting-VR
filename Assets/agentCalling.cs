using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; // Added for better JSON parsing

public class agentCalling : MonoBehaviour
{
    public static agentCalling instance;
    private static string BASE_URL = "https://agent-meet-backend.chillkro.com";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class Agent
    {
        public int id;
        public string name;
        public string description;
        public string role;
        public string goal;
        public string backstory;
        public bool is_async;
        public string prompt;
        public GptModel gpt_model;
        public string color;
        public List<Tool> tool;
        public int? user_id;
    }

    [System.Serializable]
    public class GptModel
    {
        public string model_name;
        public string description;
    }

    [System.Serializable]
    public class Tool
    {
        public int id;
        public string tool_name;
        public string description;
    }

    [System.Serializable]
    public class AgentsData
    {
        public List<Agent> agents;
    }

    public void GetAgent(System.Action<List<Agent>> callback)
    {
        Debug.Log("GetAgent() Called. Fetching agents...");

        string accessToken;

#if UNITY_EDITOR 
        accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0b2tlbl90eXBlIjoiYWNjZXNzIiwiZXhwIjoxNzQyODgxNjI5LCJpYXQiOjE3NDI0NDk2MjksImp0aSI6IjQ4NGIwODJjYTc3YzRkODk4YjhmZWRjNGU3M2M4MGRiIiwidXNlcl9pZCI6NH0.YpJA964AWiO0yc9OVn7gvYR74219Zke0fOzT8Hd9UpA";
#else
        accessToken = PlayerPrefs.GetString("access_token", "");
#endif

        if (string.IsNullOrEmpty(accessToken))
        {
            Debug.LogError("Authorization token not found!");
            return;
        }

        StartCoroutine(GetAllAgentsRequest(accessToken, callback));
    }

    private IEnumerator GetAllAgentsRequest(string token, System.Action<List<Agent>> callback)
    {
        string url = BASE_URL + "/api/agents/";
        Debug.Log("Sending API request to: " + url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("API Response Raw JSON: " + jsonResponse);

                try
                {
                    AgentsData agentsData = JsonConvert.DeserializeObject<AgentsData>(jsonResponse);

                    if (agentsData == null || agentsData.agents == null || agentsData.agents.Count == 0)
                    {
                        Debug.LogError("No agents found! API response may be incorrect.");
                        callback?.Invoke(new List<Agent>());
                        yield break;
                    }

                    Debug.Log("Agents fetched successfully! Count: " + agentsData.agents.Count);
                    callback?.Invoke(agentsData.agents);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                    Debug.LogError("Invalid JSON: " + jsonResponse);
                }
            }
            else
            {
                Debug.LogError("API Request Failed: " + www.error);
            }
        }
    }
}
