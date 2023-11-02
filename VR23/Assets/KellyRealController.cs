using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class KellyRealController : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    public NavMeshAgent agent;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb  = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
		

	}
	private void OnAnimatorIK(int layerIndex)
	{
		anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
		anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
		anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
		anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
		anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
		anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
		anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
	}
	private void FixedUpdate()
	{
        Vector3 offset = agent.transform.position - this.transform.position;

		rb.velocity = offset.normalized * offset.magnitude;
        anim.SetFloat("walkspeed", rb.velocity.magnitude);
        offset.y = 0f;
        if(offset.magnitude > .2f)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
	}
}
