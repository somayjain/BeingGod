using UnityEngine;
using System.Collections;

public class testing : MonoBehaviour {
	public GameObject x;
	public bool status = false;
	// Use this for initialization
	void Start () {
		Debug.Log ("testing initiated");
		x = GameObject.FindWithTag ("Explosion");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp("l")) {
			Debug.Log ("detected L");
			status = !status;
			x.SetActive(status); 
		}
	}
}
