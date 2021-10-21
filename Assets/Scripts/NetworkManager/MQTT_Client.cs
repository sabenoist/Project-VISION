using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Subclass of the library <see cref="M2MqttUnityClient"/> to handle our connection to the MQTT broker.
/// </summary>
public class MQTT_Client : M2MqttUnityClient
{
    /// <summary>
    /// Reference to the text field to display the connection status.
    /// </summary>
    [SerializeField]
    private Text connectionStatus;

    /// <summary>
    /// Array of topics to subscribe to.
    /// </summary>
    [Header("Topics to subscribe to")]
    [SerializeField]
    private string[] topics;

    /// <summary>
    /// Array of topics to subscribe to.
    /// </summary>
    [Header("Debug options")]
    [SerializeField]
    private bool verboseLogging = false;

    /// <summary>
    /// Event to trigger when data has been received.
    /// </summary>
    private event Action<Dictionary<string, string>> ReceivedData;

    /// <summary>
    /// Static reference to this object for other classes to call upon.
    /// </summary>
    public static MQTT_Client NetworkManager { get; private set; }

    /// <summary>
    /// Adds a listener for when <see cref="ReceivedData"/> gets invoked.
    /// </summary>
    /// <param name="listener">The listener to be added.</param>
    public void AddReceivedDataListener(Action<Dictionary<string, string>> listener) {
        ReceivedData += listener;
    }

    /// <summary>
    /// Removes a listener for when <see cref="ReceivedData"/> gets invoked.
    /// </summary>
    /// <param name="listener">The listener to be added.</param>
    public void RemoveReceivedDataListener(Action<Dictionary<string, string>> listener) {
        ReceivedData -= listener;
    }

    /// <summary>
    /// To execute while connecting.
    /// </summary>
    protected override void OnConnecting()
    {
        base.OnConnecting();
    }

    /// <summary>
    /// To execute when a connection has been established.
    /// </summary>
    protected override void OnConnected()
    {
        connectionStatus.text = "Connected";
        connectionStatus.color = Color.green;
        base.OnConnected();
    }

    /// <summary>
    /// To execute when a connection has been lost.
    /// </summary>
    protected override void OnDisconnected() {
        connectionStatus.text = "Disconnected";
        connectionStatus.color = Color.red;
        base.OnDisconnected();
    }

    /// <summary>
    /// Subscribes the client to the broker for each microphone.
    /// </summary>
    protected override void SubscribeTopics()
    {
        foreach (string topic in topics) {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Debug.Log("Subscribed to topic: " + topic + "\n");
        }
    }

    /// <summary>
    /// Unsubscribes the client to the broker for each microphone.
    /// </summary>
    protected override void UnsubscribeTopics()
    {
        foreach (string topic in topics) {
            client.Unsubscribe(new string[] { topic });
        }
    }

    /// <summary>
    /// Sets up the MQTT client and connects it to the broker upon instantiation of the object.
    /// </summary>
    protected override void Start()
    {
        NetworkManager = this;
        base.Start();
    }

    /// <summary>
    /// Deserializes the received message handles it further.
    /// </summary>
    /// <param name="topic">The topic attached to the MQTT packet.</param>
    /// <param name="message">The message attached to the MQTT packet.</param>
    protected override void DecodeMessage(string topic, byte[] message)
    {
        Dictionary<string, string> decodedMsg = new Dictionary<string, string>();

        string msg = System.Text.Encoding.UTF8.GetString(message);
        msg = msg.Substring(1, msg.Length - 2);

        string[] msgSplit = msg.Split(',');
        foreach (string msgData in msgSplit)
        {
            string[] dataSplit = msgData.Split(':');
            decodedMsg.Add(dataSplit[0].Trim('"'), dataSplit[1].Trim('"'));
        }

        if (verboseLogging) 
        {
            Debug.Log("Topic: " + topic + " ~ Received: " + msg + "\n");
        }

        ReceivedData?.Invoke(decodedMsg);
    }

    /// <summary>
    /// Processes any MQTT related events.
    /// </summary>
    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()
    }

    /// <summary>
    /// Informs the broker we're disconnecting before destroying the object.
    /// </summary>
    private void OnDestroy()
    {
        Disconnect();
    }
}

