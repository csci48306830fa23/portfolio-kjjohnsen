using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Transactions.Configuration;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VelNet;
using VelUtils.VRInteraction;

public class SyncedSpatialAnchor : SyncState
{
	public Vector3 targetPosition;
	public Quaternion targetRotation;
	public VRMoveable moveable;
    public string network_uuid;
	public string local_uuid;
	public List<ulong> requestedShares = new List<ulong>();
	public Transform rig;
	public TMP_Text text;

	protected override void ReceiveState(BinaryReader binaryReader)
	{
		var temp = binaryReader.ReadString();
		if(temp != network_uuid)
		{
			network_uuid = temp;
			StartCoroutine(eraseAnchor()); //clear the anchor now, because it's going to change

		}
		targetPosition = binaryReader.ReadVector3();
		targetRotation = binaryReader.ReadQuaternion();
	}

	protected override void SendState(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(network_uuid);
		binaryWriter.Write(moveable.transform.position);
		binaryWriter.Write(moveable.transform.rotation);
	}


	// Start is called before the first frame update
	void Start()
    {
		moveable.Grabbed += () =>
		{
			this.networkObject.TakeOwnership();
			network_uuid = ""; //will not try to find the anchor now
			StartCoroutine(eraseAnchor()); //you can't move the anchor until it's deleted
			

		};
		moveable.Released += () =>
		{
			
			StartCoroutine(createAnchor());
		};

		network_uuid = PlayerPrefs.GetString("network_uuid","");

		StartCoroutine(findAnchor());
		
	}

	IEnumerator findAnchor()
	{
		while (true)
		{
			if (local_uuid != network_uuid && network_uuid != "") //don't do this if we've already found it, or it's not set to anything real
			{
				text.text = "Erasing anchor";
				yield return StartCoroutine(eraseAnchor()); //erase any existing anchor
				text.text = "Loading cloud anchor " + network_uuid;
				OVRSpatialAnchor.LoadOptions options = new OVRSpatialAnchor.LoadOptions();
				options.StorageLocation = OVRSpace.StorageLocation.Cloud;
				options.Uuids = new System.Guid[] { System.Guid.Parse(network_uuid) };
				var t = OVRSpatialAnchor.LoadUnboundAnchorsAsync(options);
				yield return new WaitUntil(() => t.IsCompleted);

				text.text = "Finished loading anchor";
				var anchors = t.GetResult();
				
				
				if (anchors != null && anchors.Length > 0)
				{
					text.text = "Localizing anchor";
					var t2 = anchors[0].LocalizeAsync();
					yield return new WaitUntil(() => t2.IsCompleted);
					if (t2.GetResult())
					{
						text.text = "Anchor localized";
						local_uuid = network_uuid;

						var pose = anchors[0].Pose;
						moveable.transform.position = pose.position;
						moveable.transform.rotation = pose.rotation;
						OVRSpatialAnchor anchor = moveable.AddComponent<OVRSpatialAnchor>();
						anchors[0].BindTo(anchor);

					}
				}
				else
				{
					text.text = "Could not load anchor" ;
				}
			}
			yield return new WaitForSeconds(1.0f) ;
		}
	}

	IEnumerator eraseAnchor()
	{
		OVRSpatialAnchor anchor = this.GetComponent<OVRSpatialAnchor>();
		if (anchor != null) 
		{
			anchor.enabled = false; //allows me to move it immediately without destroying it yet
			var t = anchor.EraseAsync();
			yield return new WaitUntil(() => t.IsCompleted);
			GameObject.Destroy(anchor);
			yield return null;
		}
		yield return null;
	}

	IEnumerator createAnchor()
	{
		network_uuid = "";
		yield return StartCoroutine(eraseAnchor());
		text.text = "Creating anchor";
		OVRSpatialAnchor anchor = moveable.AddComponent<OVRSpatialAnchor>();
		while (!anchor.Created && !anchor.Localized)
		{
			yield return null;

		}

		text.text = "Anchor created";

		var t2 = anchor.SaveAsync();
		yield return new WaitUntil(() => t2.IsCompleted);
		text.text = "Anchor saved locally: " + t2.GetResult() + "\nSaving to Cloud";
		yield return new WaitForSeconds(1.0f);
		OVRSpatialAnchor.SaveOptions so = new OVRSpatialAnchor.SaveOptions();
		so.Storage = OVRSpace.StorageLocation.Cloud;

		var t3 = anchor.SaveAsync(so);

		yield return new WaitUntil(() => t3.IsCompleted); //save to cloud!

		var res = t3.GetResult();
		if (res && networkObject.IsMine)
		{
			
			network_uuid = anchor.Uuid.ToString();
			local_uuid = network_uuid;
			PlayerPrefs.SetString("network_uuid", network_uuid);
			requestedShares.Clear(); //time to update everyone
			text.text = network_uuid;
		}
		else
		{
			text.text = "failed to save to anchor to cloud " + res;
		}

		yield return null;
	}

	// Update is called once per frame
	void Update()
    {
		var anchor = moveable.GetComponent<OVRSpatialAnchor>();
		if (!networkObject.IsMine && anchor.enabled && anchor.Localized) //everyone else needs to move their rigs to compensate.  The owner doesn't
		{
			Vector3 anchorPosition = anchor.transform.position;
			Quaternion anchorRotation = anchor.transform.rotation;
			Vector3 pOffset = targetPosition - anchorPosition;
			Quaternion rOffset = targetRotation * Quaternion.Inverse(anchorRotation);
			

			rig.rotation = rOffset * rig.rotation;
			Vector3 rigOffset = (rOffset * pOffset);
			rig.position += rigOffset;
		}

		
		

		
		MetaIDSync[] playerSyncIDs = FindObjectsOfType<MetaIDSync>();
		foreach(var p in playerSyncIDs)
		{
			if (p.MetaID == 0 || p.networkObject.IsMine || !moveable.GetComponent<NetworkObject>().IsMine)
			{
				continue;
			}

			if (!requestedShares.Contains(p.MetaID))
			{
				OVRSpaceUser user = new OVRSpaceUser(p.MetaID);
				
				anchor.ShareAsync(user);
				requestedShares.Add(p.MetaID);
				
			}
		}
    }
}
