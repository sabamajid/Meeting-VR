using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;
    public static string BASE_URL = "https://agent-meet-backend.chillkro.com/";

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

    // Data class for Registering User
    [System.Serializable]
    public class RegisterUserData
    {
        public string email;
        public string password;
        public string first_name;
        public string last_name;
        public string role = "USER"; // Default value for role
        public string contact;
    }

    // Register user method
    public void RegisterUser(string firstName, string lastName, string email, string contact, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            Debug.Log("Passwords do not match.");
            return;
        }
        Debug.Log("Registering User...");
        RegisterUserData userData = new RegisterUserData
        {
            email = email,
            password = password,
            first_name = firstName,
            last_name = lastName,
            contact = contact
        };

        StartCoroutine(PostRegisterUserRequest(userData));
    }

    // Login user method
    public void LoginUser(string email, string password)
    {
        Debug.Log("Logging in User...");
        StartCoroutine(PostLoginUserRequest(email, password));
    }

    // Post request to register user
    private IEnumerator PostRegisterUserRequest(RegisterUserData userData)
    {
        string json = JsonUtility.ToJson(userData); // Convert user data to JSON
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "api/auth/register/", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User Registered Successfully: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Registration Failed: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
        }
    }

    // Post request to login user
    private IEnumerator PostLoginUserRequest(string email, string password)
    {
        string json = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "api/auth/login/", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Successful: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Login Failed: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
        }
    }

    // Handle Guest User Login (if applicable)
    public void GuestUserLogin()
    {
        PlayerPrefs.SetInt("guest", 1);
        // If you need to handle guest login via API, add the necessary code here.
    }
}
