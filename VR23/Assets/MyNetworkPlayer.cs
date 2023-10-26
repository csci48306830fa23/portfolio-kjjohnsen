using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VelNet;
using VelUtils;
[RequireComponent(typeof(NetworkObject))]
public class MyNetworkPlayer : SyncState
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    NetworkObject networkObj;
    public string avatarURL = "";
    public Rig r;
    public GameObject avatar;
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
    void Update()
    {

        if (networkObj.IsMine)
        {
            if(r != null)
            {
				this.transform.position = r.transform.position;
				this.transform.rotation = r.transform.rotation;
				head.position = r.head.position;
				head.rotation = r.head.rotation;
				leftHand.position = r.leftHand.position;
				leftHand.rotation = r.leftHand.rotation;
				rightHand.position = r.rightHand.position;
				rightHand.rotation = r.rightHand.rotation;
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
			//headAvatar.transform.position = head.position;
			//headAvatar.transform.rotation = head.rotation;
			//leftHandAvatar.transform.position = leftHand.position;
			//leftHandAvatar.transform.rotation = leftHand.rotation;
			//rightHandAvatar.transform.position = rightHand.position;
			//rightHandAvatar.transform.rotation = rightHand.rotation;

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


			Vector3 pos = (leftEyeAvatar.position + rightEyeAvatar.position) / 2; //this is where the eyes are at currently
			Vector3 offset = head.position - pos; //this is the vector that would move the eye position to hmd position

			avatar.transform.position += offset;

			

			leftHandAvatar.position = leftHand.position;
			rightHandAvatar.position = rightHand.position;
			leftHandAvatar.rotation = leftHand.rotation;
			rightHandAvatar.rotation = rightHand.rotation;


		}

	}

	protected override void SendState(BinaryWriter binaryWriter)
	{
        binaryWriter.Write(avatarURL);
	}

	protected override void ReceiveState(BinaryReader binaryReader)
	{
        string target_avatarURL = binaryReader.ReadString();
        if(target_avatarURL != avatarURL)
        {
            avatarURL = target_avatarURL;
            //load the avatar
            StartCoroutine(downloadAvatar());
        }
	}


	IEnumerator downloadAvatar()
	{
        if (avatar != null)
        {
            GameObject.Destroy(avatar);
            avatar = null;
        }
		using (UnityWebRequest webRequest = UnityWebRequest.Get(avatarURL))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();


			switch (webRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError("Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError("HTTP Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					Debug.Log("nReceived: " + webRequest.downloadHandler.data.Length);

					avatar = Importer.LoadFromBytes(webRequest.downloadHandler.data);
                    rigAvatar();
					break;
			}
		}
	}
    void rigAvatar()
    {

    }
}
