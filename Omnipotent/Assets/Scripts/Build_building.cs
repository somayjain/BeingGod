using UnityEngine;
using System.Collections;

public class Build_building : MonoBehaviour {

	public cursor_handle cursor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseOver () {
	}
	public void OnMouseExit () {
	}
	public void OnMouseClick () {
		/* Create Object */
		cursor.Build (1);
	}
}
