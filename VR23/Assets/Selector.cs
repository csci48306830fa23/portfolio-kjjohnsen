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
    [SerializeField]
    Camera selectCamera;
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
            //Debug.Log(hit.transform.name + " " + hit.distance);

            Renderer r = hit.transform.GetComponent<Renderer>();
            if (r != null)
            {
                
                if (selectObject != null && hit.transform != selectObject)
                {   //we had an object
                    selectObject.GetComponent<Renderer>().sharedMaterial = saveMaterial;
                }
                if (selectObject != hit.transform)
                {
                    selectObject = hit.transform;
                    saveMaterial = r.sharedMaterial;
                    r.material.color = Color.red;
                }
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

        if (Input.GetMouseButtonDown(0)){ //true for the first frame the mouse button is held down
            var mousePosition = Input.mousePosition;
            Debug.Log(mousePosition);
            var r = selectCamera.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;
            int layerMask = LayerMask.GetMask(new string[] { "Selectable" });
            if(Physics.Raycast(r,out hitInfo,Mathf.Infinity, layerMask))
            {
                Rigidbody rb = hitInfo.rigidbody;
                if (rb != null)
                {
                    GameObject.Destroy(rb.gameObject);
                }
            }

        }
        
    }
}
