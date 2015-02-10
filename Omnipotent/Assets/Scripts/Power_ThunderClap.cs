using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_ThunderClap : Powers {

//	public cursor_handle cursor;
	public GameObject ThunderClap;

//	public float time = 8.0f;
//	private float time_left = 0;
//	private bool active = false;

	// Use this for initialization
	void Start () {
		enabled = true;
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
			ThunderClap.SetActive (active);
		}
		
		if (time_left <= 0.0f) {
			time_left = Cooldown;
			refresh = true;
		}
		
		UpdateLast ();
	}

	public void Trigger (Vector3 loc) {
		if ( !enabled )	return;
		
		if (time_left <= 0.0f) {
			time_left = Cooldown;
			refresh = true;
		}
		
		if ( refresh ) {
			Levelcontroller.Powermode = LevelController.MODE.THUNDER_CLAP;
			Levelcontroller.PowerLoc = loc;

			refresh = false;
			active = true;
			time_left = Cooldown;

			location = loc;

			ThunderClap.SetActive (active);
		}
	}

	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.THUNDER_CLAP);
	}
}