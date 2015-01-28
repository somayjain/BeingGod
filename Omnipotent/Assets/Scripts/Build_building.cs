using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Build_building : MonoBehaviour {

	public cursor_handle cursor;
	public cursor_handle.BUILD build_mode = cursor_handle.BUILD.NONE;
	
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
	public void OnMouseDown () {
		/* Create Object */
		cursor.Build (build_mode);
	}
	public void OnMouseUp() {
		cursor.Build (cursor_handle.BUILD.NONE);
	}
}
