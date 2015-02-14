using UnityEngine;
using System.Collections;

public class pushnpc : MonoBehaviour {

	public cursor_handle cursor;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = cursor.cursor3d;
	}
}
