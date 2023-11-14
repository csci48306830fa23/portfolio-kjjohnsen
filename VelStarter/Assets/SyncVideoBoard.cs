using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VelNet;
using VELShareUnity;

[RequireComponent(typeof(WebRTCReceiver))]
public class SyncVideoBoard : SyncState
{
	WebRTCReceiver receiver;
	string video_room;
	protected override void ReceiveState(BinaryReader binaryReader)
	{
		string temp = binaryReader.ReadString();
		if (temp != video_room)
		{
			receiver.Startup(video_room);
		}
	}

	protected override void SendState(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(video_room);
	}

	// Start is called before the first frame update
	void Start()
    {
        receiver = GetComponent<WebRTCReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
