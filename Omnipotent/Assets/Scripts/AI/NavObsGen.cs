using UnityEngine;
using System.Collections;

public class NavObsGen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("o")) {
			Vector3 loc = new Vector3(Random.Range(-5f,5f),0.5f,Random.Range(-5f,5f));
			GameObject obs = (GameObject)Instantiate(Resources.Load("Block_NavObs"),loc,Quaternion.identity) as GameObject;
		}
	}
}
