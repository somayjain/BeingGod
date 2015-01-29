using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Tornado : MonoBehaviour {
	
	public cursor_handle cursor;
	public GameObject Tornado;
	
	public float time = 3.0f;
	private float time_left;
	private bool active = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == cursor_handle.MODE.DEFAULT)
			GetComponent<Button> ().interactable = true;

		if (time_left <= 0.0f) {
			time_left = time;
			active = false;
			Tornado.SetActive (active);
		}
		else if (active)
			time_left -= Time.deltaTime;
	}
	
	public void Trigger (Vector3 loc) {
		if (time_left <= 0.0f) {
			time_left = time;
		} else if (!active) {
			active = true;
			time_left = time;
			Tornado.transform.position = loc;
			Tornado.SetActive (active);
		}
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.TORNADO);
	}
}
