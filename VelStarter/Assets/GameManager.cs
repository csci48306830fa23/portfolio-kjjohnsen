using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VELConnect;
using VelNet;
using VELShareUnity;
using VelUtils;
using VelUtils.VRInteraction;

public class GameManager : MonoBehaviour
{
	public Rig rig;
	public OVRHand trackedHandLeft;
	public OVRHand trackedHandRight;
	public string roomToJoin = "default";
	public Player playerPrefab;
	public Player player;
	public TMP_Text infoText;
	public string avatar_url = "";
	public WebRTCReceiver videoBoard;
	public List<VRGrabbableHand> grabbers = new List<VRGrabbableHand>();
	// Start is called before the first frame update
	IEnumerator Start()
	{
		infoText.text = "" + VELConnectManager.PairingCode;


		VELConnectManager.AddDeviceDataListener("streamer_stream_id", this, (stream_id) =>
		{
			videoBoard.streamRoom = stream_id;
			videoBoard.Startup(videoBoard.streamRoom);
		}, false);

		bool hasInitialState = false;
		VELConnectManager.OnInitialState += (VELConnectManager.State state) => {
			hasInitialState = true;
		};
		bool isLoggedIn = false;
		VelNetManager.OnLoggedIn += () =>
		{

			isLoggedIn = true;
		};

		while(!hasInitialState || !isLoggedIn)
		{
			yield return null;
		}

		VelNetManager.OnJoinedRoom += (room) =>
		{
			//instantiate the player prefab
			player = VelNetManager.NetworkInstantiate(playerPrefab.name).GetComponent<Player>();
			player.r = rig;
			player.trackedHandLeft.setOVRHand(trackedHandLeft);
			player.trackedHandRight.setOVRHand(trackedHandRight);

		};

		VelNetManager.JoinRoom(roomToJoin);

		while (!VelNetManager.InRoom)
		{
			yield return null;
		}

		VELConnectManager.AddDeviceDataListener("avatar_url", this, (avatar_url) => {
			this.avatar_url = avatar_url;
			player.setAvatar(avatar_url);
		}, true);

		foreach(VRGrabbableHand grabber in grabbers)
		{
			grabber.OnGrab += (grabbable) =>
			{
				grabbable.GetComponent<NetworkObject>()?.TakeOwnership();
			};
		}

		StartCoroutine(GetRoomList());

	}

	IEnumerator GetRoomList()
	{
		while (true)
		{
			VelNetManager.GetRooms();
			yield return new WaitForSeconds(1.0f);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
