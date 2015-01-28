using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Tornado : MonoBehaviour {
	
	public cursor_handle cursor;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == cursor_handle.MODE.DEFAULT)
			GetComponent<Button> ().interactable = true;
	}
	
	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.TORNADO);
	}
}
