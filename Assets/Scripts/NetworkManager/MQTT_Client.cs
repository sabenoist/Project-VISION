using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

/// <summary>
/// Subclass of the library <see cref="M2MqttUnityClient"/> to handle our connection to the MQTT broker.
/// </summary>
public class MQTT_Client : M2MqttUnityClient
{
    /// <summary>
    /// Static reference to this object for other classes to call upon.
    /// </summary>
    public static MQTT_Client NetworkManager { get; private set; }

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
        client.Subscribe(new string[] { "mic0" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "mic1" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "mic2" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "mic3" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    /// <summary>
    /// Unsubscribes the client to the broker for each microphone.
    /// </summary>
    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "mic0" });
        client.Unsubscribe(new string[] { "mic1" });
        client.Unsubscribe(new string[] { "mic2" });
        client.Unsubscribe(new string[] { "mic3" });
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
        Debug.Log("Topic: " + topic + " ~ Received: " + msg);
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

