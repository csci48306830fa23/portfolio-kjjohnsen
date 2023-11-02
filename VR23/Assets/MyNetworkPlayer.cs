//using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VelNet;
using VelUtils;
using GLTFast;

[RequireComponent(typeof(NetworkObject))]
public class MyNetworkPlayer : SyncState
{
	public Transform head;
	public Transform headOffset;
	public Transform leftHand;
	public Transform rightHand;
	public Transform leftHandOffset;
	public Transform rightHandOffset;
	bool leftGrip = false;
	bool rightGrip = false;
	NetworkObject networkObj;
	public string avatarURL = "";
	public Rig r;
	public GameObject avatar;
	public RuntimeAnimatorController controller;
	// Start is called before the first frame update
	void Start()
	{
		networkObj = GetComponent<NetworkObject>();

	}

	public void setAvatar(string avatarURL)
	{
		this.avatarURL = avatarURL;
		StartCoroutine(downloadAvatar());
	}

	// Update is called once per frame
	void LateUpdate()
	{

		if (networkObj.IsMine)
		{
			if (r != null)
			{
				this.transform.position = r.transform.position;
				this.transform.rotation = r.transform.rotation;
				head.position = r.head.position;
				head.rotation = r.head.rotation;
				leftHand.position = r.leftHand.position;
				leftHand.rotation = r.leftHand.rotation;
				rightHand.position = r.rightHand.position;
				rightHand.rotation = r.rightHand.rotation;

				leftGrip = InputMan.Grip(Side.Left);
				rightGrip = InputMan.Grip(Side.Right);


			}
		}
		else
		{

		}

		if (avatar != null)
		{
			Transform leftEyeAvatar = avatar.transform.Find("Hips/Spine/Neck/Head/LeftEye");
			Transform rightEyeAvatar = avatar.transform.Find("Hips/Spine/Neck/Head/RightEye");
			Transform neckAvatar = avatar.transform.Find("Hips/Spine/Neck");
			Transform headAvatar = avatar.transform.Find("Hips/Spine/Neck/Head");
			Transform leftHandAvatar = avatar.transform.Find("Hips/Spine/LeftHand");
			Transform rightHandAvatar = avatar.transform.Find("Hips/Spine/RightHand");

			headAvatar.rotation = head.rotation; //first set the head rotation

			Vector3 neckXZ = neckAvatar.forward;
			neckXZ.y = 0;
			neckXZ.Normalize();
			Vector3 headXZ = headAvatar.forward;
			headXZ.y = 0;
			headXZ.Normalize();
			float headNeckOffset = Vector3.SignedAngle(neckXZ, headXZ, Vector3.up);

			if (Mathf.Abs(headNeckOffset) > 40)
			{
				avatar.transform.Rotate(Vector3.up, headNeckOffset * Time.deltaTime);
				headAvatar.rotation = head.rotation; //correct the head rotation
			}


			Vector3 pos = (leftEyeAvatar.position + rightEyeAvatar.position) / 2; //this is where the HMD should be
			Vector3 offset = headOffset.position - pos; //this is the vector that would move the hmd into position


			avatar.transform.position += offset;

			leftHandAvatar.position = leftHandOffset.position;
			rightHandAvatar.position = rightHandOffset.position;
			leftHandAvatar.rotation = leftHandOffset.rotation;
			rightHandAvatar.rotation = rightHandOffset.rotation;

			var animator = this.GetComponent<Animator>();
			animator.SetBool("CloseHandRight", rightGrip);
			animator.SetBool("CloseHandLeft", leftGrip);

		}

	}

	protected override void SendState(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(avatarURL);
		binaryWriter.Write(leftGrip);
		binaryWriter.Write(rightGrip);
	}

	protected override void ReceiveState(BinaryReader binaryReader)
	{
		string target_avatarURL = binaryReader.ReadString();
		if (target_avatarURL != avatarURL)
		{
			avatarURL = target_avatarURL;
			//load the avatar
			StartCoroutine(downloadAvatar());
		}
		leftGrip = binaryReader.ReadBoolean();
		rightGrip = binaryReader.ReadBoolean();
	}


	IEnumerator downloadAvatar()
	{

		if (avatar != null)
		{
			GameObject.Destroy(avatar);
			avatar = null;
		}
		GltfAsset gltf = gameObject.AddComponent<GltfAsset>();
		ImportSettings importSettings = new ImportSettings();
		importSettings.AnimationMethod = AnimationMethod.Mecanim;
		gltf.ImportSettings = importSettings;

		var t = gltf.Load(avatarURL);
		while (!t.IsCompleted)
		{
			yield return null;
		}
		avatar = this.transform.Find("AvatarRoot").gameObject;
		this.GetComponent<Animator>().runtimeAnimatorController = controller;


		yield return null;


	}
	void rigAvatar()
	{

	}
}
