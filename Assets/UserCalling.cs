using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class UserCalling : MonoBehaviour
{
    public static UserCalling instance;
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
    public class User
    {
        public int id;
        public string first_name;
        public string last_name;
        public string email;
        public string contact;
    }

    [System.Serializable]
    public class UsersData
    {
        public string message;
        public List<User> users;
    }

    public void GetUsers(System.Action<List<User>> callback)
    {
        Debug.Log("GetUsers() Called. Fetching users...");

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

        StartCoroutine(GetAllUsersRequest(accessToken, callback));
    }

    private IEnumerator GetAllUsersRequest(string token, System.Action<List<User>> callback)
    {
        string url = BASE_URL + "/api/auth/getAllUsers/"; // Changed to 'users' endpoint
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
                    UsersData usersData = JsonConvert.DeserializeObject<UsersData>(jsonResponse);

                    if (usersData == null || usersData.users == null || usersData.users.Count == 0)
                    {
                        Debug.LogError("No users found! API response may be incorrect.");
                        callback?.Invoke(new List<User>());
                        yield break;
                    }

                    Debug.Log("Users fetched successfully! Count: " + usersData.users.Count);
                    callback?.Invoke(usersData.users);
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
