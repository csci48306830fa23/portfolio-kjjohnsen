using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelUtils;

public class MyGrabber : MonoBehaviour
{
    public Side side;
    public MyGrabbable grabbed; //will be null if not grabbed
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerStay(Collider other)
	{
        MyGrabbable grabbable = other.attachedRigidbody?.GetComponent<MyGrabbable>();
        if (grabbable != null && grabbable.grabbedBy != this)
        {
            if (InputMan.GripDown(side))
            {
                grabbable.handleGrab(this);
            }
        }
	}
}
