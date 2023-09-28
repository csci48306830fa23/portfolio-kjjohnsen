using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VelUtils.VRInteraction;

public class CanKnockdown : MonoBehaviour
{

    [SerializeField]
    GameObject[] cans;
    int numThrowsLeft=3;
    int knockedDownCount = 0;
	[SerializeField]
	TMP_Text infoText;

    [SerializeField]
    Transform pedestal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
           
    }

	private void OnTriggerEnter(Collider other)
	{
        SkeeballBall ball = other.attachedRigidbody?.GetComponent<SkeeballBall>();
        if (ball != null)
        {
            VRMoveable moveable = ball.GetComponent<VRMoveable>();
            if (moveable.GrabbedBy != null)
            {
                //this is being held
                moveable.GrabbedBy.Release();
                GameObject.Destroy(moveable.gameObject);
            }
            else
            {
                StartCoroutine(waitForComplete(ball));
            }
        }
    }

    IEnumerator waitForComplete(SkeeballBall ball)
    {
        float timeLeft = 2.0f; //2second timer for the ball to finish moving
        while (timeLeft > 0 && ball.transform.position.y > pedestal.position.y)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

		int tempCount = 0;
		for (int i = 0; i < cans.Length; i++)
		{
			Vector3 canUp = cans[i].transform.up;
			float upAngle = Vector3.Angle(canUp, Vector3.up);
			Vector3 canPos = cans[i].transform.position;
			Vector3 pedTopPos = pedestal.position;
			Vector3 canToPed = canPos - pedTopPos; //vector from the pedestal center/top to the can origin
			if (upAngle > 20 || canToPed.y < 0)
			{
				tempCount++;
			}
		}
		//this is where we can detect a change
		knockedDownCount = tempCount;
		infoText.text = "" + knockedDownCount + "\n" + numThrowsLeft;
		if (knockedDownCount == cans.Length)
		{
            //game over, you win
        }
        else
        {
            numThrowsLeft--;
        }



	}
}
