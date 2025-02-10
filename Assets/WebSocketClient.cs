using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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
    private ClientWebSocket webSocket;

    private async void Start()
    {
        await ConnectToWebSocket();

        // Wait 5 seconds before sending a message
        await Task.Delay(5000);
        await SendMessageToServer();
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

            // Start listening for messages
            _ = ReceiveMessages();
        }
        catch (WebSocketException wsEx)
        {
            Debug.LogError("❌ WebSocketException: " + wsEx.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ General Exception: " + ex.Message);
        }
    }

    public async Task SendMessageToServer()
    {
        if (webSocket == null || webSocket.State != WebSocketState.Open)
        {
            Debug.LogError("❌ WebSocket is not connected.");
            return;
        }

        string jsonMessage = "{\"message\": \"How to improve sales for our product agency erp\"}";
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

            // Deserialize the JSON into a C# object
            try
            {
                WebSocketResponse response = JsonUtility.FromJson<WebSocketResponse>(jsonString);

                if (response != null)
                {
                    Debug.Log($"🗣️ Agent: {response.message.agent}");
                    Debug.Log($"💬 Message: {response.message.content}");
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
