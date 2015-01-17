using UnityEngine;
using System.Collections;

public class sphereScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (Input.mousePosition.x, 
		                                  Input.mousePosition.y,
		                                  Input.mousePosition.z);
	}
}
