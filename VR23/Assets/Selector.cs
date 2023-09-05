using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField]
    Transform rayOrigin;
    [SerializeField]
    Material saveMaterial;
    Transform selectObject; //will be set to any selected object by the raycast
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void FixedUpdate()
	{
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit))
        {
            Debug.DrawLine(
                rayOrigin.position,
                rayOrigin.position + rayOrigin.forward * hit.distance,
                Color.red);
            Debug.Log(hit.transform.name + " " + hit.distance);

            Renderer r = hit.transform.GetComponent<Renderer>();
            if (r != null)
            {
                
                if (selectObject != null && hit.transform != selectObject)
                {   //we had an object
                    selectObject.GetComponent<Renderer>().sharedMaterial = saveMaterial;
                }
                //todo, put if statement preventing this from re-executing
                selectObject = hit.transform;
                saveMaterial = r.sharedMaterial;
                r.material.color = Color.red;
            }

        }
        else
        {
            if (selectObject != null)
            {
                selectObject.GetComponent<Renderer>().sharedMaterial = saveMaterial;
                selectObject = null;
            }
        }
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
