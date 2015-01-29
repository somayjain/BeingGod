using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD_handle : MonoBehaviour {
	
	public cursor_handle cursor;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnMouseOver () {
		cursor.OnHUD ();
	}
	
	public void OnMouseExit () {
		cursor.OffHUD ();
	}
}