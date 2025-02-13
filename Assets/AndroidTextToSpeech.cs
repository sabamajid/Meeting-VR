using UnityEngine;

public class AndroidTextToSpeech : MonoBehaviour
{
    private AndroidJavaObject ttsObject;

    void Start()
    {
        InitializeTTS();
    }

    private void InitializeTTS()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                ttsObject = new AndroidJavaObject("android.speech.tts.TextToSpeech", activity, new TTSInitListener());
            }
        }
        else
        {
            Debug.LogWarning("TTS only works on Android devices (Quest 3).");
        }
    }

    public void SpeakText(string text)
    {
        if (ttsObject != null)
        {
            ttsObject.Call<int>("speak", text, 0, null, null);
        }
        else
        {
            Debug.LogError("TTS is not initialized.");
        }
    }

    private class TTSInitListener : AndroidJavaProxy
    {
        public TTSInitListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        public void onInit(int status)
        {
            if (status == 0)
                Debug.Log("TTS Initialized Successfully!");
            else
                Debug.LogError("TTS Initialization Failed!");
        }
    }
}
