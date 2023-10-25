using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VelNet;
public class MyNetworkSyncTransform : SyncState
{

	Vector3 targetPosition;
	Quaternion targetRotation;
	//this will only happen for the non-owner
	protected override void ReceiveState(BinaryReader binaryReader)
	{
		targetPosition = binaryReader.ReadVector3();
		targetRotation = binaryReader.ReadQuaternion();
	}

	//this will only happen for the owner
	protected override void SendState(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(transform.position);
		binaryWriter.Write(transform.rotation);
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!this.networkObject.IsMine)
		{
			transform.position = Vector3.Lerp(transform.position, targetPosition, .2f);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .2f);
		}
        
    }
}
