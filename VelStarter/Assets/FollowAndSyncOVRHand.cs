using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using VelNet;
using Unity.VisualScripting;

public class FollowAndSyncOVRHand : SyncState
{
	public GameObject visibleHand;
    public Transform[] syncBones;
	Transform[] ovrBones;
    OVRHand hand;
    OVRCustomSkeleton skeleton;
	public bool isTracked = false;

	public void Update()
	{
		if (hand != null && IsMine)
		{
			isTracked = hand.IsTracked;
			//for some reason, we also have to set the visibility of the ovr hand, even though we normally would not have to...
			hand.transform.localScale = isTracked ? Vector3.one : Vector3.zero;
			visibleHand.SetActive(false); //we don't need the visible hand locally
		}
		else
		{
			visibleHand.SetActive(isTracked);
		}
	}
	public void setOVRHand(OVRHand ovrHand)
	{
		hand = ovrHand;
		skeleton = ovrHand.GetComponent<OVRCustomSkeleton>();
		ovrBones = new Transform[18];
		ovrBones[0] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_WristRoot];
		ovrBones[1] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Index1];
		ovrBones[2] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Index2];
		ovrBones[3] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Index3];
		ovrBones[4] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Middle1];
		ovrBones[5] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Middle2];
		ovrBones[6] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Middle3];
		ovrBones[7] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Ring1];
		ovrBones[8] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Ring2];
		ovrBones[9] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Ring3];
		ovrBones[10] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Pinky0];
		ovrBones[11] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Pinky1];
		ovrBones[12] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Pinky2];
		ovrBones[13] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Pinky3];
		ovrBones[14] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Thumb0];
		ovrBones[15] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Thumb1];
		ovrBones[16] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Thumb2];
		ovrBones[17] = skeleton.CustomBones[(int)OVRSkeleton.BoneId.Hand_Thumb3];
	}

	protected override void ReceiveState(BinaryReader binaryReader)
	{
		isTracked = binaryReader.ReadBoolean();
		if (isTracked)
		{
			for (int i = 0; i < syncBones.Length; i++)
			{
				sbyte x = binaryReader.ReadSByte();
				sbyte y = binaryReader.ReadSByte();
				sbyte z = binaryReader.ReadSByte();
				float qx = x / 127.0f;
				float qy = y / 127.0f;
				float qz = z / 127.0f;
				float qw = Mathf.Sqrt(1 - (qx * qx + qy * qy + qz * qz));
				//get the quaternion for the bone
				Quaternion q = new Quaternion(qx, qy, qz, qw);
				syncBones[i].localRotation = q;
			}
		}
	}

	//a somewhat efficient update strategy, but this could be refined, as most of the bones don't have 3dof rotation
	//we could also compress finger rotations into 4 bits each for a bigger reduction
	protected override void SendState(BinaryWriter binaryWriter)
	{
		
		binaryWriter.Write(isTracked);
		if (isTracked)
		{
			for (int i = 0; i < ovrBones.Length; i++)
			{
				//get the quaternion for the bone
				Quaternion q = ovrBones[i].localRotation;
				//now compress the quaternion into 3 bytes
				//each of the components of the quaternion is between -1 and 1, so multiply it by 127
				if (q.w < 0) //we must ensure that w is positive when ther rotation is recovered, and a quaternion's negation is the same quaternion (
				{
					q.x = -q.x;
					q.y = -q.y;
					q.z = -q.z;
					q.w = -q.w;  //inver the quaternion (same quanterion)
				}
				sbyte x = (sbyte)(q.x * 127);
				sbyte y = (sbyte)(q.y * 127);
				sbyte z = (sbyte)(q.z * 127);
				binaryWriter.Write(x); binaryWriter.Write(y); binaryWriter.Write(z);
				//note, we don't need w, because w^2 = 1-(x^2+y^2+z^2)
			}
		}
		
	}
}   
    
