using UnityEngine;
using System.Collections;

public class ObstacleMovement : MonoBehaviour {

	public Vector3 homeLoc;
	public Vector3 targetLoc;

	bool toggle = false;

	CharacterController controller;

	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();
	}

	void FixedUpdate(){
		if (targetLoc == null || homeLoc == null) {
			return;
		}
		Vector3 dir;
		if(toggle)
		dir = (homeLoc-targetLoc).normalized * 1000 * Time.fixedDeltaTime;
		else
		dir = (targetLoc-homeLoc).normalized * 1000 * Time.fixedDeltaTime;

		float mag = (targetLoc - homeLoc).magnitude;



		//controller.SimpleMove(dir);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
