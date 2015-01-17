using UnityEngine;
using System.Collections;

public class LoadVoxelPeople : MonoBehaviour {

	GameObject personManager;
	// Use this for initialization
	void Start () {
		personManager = new GameObject ();
		personManager.name = "personManager";
		PeopleStats ps = personManager.AddComponent<PeopleStats> ();
	}

	void initPerson(){
		GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/chr_priest"), new Vector3 (0, 0, 0),Quaternion.AngleAxis(270,Vector3.right)) as GameObject;
		obs.name = "Priest";
		NavMeshAgent agent = (NavMeshAgent)obs.AddComponent("NavMeshAgent");
		//agent.enabled = true;
		NavAgentMovement agentMovement =  obs.AddComponent<NavAgentMovement> ();
		obs.transform.FindChild ("chr_priest").transform.Rotate (Vector3.forward, 180);
		personManager.GetComponent<PeopleStats> ().addPeople ();
	}

	// Update is called once per frame
	void Update () {
				if (Input.GetKeyUp ("n")) {
					initPerson ();
		    }
		if (Input.GetKeyUp ("o")) {
			//Vector3 loc = new Vector3(Random.Range(-5f,5f),0.5f,Random.Range(-5f,5f));
			//GameObject obs = (GameObject)Instantiate(Resources.Load("Block_NavObs"),loc,Quaternion.identity) as GameObject;
		}
		}
}
