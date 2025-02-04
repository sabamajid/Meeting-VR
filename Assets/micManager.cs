using UnityEngine;
using TMPro;

public class MicManager : MonoBehaviour
{
    private string microphoneDevice;
    private AudioSource audioSource;

    // Reference to TextMeshProUGUI for Debug Info
    public TextMeshProUGUI debugText;

    private void Start()
    {
        // Get default microphone device
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
            SetDebugText("Microphone detected: " + microphoneDevice);
        }
        else
        {
            SetDebugText("No microphone detected!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            SetDebugText("Error: No AudioSource found!");
        }
    }

    public void MicOn()
    {
        if (!string.IsNullOrEmpty(microphoneDevice))
        {
            if (Microphone.IsRecording(microphoneDevice))
            {
                SetDebugText("Microphone already recording.");
                return;
            }

            if (audioSource != null)
            {
                SetDebugText("Starting microphone input...");

                // Clear previous audio data
                audioSource.Stop();
                audioSource.clip = null;

                // Start recording
                audioSource.clip = Microphone.Start(microphoneDevice, true, 10, 44100);
                audioSource.loop = true;

                // Wait for mic to start properly
                while (Microphone.GetPosition(microphoneDevice) <= 0) { }

                audioSource.Play();
                SetDebugText("Microphone started and audio playing.");
            }
            else
            {
                SetDebugText("Error: No AudioSource found!");
            }
        }
        else
        {
            SetDebugText("No microphone device available to start.");
        }
    }

    public void MicOff()
    {
        if (!string.IsNullOrEmpty(microphoneDevice))
        {
            SetDebugText("Stopping microphone input...");

            // Stop microphone input
            Microphone.End(microphoneDevice);

            // Stop and reset AudioSource
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null; // Fully clear audio
            }

            SetDebugText("Microphone stopped.");
        }
        else
        {
            SetDebugText("No microphone device to stop.");
        }
    }

    private void SetDebugText(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
        else
        {
            Debug.LogError("Debug Text UI not assigned!");
        }
    }
}
