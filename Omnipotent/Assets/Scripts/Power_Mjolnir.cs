using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Mjolnir : Powers {

	public GameObject Mjolnir;

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
		}
	}

	public void AddXP (int num_of_people, int flag) {
		if (flag > 0)
			XP.AddXP (num_of_people * XP_per_NPC, POWERTYPE.GOOD);
		else
			XP.AddXP (num_of_people * XP_per_NPC, POWERTYPE.EVIL);
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.MJOLNIR);
	}
}
