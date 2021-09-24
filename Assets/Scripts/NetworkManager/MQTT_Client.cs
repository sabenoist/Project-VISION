using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

public class MQTT_Client : M2MqttUnityClient
{
    protected override void OnConnecting()
    {
        base.OnConnecting();
    }

    protected override void OnConnected()
    {
        base.OnConnected();
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "mic0" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "mic1" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "mic2" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "mic3" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "mic0" });
        client.Unsubscribe(new string[] { "mic1" });
        client.Unsubscribe(new string[] { "mic2" });
        client.Unsubscribe(new string[] { "mic3" });
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Debug.Log("Topic: " + topic + " ~ Received: " + msg);
    }

    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()
    }

    private void OnDestroy()
    {
        Disconnect();
    }
}

