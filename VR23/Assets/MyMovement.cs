using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using VelUtils;

public class MyMovement : MonoBehaviour
{
    [SerializeField]
    Transform leftController;
    [SerializeField]
    Transform rightController;

    [SerializeField]
    Transform hmd;

    [SerializeField]
    Transform body;

    [SerializeField]
    LineRenderer rightTeleportLine;

    bool canRotate = true;
    bool canTeleport = true;
	bool teleportPointValid = false;
	Vector3 teleportPoint = Vector3.zero;
	// Start is called before the first frame update
	void Start()
    {
		if (XRDevice.refreshRate != 0)
		{
			Time.fixedDeltaTime = 1 / XRDevice.refreshRate;
		}
	}

    

    // Update is called once per frame
    void Update()
    {
        //move my feet
        Vector3 bodyPosition = hmd.transform.position;
        bodyPosition.y = transform.position.y;
        body.transform.position = bodyPosition;
        float leftTrigger = Input.GetAxis("VR_Trigger_Right");

		if (leftTrigger > 0)
        {
            //transform.Translate(leftController.forward*Time.deltaTime, Space.World);
            Vector3 v = leftController.forward;
            v.y = 0;
            GetComponent<Rigidbody>().velocity = v.normalized*leftTrigger;
        }

        Vector2 rightThumbstick = new Vector2(
            Input.GetAxis("VR_Thumbstick_X_Right"),
            Input.GetAxis("VR_Thumbstick_Y_Right"));
        //Debug.Log(rightThumbstick);

        if(Mathf.Abs(rightThumbstick.x) > .8f && canRotate)
        {
            canRotate = false;
            //do a rotation
            transform.RotateAround(hmd.position, Vector3.up, rightThumbstick.x > 0 ? 30 : -30);
        }

        if(Mathf.Abs(rightThumbstick.x) < .7f){
            canRotate = true;
        }

        
        
        if (rightThumbstick.y < -.8f)
        {
            canTeleport = false;
			rightTeleportLine.gameObject.SetActive(true);
			Vector3 currentPosition = rightController.position + rightController.forward*.1f; //start 10cm ahead of the controller
            Vector3 currentVelocity = rightController.forward*10f;

            rightTeleportLine.positionCount = 50;
            rightTeleportLine.startWidth = .03f;
            rightTeleportLine.endWidth = .03f;
            teleportPointValid = false;
			for (int i = 0;i < 50; i++)
            {
                //do the raycast
                if( i > 0)
                {
                    Vector3 lastPosition = rightTeleportLine.GetPosition(i - 1);
                    RaycastHit hit;
                    if(Physics.Raycast(lastPosition, currentVelocity.normalized, out hit, (currentVelocity * .1f).magnitude))
                    {
                        rightTeleportLine.SetPosition(i, hit.point);
                        rightTeleportLine.positionCount = i + 1;
                        teleportPoint = hit.point;
                        teleportPointValid = true;
                        break;
                    }
                }
				rightTeleportLine.SetPosition(i, currentPosition);
               
				currentPosition += currentVelocity * .1f;
                currentVelocity += Physics.gravity * .1f;
                
                
   
            }

            //if (canTeleport)
            //{
            //    Vector3 teleportForward = rightController.transform.forward;
            //    teleportForward.y = 0;

            //    canTeleport = false;
            //    //do a teleportation
            //    transform.Translate(teleportForward.normalized, Space.World);
            //}
        }

		if (rightThumbstick.y > -.7f) 
		{
            if(canTeleport == false && teleportPointValid) //the teleporter was active
            {
                Vector3 footPosition = hmd.position;
                footPosition.y = transform.position.y;
                Vector3 t = teleportPoint - footPosition;
                
                transform.Translate(t, Space.World);  
            }
            
            rightTeleportLine.gameObject.SetActive(false);
			canTeleport = true;
		}



	}
}
