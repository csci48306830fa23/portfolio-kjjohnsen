using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using VelNet;

public class MetaIDSync : SyncState
{
	public TMP_Text id_text;
	public ulong MetaID;
	protected override void ReceiveState(BinaryReader binaryReader)
	{
		MetaID = binaryReader.ReadUInt64();
		id_text.text = MetaID + "";
	}

	protected override void SendState(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(MetaID);
	}

	void EntitlementCallback(Message msg)
	{
		if (msg.IsError)
		{
			Debug.LogError("Oculus Platform entitlement error: " + msg.GetError());
		}
		else
		{
			Debug.Log("Oculus Platform entitlement success! " + msg);
			Users.GetLoggedInUser().OnComplete(m => {
				if (!m.IsError && m.Type == Message.MessageType.User_GetLoggedInUser)
				{
					MetaID = m.GetUser().ID;
					id_text.text = MetaID + "";
				}
				else
				{
					UnityEngine.Application.Quit();
				}
			});
		}
	}

	// Start is called before the first frame update
	IEnumerator Start()
    {
		if (IsMine)
		{
			Oculus.Platform.Core.AsyncInitialize();
			Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
		}
		yield return null;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
