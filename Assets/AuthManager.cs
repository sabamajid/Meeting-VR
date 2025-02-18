using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public Tokens tokens;
        public User user;

        [System.Serializable]
        public class Tokens
        {
            public string access;
            public string refresh;
        }

        [System.Serializable]
        public class User
        {
            public int id;
            public string email;
            public string first_name;
            public string last_name;
            public string role;
        }
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
                SceneManager.LoadScene("Registration");
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
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "api/auth/login/", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Successful: " + www.downloadHandler.text);
                // Parse the login response
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

                // Save the tokens
                PlayerPrefs.SetString("access_token", response.tokens.access);
                PlayerPrefs.SetString("refresh_token", response.tokens.refresh);

                // Optionally save user info (if needed)
                PlayerPrefs.SetInt("user_id", response.user.id);
                PlayerPrefs.SetString("user_email", response.user.email);
                PlayerPrefs.SetString("user_first_name", response.user.first_name);
                PlayerPrefs.SetString("user_last_name", response.user.last_name);
                PlayerPrefs.SetString("user_role", response.user.role);

                // Load next scene or home screen (if applicable)
                // SceneManager.LoadScene("HomeScene");

                Debug.Log("Tokens saved successfully.");
                SceneManager.LoadScene("Main Menu");
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
