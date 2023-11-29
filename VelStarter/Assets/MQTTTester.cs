using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTTester : MonoBehaviour
{
    public TMP_Text text;
    MqttClient mqttClient;
    Queue<Tuple<string,byte[]>> messages = new Queue<Tuple<string, byte[]>>();
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        mqttClient = new MqttClient("eduoracle.ugavel.com",1883,false,null,null,MqttSslProtocols.None);
        mqttClient.Connect(SystemInfo.deviceUniqueIdentifier,"giiuser","giipassword");
        
        mqttClient.MqttMsgPublishReceived += (sender, e) =>
        {
			//note, this is received in a different thread...
			//if you want to do something on the UI thread, you'll need to post it to a locked place
            //but debug.log is thread safe
			Debug.Log("got message:" + e.Topic + ":" + Encoding.UTF8.GetString(e.Message));

            lock (messages)
            {
                
                messages.Enqueue(new Tuple<string, byte[]>(e.Topic, e.Message));
            }
        };
        mqttClient.Subscribe(new string[] { "mqttpublisher/kyle/attitude" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        StartCoroutine(sendStuff());
    }

    // Update is called once per frame
    void Update()
    {
        
        lock (messages)
        {
            Debug.Log(messages.Count);
            while (messages.Count > 0)
            {
                var m = messages.Dequeue();
                int start = m.Item1.IndexOf("/")+1;
                int end = m.Item1.IndexOf("/attitude");
                string sender = m.Item1.Substring(start, (end - start));
                MemoryStream ms = new MemoryStream(m.Item2);
                BinaryReader br = new BinaryReader(ms);
                float x = br.ReadSingle();
                float y = br.ReadSingle();
                float z = br.ReadSingle();
                float w = br.ReadSingle();

                text.text = sender + "\n"+x+","+y+","+z+","+w;
                this.transform.rotation = new Quaternion(x, y, z, w);
                
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

	private void OnApplicationQuit()
	{
        if (mqttClient != null)
        {
            mqttClient.Disconnect();
            mqttClient = null;
        }
	}


}
