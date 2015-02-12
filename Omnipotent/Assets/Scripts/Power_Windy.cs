using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Windy : Powers {

	public GameObject Windy;

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
			Windy.SetActive (active);
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
			refresh = false;
			active = true;
			time_left = Cooldown;
			
			location = loc;
			
			Windy.transform.position = loc;
			Windy.SetActive (active);
		}
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.WINDY);
	}
}
