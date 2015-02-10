using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Mjolnir : Powers {

//	public enum POWERTYPE
//	{
//		GOOD, 
//		NEUTRAL, 
//		EVIL
//	};
//
//	public cursor_handle cursor;
	public GameObject Mjolnir;

//	public float Cooldown = 10.0f;
//	public float CastTime = 3.0f;
//	
//	private float time_left = 0.0f;
//	private bool refresh = false;
//	private bool active = false;
//
//	public POWERTYPE PowerType = POWERTYPE.NEUTRAL;
//	public float XP_per_NPC = 1.0f;
//
//	private bool enabled = false;

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
			Mjolnir.SetActive (active);
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
			Levelcontroller.Powermode = LevelController.MODE.MJOLNIR;
			Levelcontroller.PowerLoc = loc;

			refresh = false;
			active = true;
			time_left = Cooldown;

			location = loc;

			Mjolnir.transform.position = loc;
			Mjolnir.SetActive (active);

			XP.AddXP(XP_per_NPC, PowerType);
		}
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.MJOLNIR);
	}
}
