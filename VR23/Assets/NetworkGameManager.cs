using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VELConnect;
using VelNet;
using VELShareUnity;
using VelUtils;
public class NetworkGameManager : MonoBehaviour
{
    public Rig rig;
    public string roomToJoin = "default";
    public MyNetworkPlayer playerPrefab;
    public MyNetworkPlayer player;
    public TMP_Text infoText;
    public string avatar_url="";
    public WebRTCReceiver videoBoard;
    // Start is called before the first frame update
    void Start()
    {

        infoText.text = ""+VELConnectManager.PairingCode;
        VELConnectManager.AddDeviceDataListener("avatar_url", this, (avatar_url) => {
            this.avatar_url = avatar_url;
            if(player != null)
            {
                player.setAvatar(avatar_url);
            }
        }, true);

        VELConnectManager.AddDeviceDataListener("streamer_stream_id", this, (stream_id) =>
        {
            videoBoard.streamRoom = stream_id;
            videoBoard.Startup(videoBoard.streamRoom);
        }, false);
        VelNetManager.OnLoggedIn += () =>
        {
            VelNetManager.JoinRoom(roomToJoin);
        };

        VelNetManager.OnJoinedRoom += (room) =>
        {

            //instantiate the player prefab
            player = VelNetManager.NetworkInstantiate(playerPrefab.name).GetComponent<MyNetworkPlayer>();
            player.r = rig;
            if(avatar_url != "")
            {
                player.setAvatar(avatar_url);
            }

        };

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
