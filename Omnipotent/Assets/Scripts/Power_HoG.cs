using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_HoG : Powers {

	public GameObject HoG;
	public GameObject faceMesh1;
	public GameObject faceMesh2;
	public GameObject faceMesh3;
	public GameObject faceMesh4;
	bool flag=false;

	// Use this for initialization
	void Start () {
		time_left = Cooldown;
		refresh = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == cursor_handle.MODE.DEFAULT)
			GetComponent<Button> ().interactable = true;

		if ( !enabled )	return;

		if (active && time_left <= Cooldown - CastTime) {
			active = false;
		//	HoG.SetActive (active);
		}

		if (time_left <= 0.0f) {
			time_left = Cooldown;
			refresh = true;
		}

		UpdateLast ();
	}

	public void Trigger (Vector3 loc) {
		if (!enabled)	return;

		if (time_left <= 0.0f) {
			time_left = Cooldown;
			refresh = true;
		}

		if ( refresh ) {
//			Levelcontroller.Powermode = LevelController.MODE.MJOLNIR;
//			Levelcontroller.PowerLoc = loc;
			Debug.Log("Triggered");

			refresh = false;
			active = true;
			time_left = Cooldown;
			enabled = false;

			loc.y = 120.0f;
			location = loc;

			HoG.transform.position = loc;
			HoG.SetActive (active);
			HoG.GetComponent<Rigidbody>().useGravity = true;

			flag=true;
			//GameObject trapezoid = (GameObject)Instantiate(Resources.Load("Trapezoid"), new Vector3(0,0,0), Quaternion.identity) as GameObject;
			//trapezoid.transform.parent = 
			//add _mesh to this and scale it
//			faceMesh1.transform.parent = HoG.transform;
//			faceMesh1.transform.localPosition = new Vector3(0.0f,1.8f,-3.4f);
//			faceMesh1.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
//			faceMesh1.transform.localRotation = Quaternion.Euler(-12,180,0);
//
//			faceMesh2.transform.parent = HoG.transform;
//			faceMesh2.transform.localPosition = new Vector3(0.0f,1.8f,3.5f);
//			faceMesh2.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
//			faceMesh2.transform.localRotation = Quaternion.Euler(-10,0,0);
//			
//			faceMesh3.transform.parent = HoG.transform;
//			faceMesh3.transform.localPosition = new Vector3(-3.0f,1.8f,0.0f);
//			faceMesh3.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
//			faceMesh3.transform.localRotation = Quaternion.Euler(-10,270,0);
//
//			faceMesh4.transform.parent = HoG.transform;
//			faceMesh4.transform.localPosition = new Vector3(3.0f,1.8f,0.0f);
//			faceMesh4.transform.localScale = new Vector3(0.04f,0.05f,0.05f);
//			faceMesh4.transform.localRotation = Quaternion.Euler(-10,90,0);
			
//			XP.AddXP(XP_per_NPC, PowerType);
		}
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.HOG);
	}
}
