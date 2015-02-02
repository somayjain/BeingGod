using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public int currentLevel;
	public GameObject levelObject;

	// Use this for initialization
	void Start () {
		currentLevel = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentLevel == 0 && Input.GetKeyUp("z")) {
			levelObject = (GameObject)Instantiate (Resources.Load ("level0"), gameObject.transform.position, Quaternion.identity) as GameObject;
			levelObject.transform.parent = transform;
			GameObject.FindGameObjectWithTag("Hand").GetComponent<cursor_handle>().currentLevel = levelObject;
			currentLevel++;
		}
	}
}
