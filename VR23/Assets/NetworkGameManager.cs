using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelNet;
public class NetworkGameManager : MonoBehaviour
{
    public string roomToJoin = "default";
    // Start is called before the first frame update
    void Start()
    {

        VelNetManager.OnLoggedIn += () =>
        {
            VelNetManager.JoinRoom(roomToJoin);
        };

        VelNetManager.OnJoinedRoom += (room) =>
        {

        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
