using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Oculus.Voice;
using Meta.WitAi.CallbackHandlers;

public class VoiceTranscriptionHandler : MonoBehaviour
{
    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] private TextMeshProUGUI transcriptionText;

    public UnityEvent<string> completeTranscription;

    private void OnEnable()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
            appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);
            appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(RestartListening);
        }
    }

    private void OnDisable()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
            appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
            appVoiceExperience.VoiceEvents.OnRequestCompleted.RemoveListener(RestartListening);
        }
    }

    private void Awake()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.Activate();
        }
        else
        {
            Debug.LogError("AppVoiceExperience is not set!");
        }
    }

    private void OnPartialTranscription(string transcription)
    {
        transcriptionText.text = transcription; // Show real-time text
    }

    private void OnFullTranscription(string transcription)
    {
        transcriptionText.text += "\n" + transcription; // Append new text
        completeTranscription?.Invoke(transcription);
    }

    private void RestartListening()
    {
        Debug.Log("Restarting voice recognition...");
        appVoiceExperience.Activate(); // Keep it running continuously
    }
}
