using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTTester : MonoBehaviour
{
    public TMP_Text text;
    MqttClient mqttClient;
    Queue<string> messages = new Queue<string>();
    // Start is called before the first frame update
    void Start()
    {
        mqttClient = new MqttClient("eduoracle.ugavel.com");
        mqttClient.Connect(SystemInfo.deviceUniqueIdentifier,"giiuser","giipassword");

        mqttClient.MqttMsgPublishReceived += (sender, e) =>
        {
			//note, this is received in a different thread...
			//if you want to do something on the UI thread, you'll need to post it to a locked place
            //but debug.log is thread safe
			//Debug.Log("got message " + Encoding.UTF8.GetString(e.Message));
            lock (messages)
            {
                messages.Enqueue(Encoding.UTF8.GetString(e.Message));
            }
        };
        mqttClient.Subscribe(new string[] { "/test/mytest" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        StartCoroutine(sendStuff());
    }

    // Update is called once per frame
    void Update()
    {
        
        lock (messages)
        {
            while (messages.Count > 0)
            {
                var m = messages.Dequeue();
                text.text = m;
            }
        }
    }

    IEnumerator sendStuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
			//mqttClient.Publish("/test/mytest", Encoding.UTF8.GetBytes("" + Input.mousePosition.ToString()));

		}
	}


}
