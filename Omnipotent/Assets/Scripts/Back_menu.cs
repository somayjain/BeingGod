using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Back_menu : MonoBehaviour {

	public GameObject Shelf;
	public GameObject Menu;
	public GameObject Current;

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
		Current.SetActive (false);
		Menu.SetActive (true);
		Shelf.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Shelf.GetComponent<RectTransform> ().sizeDelta.x, 400);
	}
}
