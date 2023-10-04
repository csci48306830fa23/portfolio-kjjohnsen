using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelUtils;

public class MyMovement : MonoBehaviour
{
    [SerializeField]
    Transform leftController;
    [SerializeField]
    Transform rightController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float leftTrigger = Input.GetAxis("VR_Trigger_Left");

		if (leftTrigger > 0)
        {
            //transform.Translate(leftController.forward*Time.deltaTime, Space.World);
            Vector3 v = leftController.forward;
            v.y = 0;
            GetComponent<Rigidbody>().velocity = v.normalized*leftTrigger;
        }
    }
}
