using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeeball : MonoBehaviour
{
    [SerializeField]
    GameObject ballPrefab;
    [SerializeField]
    Transform ballSpawnLocation;

    [SerializeField]
    HoleTrigger trigger10;
	[SerializeField]
	HoleTrigger trigger20;
	[SerializeField]
	HoleTrigger trigger30;
	[SerializeField]
	HoleTrigger trigger40;
	[SerializeField]
	HoleTrigger trigger50;

    float points;
	// Start is called before the first frame update
	void Start()
    {
        trigger10.pointValue = 10;
        trigger20.pointValue = 20;
        trigger30.pointValue = 30;
        trigger40.pointValue = 40;
        trigger50.pointValue = 50;
        spawnBall();
        points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnBall()
    {
        GameObject.Instantiate<GameObject>(ballPrefab, 
            ballSpawnLocation.position, Quaternion.identity);
    }

    public void sensorDetected(HoleTrigger trigger)
    {
        points += trigger.pointValue;
        spawnBall();
    }


}
