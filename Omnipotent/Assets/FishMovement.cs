using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour {

	public GameObject[] fishes;
	public Vector3[] WPLoc;

	float resetTimer = 20.0f;
	float fSpeed = 3.0f;
	Vector3 dir;

	// Use this for initialization
	void Start () {
		dir = WPLoc [0];
	}
	
	// Update is called once per frame
	void Update () {
		resetTimer -= Time.fixedDeltaTime;
		if (resetTimer <= 0.0f) {
			dir = WPLoc[Random.Range(0,WPLoc.Length)];
			resetTimer = 20.0f;
			//Debug.Log("resetting dir");
		}
		float step = fSpeed * Time.fixedDeltaTime;
		for (int i=0; i<fishes.Length; i++) {
				fishes[i].transform.position = Vector3.MoveTowards(fishes[i].transform.position,dir,step);
				Quaternion RotationWanted = Quaternion.LookRotation(-1.0f*dir);
				fishes[i].transform.rotation = Quaternion.RotateTowards(fishes[i].transform.rotation, RotationWanted, 5.0f*Time.fixedDeltaTime);

		}
	}
}
