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
        if(InputMan.GripDown(side))
        {
            Collider[] collisions = Physics.OverlapSphere(transform.position, .05f);
            foreach(Collider collision in collisions)
            {
                MyGrabbable grabbable = collision.gameObject.GetComponent<MyGrabbable>();
                if(grabbable && !grabbed)
                {
                    grabbable.handleGrab(this);
                    grabbed = grabbable;
                }
            }
        }


        if(InputMan.GripUp(side))
        {
            grabbed.handleRelease();
            grabbed = null;
        }
    }
}
