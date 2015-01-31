﻿using UnityEngine;
using System.Collections.Generic;

public class LoadVoxelPeople : MonoBehaviour {
	public enum MODE {
		DEFAULT,
		BUILD,
		THUNDER_CLAP,
		WINDY,
		GMBC,
		MJOLNIR,
		FIREBALL,
		TORNADO
	}
	public MODE Powermode = MODE.DEFAULT;
	public Vector3 pointOfContact;

	GameObject personManager;
	public List<Vector3> sources = new List<Vector3>();
	public int nos_sources=4; 
	public List<GameObject> people = new List<GameObject>();
	public List<float> people_last_check = new List<float>();
	List<string> prefabNameList = new List<string>(new string[] { "chr_priest", "chr_hunter1", "chr_lady3","chr_mike","chr_bro" });
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
		int rand_chr = Random.Range(0,prefabNameList.Count);
		GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/"+prefabNameList[rand_chr]), sourceLoc,Quaternion.AngleAxis(270,Vector3.right)) as GameObject;
		obs.name = prefabNameList[rand_chr];
		obs.transform.parent = transform;
		NavMeshAgent agent = (NavMeshAgent)obs.AddComponent("NavMeshAgent");
		NavAgentMovement agentMovement =  obs.AddComponent<NavAgentMovement> ();
		agentMovement.target = targetLoc;
		obs.transform.FindChild (obs.name).transform.Rotate (Vector3.forward, 180);
		personManager.GetComponent<PeopleStats> ().addPeople ();
		people.Add (obs);
	}



	void checkCrowd(){
		int people_nos = people.Count;
		for (int i=0; i<people_nos; i++) {
			int rand_pos = Random.Range(0,nos_sources);
			NavAgentMovement person_script = people[i].GetComponent<NavAgentMovement>();
			//	Debug.Log(person_script.lastTimeUpdate+" ");
			if(person_script.targetReached){
				person_script.setNewPath(sources[rand_pos]);
				Debug.Log("Recompute the new path");
			}
			switch(Powermode){
			case MODE.THUNDER_CLAP:
				person_script.toggleScaredRun(true);
				person_script.currentlyScared = true;
				Debug.Log("Thunder Clap at "+pointOfContact+" ");
				break;
			case MODE.FIREBALL:
				Debug.Log("Fireball at "+pointOfContact+" ");
				break;
			case MODE.MJOLNIR:
				Debug.Log("MJOLNIR at "+pointOfContact+" ");
				break;
			case MODE.TORNADO:
				Debug.Log("Tornado at "+pointOfContact+" ");
				break;
			}
		}
		Powermode = MODE.DEFAULT;
	}

	// Update is called once per frame
	void Update () {
		checkCrowd ();

		if (Input.GetKeyUp ("n")) {
					initPerson ();
		    }
		if (Input.GetKeyUp ("o")) {
			//Vector3 loc = new Vector3(Random.Range(-5f,5f),0.5f,Random.Range(-5f,5f));
			//GameObject obs = (GameObject)Instantiate(Resources.Load("Block_NavObs"),loc,Quaternion.identity) as GameObject;
		}
		}
}
