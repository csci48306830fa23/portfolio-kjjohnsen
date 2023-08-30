using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputExample : MonoBehaviour
{

    [SerializeField]
    SpawnedObject spawnedObjectPrefab;

    [SerializeField]
    Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
			SpawnedObject spawned = GameObject.Instantiate(spawnedObjectPrefab);
            Rigidbody rb = spawned.GetComponent<Rigidbody>();
            rb.position = spawnPoint.position;
            rb.velocity = new Vector3(
                Random.Range(-5.0f, 5.0f), 
                Random.Range(-5.0f, 5.0f), 
                Random.Range(-5.0f, 5.0f));


		}

        
    }
}
