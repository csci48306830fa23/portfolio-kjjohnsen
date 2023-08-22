using UnityEngine;

public class Triangle : MonoBehaviour
{
	private void Start()
	{
		Debug.Log(transform.localToWorldMatrix);
		Debug.Log(transform.worldToLocalMatrix);
	}
	private void Update()
	{
		
	}
}
