using UnityEngine;
using System.Collections;



public class modeDetect : MonoBehaviour {
	public bool editMountainFlag;
	public bool editForestFlag;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		editMountainFlag = Input.GetKeyDown (KeyCode.M); // Checking for Mountain mode
		editForestFlag = Input.GetKeyDown (KeyCode.F); // Checking for Forest mode
		if(editMountainFlag){  

		}
		if(editForestFlag){

		}


	}
}
