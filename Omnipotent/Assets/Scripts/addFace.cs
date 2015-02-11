using UnityEngine;
using System.Collections;

public class addFace : MonoBehaviour {
	Mesh mesh;
	// Use this for initialization
	void Start () {
		gameObject.AddComponent("MeshFilter");
		gameObject.AddComponent("MeshRenderer");
		mesh = CreatePlaneMesh();
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		Material newMat = Resources.Load("DiffuseMat", typeof(Material)) as Material;
		gameObject.renderer.material = newMat;
	}
	
	void Update ()
	{	
	}
	Mesh ModifyMesh(Mesh mesh)	//m is face mesh
	{	Vector3[] vertices = mesh.vertices;
		int p = 0;
		while (p < vertices.Length) {
			vertices[p] += new Vector3(0, Random.Range(-0.1F, 0.1F), 0);
			p++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		return mesh;
	}
	Vector4[] Solve(Mesh mesh) 
	{ 	int triangleCount = mesh.triangles.Length / 3; 
		int vertexCount = mesh.vertices.Length;
			
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];		
		Vector4[] tangents = new Vector4[vertexCount];
		
		for(long a = 0; a < triangleCount; a+=3)
		{	long i1 = mesh.triangles[a+0];
			long i2 = mesh.triangles[a+1];
			long i3 = mesh.triangles[a+2];				
			Vector3 v1 = mesh.vertices[i1];
			Vector3 v2 = mesh.vertices[i2];
			Vector3 v3 = mesh.vertices[i3];
			Vector2 w1 = mesh.uv[i1];
			Vector2 w2 = mesh.uv[i2];
			Vector2 w3 = mesh.uv[i3];
			
			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;
			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;
			
			float r = 1.0f / (s1 * t2 - s2 * t1);		
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;			
			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}
		for (long a = 0; a < vertexCount; ++a)
		{
			Vector3 n = mesh.normals[a];
			Vector3 t = tan1[a];				
			Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
			tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		return tangents;
	}
	Mesh CreatePlaneMesh()
	{	Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[]
		{	new Vector3( 1, 0, 1),
			new Vector3( 1, 0, -1),
			new Vector3(-1, 0, 1),
			new Vector3(-1, 0, -1),
			new Vector3( 0.5f, 2, 0.5f),
			new Vector3( 0.5f, 2, -0.5f),
			new Vector3(-0.5f, 2, 0.5f),
			new Vector3(-0.5f, 2, -0.5f),
		};		
		Vector2[] uvs = new Vector2[vertices.Length];
		int i = 0;
		while (i < uvs.Length) {
			uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
			i++;
		}
		int[] triangles = new int[]
		{	0, 1, 2,
			2, 1, 3,
			4, 5, 6,
			6, 5, 7,
			1, 5, 0,
			0, 5, 4,
			2, 6, 3,
			3, 6, 7, 
			3, 7, 1,
			1, 7, 5,
			0, 4, 2, 
			2, 4, 6,
		};
		
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.tangents = Solve(mesh);
		return mesh;
	}
}
