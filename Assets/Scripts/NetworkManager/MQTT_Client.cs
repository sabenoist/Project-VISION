using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using System;

/// <summary>
/// Subclass of the library <see cref="M2MqttUnityClient"/> to handle our connection to the MQTT broker.
/// </summary>
public class MQTT_Client : M2MqttUnityClient
{
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
    private event Action<Microphone, float> ReceivedData;

    /// <summary>
    /// Static reference to this object for other classes to call upon.
    /// </summary>
    public static MQTT_Client NetworkManager { get; private set; }

    /// <summary>
    /// Adds a listener for when <see cref="ReceivedData"/> gets invoked.
    /// </summary>
    /// <param name="listener">The listener to be added.</param>
    public void AddReceivedDataListener(Action<Microphone, float> listener) {
        ReceivedData += listener;
    }

    /// <summary>
    /// Removes a listener for when <see cref="ReceivedData"/> gets invoked.
    /// </summary>
    /// <param name="listener">The listener to be added.</param>
    public void RemoveReceivedDataListener(Action<Microphone, float> listener) {
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
        base.OnConnected();
    }

    /// <summary>
    /// Subscribes the client to the broker for each microphone.
    /// </summary>
    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "0" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "1" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "2" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "3" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    /// <summary>
    /// Unsubscribes the client to the broker for each microphone.
    /// </summary>
    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "0" });
        client.Unsubscribe(new string[] { "1" });
        client.Unsubscribe(new string[] { "2" });
        client.Unsubscribe(new string[] { "3" });
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
        string msg = System.Text.Encoding.UTF8.GetString(message);

        if (verboseLogging) 
        {
            Debug.Log("Topic: " + topic + " ~ Received: " + msg);
        }

        ReceivedData?.Invoke((Microphone)int.Parse(topic), float.Parse(msg));
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

