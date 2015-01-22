using UnityEngine;
using System.Collections.Generic;

public class LoadVoxelPeople : MonoBehaviour {


	GameObject personManager;
	public List<Vector3> sources = new List<Vector3>();
	public int nos_sources=4; 
	public List<GameObject> people = new List<GameObject>();
	public List<float> people_last_check = new List<float>();
	// Use this for initialization
	void Start () {
		personManager = new GameObject ();
		personManager.name = "personManager";
		PeopleStats ps = personManager.AddComponent<PeopleStats> ();
		// comment
		for (int i=0; i<nos_sources; i++) {
			GameObject obj = GameObject.FindGameObjectWithTag("source"+i);
			Vector3 objLoc = obj.transform.position;
			sources.Add(objLoc);
			Debug.Log(obj.name);
		}
	}

	void initPerson(){
		int rand_indx = Random.Range (0,nos_sources);
		Vector3 sourceLoc = sources [rand_indx];
		Vector3 targetLoc = sources [nos_sources-rand_indx-1];
		GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/chr_priest"), sourceLoc,Quaternion.AngleAxis(270,Vector3.right)) as GameObject;
		obs.name = "Priest";
		NavMeshAgent agent = (NavMeshAgent)obs.AddComponent("NavMeshAgent");
		NavAgentMovement agentMovement =  obs.AddComponent<NavAgentMovement> ();
		agentMovement.target = targetLoc;
		obs.transform.FindChild ("chr_priest").transform.Rotate (Vector3.forward, 180);
		personManager.GetComponent<PeopleStats> ().addPeople ();
		people.Add (obs);
	}

	void checkCrowdBlock(){
		int people_nos = people.Count;
		for (int i=0; i<people_nos; i++) {
			int rand_pos = Random.Range(0,nos_sources);
			NavAgentMovement person_script = people[i].GetComponent<NavAgentMovement>();
			float dist = (person_script.lastLocUpdate-person_script.agent.transform.position).magnitude;
			if(person_script.targetReached || (dist<=.5f && person_script.lastTimeUpdate >= 2.0f)){
				person_script.setNewPath(sources[rand_pos]);
				Debug.Log(dist+" "+person_script.lastTimeUpdate+" ");
				person_script.lastTimeUpdate = 0.0f;
				person_script.lastLocUpdate = sources[rand_pos];
			}else{
				if(person_script.lastTimeUpdate >= 2.0f && dist > 0.5f){
					Debug.Log(dist+" "+person_script.lastTimeUpdate+" ");
					person_script.lastTimeUpdate = 0.0f;
					person_script.lastLocUpdate = person_script.agent.transform.position;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {

		checkCrowdBlock ();

				if (Input.GetKeyUp ("n")) {
					initPerson ();
		    }
		if (Input.GetKeyUp ("o")) {
			//Vector3 loc = new Vector3(Random.Range(-5f,5f),0.5f,Random.Range(-5f,5f));
			//GameObject obs = (GameObject)Instantiate(Resources.Load("Block_NavObs"),loc,Quaternion.identity) as GameObject;
		}
		}
}
