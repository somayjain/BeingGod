using UnityEngine;
using System.Collections;

public class camScript : MonoBehaviour {
	public Vector3 camOffset;
	// Use this for initialization
	void Start () {
		camOffset = new Vector3 (-5, 5, -5);
	}
	
	// Update is called once per frame
	void Update () {
//		transform.position = new Vector3 (Input.mousePosition.x + camOffset.x,
//		                                  Input.mousePosition.y + camOffset.y,
//		                                  Input.mousePosition.z + camOffset.z);
		transform.position = new Vector3 (Input.mousePosition.x,
		                                  Input.mousePosition.y,
		                                  Input.mousePosition.z);
	}
}
