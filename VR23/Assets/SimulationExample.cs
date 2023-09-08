using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SimulationExample : MonoBehaviour
{
    [SerializeField]
    float speed_mps=1.0f; //meters per second

    [SerializeField]
    TMP_Text debugText;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(myUpdate());
        
    }

    IEnumerator moveForward(float timer)
    {
		
		while (timer > 0)
		{
			Vector3 velocity = speed_mps * transform.forward;
			transform.position += velocity * Time.deltaTime;
			timer -= Time.deltaTime;
			debugText.text = "" + timer;
			yield return null;
		}
	}
    IEnumerator myUpdate()
    {

        while (true)
        {
			debugText.text = "Hit Up!";
			if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                debugText.text = "Waiting 5 seconds to launch";
				yield return new WaitForSeconds(5.0f);

                yield return StartCoroutine(moveForward(5.0f));
            }
            yield return null;
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
