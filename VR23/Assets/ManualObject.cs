using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualObject : MonoBehaviour
{
    public Shader shader;
	// Start is called before the first frame update
	void Start()
    {
        Mesh mesh = new Mesh();
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        Vector3[] verts = new Vector3[3];

        verts[0] = new Vector3(0, 0, 0);
        verts[1] = new Vector3(0, 1, 0);
        verts[2] = new Vector3(1, 1, 0);

        mesh.vertices = verts;

		int[] indices = new int[3];
		indices[0] = 0;
		indices[1] = 1;
		indices[2] = 2;

        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

		mf.mesh = mesh;

        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = new Material(shader);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
