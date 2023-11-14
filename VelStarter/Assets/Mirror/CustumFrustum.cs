using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CustumFrustum : MonoBehaviour
{
	public Transform eye;
	public float eyeOffset = .022f;
	public bool isRight = false;
	[Space]
	public Transform mirrorParent;
	public Vector2 dimensions;
	public float far = 1000;
	public float nearPlaneBuffer = .05f;

	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		if (eye == null) return;
		Vector3 eyePos = eye.position;
		eyePos += eye.right * eyeOffset * (isRight ? 1 : -1);
		Camera c = GetComponent<Camera>();
		Vector3 mirrorCamP = mirrorParent.worldToLocalMatrix.MultiplyPoint(eyePos);
		mirrorCamP.z = -mirrorCamP.z;
		transform.position = mirrorParent.localToWorldMatrix.MultiplyPoint(mirrorCamP);
		Vector3 p = -mirrorParent.worldToLocalMatrix.MultiplyPoint(transform.position);

		Matrix4x4 customMatrix = PerspectiveOffCenter(p.x - dimensions.x, p.x + dimensions.x, p.y - dimensions.y, p.y + dimensions.y, p.z, far);
		c.projectionMatrix = customMatrix;
		c.nearClipPlane = -mirrorCamP.z + nearPlaneBuffer;

	}

	private static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float x = 2.0F * near / (right - left);
		float y = 2.0F * near / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		Matrix4x4 m = new Matrix4x4
		{
			[0, 0] = x,
			[0, 1] = 0,
			[0, 2] = a,
			[0, 3] = 0,
			[1, 0] = 0,
			[1, 1] = y,
			[1, 2] = b,
			[1, 3] = 0,
			[2, 0] = 0,
			[2, 1] = 0,
			[2, 2] = c,
			[2, 3] = d,
			[3, 0] = 0,
			[3, 1] = 0,
			[3, 2] = e,
			[3, 3] = 0
		};
		return m;
	}

}
