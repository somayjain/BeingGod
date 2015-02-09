using UnityEngine;
using System.Collections;

public class RotateDialogues : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		transform.rotation = Quaternion.LookRotation (Camera.main.transform.forward);
		GetComponent<RectTransform> ().rotation = Quaternion.LookRotation (Camera.main.transform.forward);
	
	}
}
