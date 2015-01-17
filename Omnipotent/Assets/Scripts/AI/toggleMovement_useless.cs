using UnityEngine;
using System.Collections;

public class toggleMovement_useless : MonoBehaviour {
	Vector3 start;
	bool toTarget=false;
	Vector3 end;
	CharacterController controller;
	// Use this for initialization
	void Start () {
		start = transform.position;
		end = start;
		end.x = -1* end.x;
		controller = gameObject.GetComponent<CharacterController>();
		Debug.Log (start + " "+end);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x>=end.x || transform.position.x <= start.x) {
			toTarget=!toTarget;
		}
	    Vector3 dir;
		if(toTarget)
			dir =  new Vector3(1,0,0)*100 * Time.fixedDeltaTime;
		else
			dir = new Vector3(-1,0,0)*100 * Time.fixedDeltaTime;

		controller.SimpleMove(dir);
	}
}
