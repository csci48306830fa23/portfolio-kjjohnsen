using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelNet;
using VelUtils.VRInteraction;

public class NetworkGrabber : MonoBehaviour
{
    [SerializeField]
    VRGrabbableHand hand;
    // Start is called before the first frame update
    void Start()
    {
        hand.OnGrab += (grabbable) =>
        {
            NetworkObject no = grabbable.GetComponent<NetworkObject>();
            if (no != null && !no.IsMine)
            {
                no.TakeOwnership();
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
