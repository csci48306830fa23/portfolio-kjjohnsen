using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class SpawnedObject : MonoBehaviour
{
    float lifeTime_s = 100; //seconds
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        lifeTime_s -= Time.deltaTime;

        if(lifeTime_s < 0)
        {
            GameObject.Destroy(gameObject);
        }
        
    }

	private void OnCollisionEnter(Collision collision)
	{
		//GameObject.Destroy(gameObject);
	}

    private void OnTriggerStay(Collider other)
    {
        /*        Rigidbody rb = other.attachedRigidbody;
                if (rb != null)
                {
                    DangerZone dz = rb.GetComponent<DangerZone>();
                    if(dz != null)
                    {
                        lifeTime_s -= Time.deltaTime * 50;
                    }
                }*/

        DangerZone dz = other.attachedRigidbody?.GetComponent<DangerZone>();
		if (dz != null)
		{
			lifeTime_s -= Time.deltaTime * 50;
		}

	}


}
