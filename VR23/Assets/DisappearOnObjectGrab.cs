using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelUtils.VRInteraction;

public class DisappearOnObjectGrab : MonoBehaviour
{
    [SerializeField]
    VRMoveable grabbedObject;
    // Start is called before the first frame update
    void Start()
    {
        grabbedObject.Grabbed += () => { 
            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
