using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratePeople : MonoBehaviour {

	public GameObject[] PersonInWorld = new GameObject[100];
	int live_CountOfPerson = 0;
	int dead_CountOfPerson = 0;

	public List<Vector3> homeLocations = new List<Vector3>();	

	// Use this for initialization
	void Start () {
		GameObject[]homeGroup;
		homeGroup = GameObject.FindGameObjectsWithTag("Home");
		foreach (GameObject home in homeGroup) {
			homeLocations.Add(home.transform.position);

		}
	}

	void FixedUpdate(){
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp ("o")) {
			Vector3 loc = new Vector3(Random.Range(-10,10),0.5f,Random.Range(-10,10));
			GameObject obs = (GameObject)Instantiate(Resources.Load("Obstacle"),loc,Quaternion.identity) as GameObject;
			obs.GetComponent<ObstacleMovement>().targetLoc = new Vector3(0,.5f,0);
			obs.GetComponent<ObstacleMovement>().homeLoc = obs.transform.position;

//			for(int i=dead_CountOfPerson; i< live_CountOfPerson;i++){
//				PersonInWorld[i].GetComponent<AI_Logic>().getNewPath();
//			}
		}

		if (Input.GetMouseButtonUp (0)) {
			Debug.Log(" "+homeLocations.Capacity);
			int value = Random.Range(0,homeLocations.Capacity-1);
			GameObject personInstance = (GameObject)Instantiate(Resources.Load("People"),homeLocations[value],Quaternion.identity);
			personInstance.GetComponent<Person>().sourceID = homeLocations[value];
			PersonInWorld[live_CountOfPerson] = personInstance;
			live_CountOfPerson++;
		}
		if (Input.GetMouseButtonUp (1) && dead_CountOfPerson<live_CountOfPerson) {
			Destroy(PersonInWorld[dead_CountOfPerson]);
			dead_CountOfPerson++;
		}
		if (dead_CountOfPerson == live_CountOfPerson) {
			dead_CountOfPerson = 0;
			live_CountOfPerson = 0;
		}
	}
}
