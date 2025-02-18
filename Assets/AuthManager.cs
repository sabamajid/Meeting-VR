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

    public void RegisterUser(string firstName, string lastName, string email, string contact, string password, string confirmPassword)
    {
        Debug.Log("Registering User...");
        StartCoroutine(PostRegisterUserRequest(firstName, lastName, email, contact, password, confirmPassword));
    }

    public void LoginUser(string email, string password)
    {
        Debug.Log("Logging in User...");
        StartCoroutine(PostLoginUserRequest(email, password));
    }


    private IEnumerator PostRegisterUserRequest(string firstName, string lastName, string email, string contact, string password, string confirmPassword)
    {
        string requestName = "api/auth/register/";
        string json = $"{{\"first_name\":\"{firstName}\",\"last_name\":\"{lastName}\",\"email\":\"{email}\",\"contact\":\"{contact}\",\"password\":\"{password}\",\"confirm_password\":\"{confirmPassword}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + requestName, "POST"))
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

    private IEnumerator PostLoginUserRequest(string email, string password)
    {
        string requestName = "api/auth/login/";
        string json = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + requestName, "POST"))
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

    
}
