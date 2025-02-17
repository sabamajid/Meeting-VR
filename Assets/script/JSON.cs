using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class JSON : MonoBehaviour
{
    public static JSON instance;


   

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
    }

    [System.Serializable]
    public class AgentData
    {
        public Agent[] agents;
    }

    private string apiUrl = "https://agent-meet-backend.chillkro.com";

    //  private string apiUrl = "https://8bc6-182-185-140-19.ngrok-free.app/api";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(GetRequest());
    }

    IEnumerator GetRequest()
    {
        string requestName = "/api/agents/";

        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl + requestName))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("GET Request Successful: " + www.downloadHandler.text);

                // Parse the JSON response
                string jsonString = www.downloadHandler.text;
                AgentData data = JsonUtility.FromJson<AgentData>(jsonString);

                // Pass the agents to the ButtonGenerator to create buttons
                ButtonGenerator.instance.GenerateButtons(data.agents);
            }
            else
            {
                Debug.LogError("GET Request Error: " + www.error);
                if (www.downloadHandler != null)
                {
                    Debug.Log("Response Text: " + www.downloadHandler.text);
                }
            }
        }
    }
}
