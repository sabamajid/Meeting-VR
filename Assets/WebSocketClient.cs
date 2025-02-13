using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

[System.Serializable]
public class WebSocketResponse
{
    public string type;
    public MessageData message;
}

[System.Serializable]
public class MessageData
{
    public string agent;
    public string content;
}

public class WebSocketClient : MonoBehaviour
{
    private static WebSocketClient instance;  // Singleton instance
    private ClientWebSocket webSocket;
    public TextMeshProUGUI responseText;  // UI Text
    private AndroidTextToSpeech tts;      // Reference to TTS system

    public static WebSocketClient Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("WebSocketClient");
                instance = obj.AddComponent<WebSocketClient>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private async void Start()
    {
        instance = this; // Set instance
        tts = FindObjectOfType<AndroidTextToSpeech>(); // Find TTS script
        await ConnectToWebSocket();
    }

    private async Task ConnectToWebSocket()
    {
        webSocket = new ClientWebSocket();
        Uri serverUri = new Uri("wss://agent-meet-backend.chillkro.com/ws/chat/");

        try
        {
            Debug.Log("🔄 Connecting to WebSocket: " + serverUri);
            await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("✅ Connected to WebSocket server.");

            _ = ReceiveMessages(); // Start receiving messages
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ WebSocket Connection Failed: " + ex.Message);
        }
    }

    public async Task SendMessageToServer(string message)
    {
        if (webSocket == null || webSocket.State != WebSocketState.Open)
        {
            Debug.LogError("❌ WebSocket is not connected.");
            return;
        }

        string jsonMessage = "{\"message\": \"" + message + "\"}";
        byte[] bytes = Encoding.UTF8.GetBytes(jsonMessage);

        await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        Debug.Log("📤 Message Sent: " + jsonMessage);
    }

    private async Task ReceiveMessages()
    {
        byte[] buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string jsonString = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Debug.Log("📩 Received: " + jsonString);

            try
            {
                WebSocketResponse response = JsonUtility.FromJson<WebSocketResponse>(jsonString);
                if (response != null)
                {
                    string receivedMessage = response.message.content;

                    // Call UI update and TTS from the main thread using Coroutine
                    StartCoroutine(UpdateUIAndSpeak(receivedMessage));
                }
                else
                {
                    Debug.LogError("⚠️ Failed to parse WebSocket response.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("⚠️ JSON Parsing Error: " + ex.Message);
            }
        }
    }

    private System.Collections.IEnumerator UpdateUIAndSpeak(string text)
    {
        responseText.text = text; // Update UI text
        yield return new WaitForSeconds(0.1f); // Small delay to ensure UI updates

        if (tts != null)
        {
            tts.SpeakText(text);
        }
    }

    private async void OnApplicationQuit()
    {
        if (webSocket != null)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            webSocket.Dispose();
            Debug.Log("🛑 WebSocket Closed.");
        }
    }
}
