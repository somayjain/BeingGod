using UnityEngine;
using System.Collections;


public class fireball : MonoBehaviour {
	public bool gravity = false;
	AudioSource audio;
	// Use this for initialization
	void Start () {
		//myPlane.transform.position;
		rigidbody.useGravity = gravity;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("f"))
		{	gravity = !gravity;
			rigidbody.useGravity = gravity;
		}
	}
}
