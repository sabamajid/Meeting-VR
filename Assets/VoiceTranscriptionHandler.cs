using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Oculus.Voice;
using System.Collections;

public class VoiceTranscriptionHandler : MonoBehaviour
{
    public static VoiceTranscriptionHandler Instance { get; private set; }

    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] private TextMeshProUGUI transcriptionText;

    public UnityEvent<string> onTranscriptionComplete;
    private bool isRecording = false;
    private string finalTranscription = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartRecording();
        StartCoroutine(ShowMessageAfterDelay("Hello, Unity!", 5f));
    }

    private IEnumerator ShowMessageAfterDelay(string message, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for 'delay' seconds
     
       StopRecording();   // Stop and send message
        Debug.Log("🕒 " + message);
    }
    private void OnEnable()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
            appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);
        }
    }

    private void OnDisable()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
            appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
        }
    }

    private void OnPartialTranscription(string transcription)
    {
        transcriptionText.text = transcription;
    }

    private void OnFullTranscription(string transcription)
    {
        finalTranscription = transcription;
        transcriptionText.text = finalTranscription;
        onTranscriptionComplete?.Invoke(finalTranscription);
    }

    public void StartRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            appVoiceExperience.Activate();
            Debug.Log("🎤 Voice recording started...");
        }
    }

    public void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            appVoiceExperience.Deactivate();
            Debug.Log("🛑 Voice recording stopped.");

            // Send message through WebSocket
            WebSocketClient.Instance.SendMessageToServer(finalTranscription);
        }
    }
}
