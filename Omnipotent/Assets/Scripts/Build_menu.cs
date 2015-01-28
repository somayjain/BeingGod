using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Build_menu : MonoBehaviour {
	
	public cursor_handle cursor;
	
	public GameObject Shelf;
	public GameObject Menu;
	public GameObject Buildings;
	
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
		Shelf.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Shelf.GetComponent<RectTransform> ().sizeDelta.x, 600);
		Menu.SetActive (false);
		Buildings.SetActive (true);
	}
}
