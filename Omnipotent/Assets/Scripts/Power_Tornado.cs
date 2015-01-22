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
		if (cursor.mode == 1)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == 0)
			GetComponent<Button> ().interactable = true;
	}
	
	public void OnClick () {
		cursor.setMode (7);
	}
}
