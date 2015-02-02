using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Fireball : MonoBehaviour {
	
	public cursor_handle cursor;
	public GameObject Fireball;
	
	public float time = 10.0f;
	private float time_left = 0.0f;
	private bool active = false;
	private bool emit = false;

	public float Fireball_height = 30.0f;

	public cursor_handle.MODE cursor_mode = cursor_handle.MODE.FIREBALL;
	
	// Use this for initialization
	void Start () {
		time_left = time;
	}
	
	// Update is called once per frame
	void Update () {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == cursor_handle.MODE.DEFAULT)
			GetComponent<Button> ().interactable = true;
		//Debug.Log(time_left.ToString());
		if (time_left <= 0.0f) {
			time_left = time;
			active = false;
//			DestroyObject(Fireball.transform.GetChild(0).gameObject);// Explosion);
			Fireball.SetActive (active);
		} else if (active)
			time_left -= Time.deltaTime;
	}
	
	public void Trigger (Vector3 loc) {
		if (time_left <= 0.0f) {
			time_left = time;
		} else if (!active) {
			active = true;
			time_left = time;

			Fireball.transform.position = loc;
			Fireball.SetActive (active);

			GameObject trail = (GameObject)Instantiate(Resources.Load("FireBall"), loc + new Vector3(0,Fireball_height,0), Quaternion.identity) as GameObject;
			trail.name = "Trail";
			trail.transform.parent = Fireball.transform;
			trail.SetActive (true);
		}
	}
	
	public void OnClick () {
		cursor.setMode (cursor_mode);
	}
}
