using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    [SerializeField]
    Skeeball machine;

    public float pointValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnTriggerEnter(Collider other)
	{
        SkeeballBall ball = other.attachedRigidbody?.GetComponent<SkeeballBall>();
        if (ball != null)
        {
            GameObject.Destroy(ball.gameObject);
            machine.sensorDetected(this);
        }
	}
}
