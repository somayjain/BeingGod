using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_Build : MonoBehaviour {

	public houseManager Houses;
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
//		shelves.SetActive(buildmode);

		if (buildmode) {
			cursor.setMode(cursor_handle.MODE.BUILD);
//			this.GetComponent<Button> ().colors.normalColor = Color.white;
		} else {
			cursor.setMode(cursor_handle.MODE.DEFAULT);
//			this.GetComponent<Button> ().colors.normalColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
		}
	}

	public void StartBuildMode () {
		buildmode = true;
		cursor.setMode(cursor_handle.MODE.BUILD);
	}

	public void StopBuildMode () {
		buildmode = false;
		cursor.setMode (cursor_handle.MODE.DEFAULT);
		Houses.cancelBuild ();
	}

	public void BuildHouse ( ) {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			Houses.buildHouse ();
	}

}
