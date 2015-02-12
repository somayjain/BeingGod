using UnityEngine;
using System.Collections;

public class Power_HoG1 : MonoBehaviour {
	public GameObject faceMesh1;
	public GameObject faceMesh2;
	public GameObject faceMesh3;
	public GameObject faceMesh4;
	bool flag=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("h") && !flag)
		{	flag=true;
			GameObject trapezoid = (GameObject)Instantiate(Resources.Load("Trapezoid"), new Vector3(0,0,0), Quaternion.identity) as GameObject;
			//add _mesh to this and scale it
			faceMesh1.transform.position = new Vector3(0.0f,1.8f,-3.4f);
			faceMesh1.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
			faceMesh1.transform.localRotation = Quaternion.Euler(-12,180,0);
			faceMesh1.transform.parent = trapezoid.transform;
			faceMesh2.transform.position = new Vector3(0.0f,1.8f,3.5f);
			faceMesh2.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
			faceMesh2.transform.localRotation = Quaternion.Euler(-10,0,0);
			faceMesh2.transform.parent = trapezoid.transform;
			faceMesh3.transform.position = new Vector3(-3.0f,1.8f,0.0f);
			faceMesh3.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
			faceMesh3.transform.localRotation = Quaternion.Euler(-10,270,0);
			faceMesh3.transform.parent = trapezoid.transform;
			faceMesh4.transform.position = new Vector3(3.0f,1.8f,0.0f);
			faceMesh4.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
			faceMesh4.transform.localRotation = Quaternion.Euler(-10,90,0);
			faceMesh4.transform.parent = trapezoid.transform;
		}
	}
}
