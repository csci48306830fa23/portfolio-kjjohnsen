using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelNet;
using VelUtils;
[RequireComponent(typeof(NetworkObject))]
public class MyNetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    NetworkObject networkObj;
    public Rig r;
    // Start is called before the first frame update
    void Start()
    {
        networkObj = GetComponent<NetworkObject>();
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
        
    }
}
