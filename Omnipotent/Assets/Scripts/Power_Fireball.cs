using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Fireball : Powers {
	
	public GameObject Fireball;
	public float Fireball_height = 30.0f;
	private bool emit = false;
	
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
		//Debug.Log(time_left.ToString());

		if ( !enabled )	return;

		if (active && time_left <= Cooldown - CastTime) {
			active = false;
			Fireball.SetActive (active);
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
			Levelcontroller.Powermode = LevelController.MODE.FIREBALL;
			Levelcontroller.PowerLoc = loc;

			refresh = false;
			active = true;
			time_left = Cooldown;

			location = loc;

			Fireball.transform.position = loc;
			Fireball.SetActive (active);

//			GameObject fireball = (GameObject)Instantiate(Resources.Load("FireBALL_NEW"), loc + new Vector3(0,90f,0), Quaternion.identity) as GameObject;
//			fireball.name = "fireball";
//			fireball.transform.parent = Fireball.transform;
//			fireball.transform.rotation = Quaternion.Euler(90f,-180f,0);
//			fireball.SetActive (true);
			GameObject trail = (GameObject)Instantiate(Resources.Load("FireBall"), loc + new Vector3(0,Fireball_height,0), Quaternion.identity) as GameObject;
			trail.name = "Trail";
			trail.transform.parent = Fireball.transform;
			trail.SetActive (true);
		}
	}

	public void AddXP (int num_of_people, int flag) {
		if (flag > 0)
			XP.AddXP (num_of_people * XP_per_NPC, POWERTYPE.GOOD);
		else
			XP.AddXP (num_of_people * XP_per_NPC, POWERTYPE.EVIL);
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.FIREBALL);
	}
}
