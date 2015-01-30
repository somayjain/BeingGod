using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Tornado : MonoBehaviour {
	
	public cursor_handle cursor;
	public GameObject Tornado;
	
	public float time = 25.0f;
	private float time_left = 0;
	private bool active = false;
	private bool emit = false;

	// Use this for initialization
	void Start () {
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
		if (time_left <= 0.0f) {
			time_left = time;
			active = false;
			Tornado.SetActive (active);
			Tornado.transform.GetChild(0).position = Vector3.zero;
			Tornado.transform.GetChild(0).gameObject.SetActive (active);
			foreach (Transform child in Tornado.transform.GetChild(0).transform)
				child.gameObject.SetActive (active);
		} else if (time_left <= 8.0f && emit) {
			emit = false;
			ParticleEmitter[] twister_emitters = Tornado.GetComponentsInChildren<ParticleEmitter>();
			foreach (ParticleEmitter emitter in twister_emitters)
				emitter.emit = emit;
			time_left -= Time.deltaTime;
		} else if (active)
			time_left -= Time.deltaTime;
	}
	
	public void Trigger (Vector3 loc) {
		if (time_left <= 0.0f) {
			time_left = time;
		} else if (!active) {
			active = true;
			emit = true;
			time_left = time;
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
