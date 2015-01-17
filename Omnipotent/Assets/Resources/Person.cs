using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	/*Must have attributes of a person*/
	int Faith; 
	int Fear;
	public Vector3 sourceID; // ID of some sort where he'll go and work during the day and back to home during night.
	public Vector3 destID;  //
	private int ssID;

	/* For NPC's dynamic nature*/
	int converse; // willingness to stop and interact with other people if he meets any.
	char societyLevel; // 'p'riest ? worker/'c'ommoner ? 'w'anderer ? 'a'theist(chances of dying are large)   

	// Use this for initialization
	void Start () {
		//AstarPath.active.UpdateGraphs(gameObject.collider.bounds);
		Faith = 50;
		Fear = 50;
	}	

	// Update is called once per frame
	void Update () {
	if (sourceID != null) {
			Debug.Log("I have a home!");
	}
	}
}
