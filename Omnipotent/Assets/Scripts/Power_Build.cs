using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Build : MonoBehaviour {

	public GameObject shelves;
	public cursor_handle cursor;

	private bool buildmode = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick () {
		buildmode = !buildmode;
		shelves.SetActive(buildmode);

		if (buildmode) {
			cursor.setMode(1);
//			this.GetComponent<Button> ().colors.normalColor = Color.white;
		} else {
			cursor.setMode(0);
//			this.GetComponent<Button> ().colors.normalColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
		}
	}
}
