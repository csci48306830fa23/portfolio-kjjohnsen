using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VelUtils;
using VelUtils.VRInteraction;

//this class demonstrates how to do basic tracked hands grabbing
public class HandTrackingInteraction : MonoBehaviour
{
    public OVRHand hand;
    public Movement movement; //mostly used for rig, but could be used for teleporting or other actions
    public VRGrabbableHand grabber;  //This will be moved, so it's not exactly one of the hands
    bool wasPinching = false;
    bool airGrab = false;
    Vector3 lastPinchPos;
    Vector3 grabPositionRigSpace;
    Vector3 rigPosition;
    // Start is called before the first frame update
    void Start()
    {
        grabber.lastVels.Enqueue(Vector3.zero); //hack for quest, which won't treat hands like controllers

	}

    // Update is called once per frame
    void Update()
    {
		bool isPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        
		Vector3 pinchPos = (hand.GetComponent<OVRCustomSkeleton>().CustomBones[(int)OVRSkeleton.BoneId.Hand_IndexTip].position +
								   hand.GetComponent<OVRCustomSkeleton>().CustomBones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].position) / 2.0f;
        
        Vector3 pinchPosRigSpace = movement.rig.transform.InverseTransformPoint(pinchPos); //The rig space position is useful because it doesn't shift if the rig moves

		if (isPinching && wasPinching) //do some filtering
		{
			pinchPosRigSpace = lastPinchPos * .9f + pinchPosRigSpace * .1f;
			lastPinchPos = pinchPosRigSpace;
            

		}
		grabber.transform.localPosition = pinchPosRigSpace; //we move the grabber to coincide with the pinch position (could visualize)

		if (hand.IsTracked)
        {
            if (isPinching && !wasPinching)
            {
                wasPinching = true;
                lastPinchPos = pinchPosRigSpace;
                Collider[] collisions = Physics.OverlapSphere(pinchPos, .01f);
                bool grabbedSomething = false;
                foreach(var c in collisions)
                {
                    if (c.attachedRigidbody?.GetComponent<VRGrabbable>())
                    {
                        grabber.Grab(c.attachedRigidbody?.GetComponent<VRGrabbable>());
                        grabbedSomething = true;
                        break;
                    }
                }
                if (!grabbedSomething)
                {
                    airGrab = true;
                    grabPositionRigSpace = pinchPosRigSpace;
                    rigPosition = movement.rig.transform.position;
                }
                
            }
            else if(!isPinching && wasPinching)
            {
                wasPinching = false;
                grabber.Release();
                if (airGrab)
                {
                    airGrab = false;
                }
            }
            if (airGrab)
            {
                movement.rig.transform.position = rigPosition + movement.rig.transform.TransformVector(grabPositionRigSpace - pinchPosRigSpace);
               
            }
        }

	}
}
