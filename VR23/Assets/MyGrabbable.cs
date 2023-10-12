using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MyGrabbable : MonoBehaviour
{

    public MyGrabber grabbedBy; //will be null if not grabbed
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		if (grabbedBy == null) return;
		Vector3 target = grabbedBy.transform.position;
		Vector3 pos = transform.position;
		Vector3 between = target - pos;
		rb.velocity = between / Time.deltaTime;

        Quaternion targetRot = grabbedBy.transform.rotation;
        Quaternion rot = transform.rotation;
        Quaternion betweenRot = targetRot * Quaternion.Inverse(rot);
        float angle;
        Vector3 axis;
        betweenRot.ToAngleAxis(out angle, out axis);
        angle = angle * Mathf.Deg2Rad;
        rb.maxAngularVelocity = Mathf.Infinity;
        rb.angularVelocity = axis * angle / Time.deltaTime;
	}

	public void handleGrab(MyGrabber grabber)
    {
        grabbedBy = grabber;
    }

    public void handleRelease()
    {
        grabbedBy = null;
    }
}
