using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Tornado : Powers {
	
//	public cursor_handle cursor;
	public GameObject Tornado;
	
//	public float time = 25.0f;
//	private float time_left = 0;
//	private bool active = false;
	private bool emit = false;

	// Use this for initialization
	void Start () {
		enabled = true;
		time_left = Cooldown;
		refresh = true;

		Tornado.transform.GetChild(0).gameObject.SetActive (active);
		foreach (Transform child in Tornado.transform.GetChild(0).transform)
			child.gameObject.SetActive (active);
		ParticleEmitter[] twister_emitters = Tornado.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter emitter in twister_emitters)
			emitter.emit = active;
	}
	
	// Update is called once per frame
	void Update () {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == cursor_handle.MODE.DEFAULT)
			GetComponent<Button> ().interactable = true;

//		Debug.Log (time_left.ToString ());

		if ( !enabled )	return;

		if (active) {
			if (active && time_left <= Cooldown - CastTime) {
				active = false;
				Tornado.SetActive (active);
				Tornado.transform.GetChild (0).localPosition = Vector3.zero;
				Tornado.transform.GetChild (0).gameObject.SetActive (active);
				foreach (Transform child in Tornado.transform.GetChild(0).transform)
					child.gameObject.SetActive (active);
			} else if (time_left <= Cooldown - CastTime + 8.0f && emit) {
				emit = false;
				ParticleEmitter[] twister_emitters = Tornado.GetComponentsInChildren<ParticleEmitter> ();
				foreach (ParticleEmitter emitter in twister_emitters)
					emitter.emit = emit;
			}
		
			location = Tornado.transform.GetChild (0).position;
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
			refresh = false;
			active = true;
			emit = true;
			time_left = Cooldown;

			location = loc;

			Tornado.transform.position = loc;
			Tornado.SetActive (active);
			Tornado.transform.GetChild(0).gameObject.SetActive (active);
			foreach (Transform child in Tornado.transform.GetChild(0).transform)
				child.gameObject.SetActive (active);

			ParticleEmitter[] twister_emitters = Tornado.GetComponentsInChildren<ParticleEmitter>();
			foreach (ParticleEmitter emitter in twister_emitters)
				emitter.emit = emit;
		}
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.TORNADO);
	}
}
