using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadVoxelPeople : MonoBehaviour {


	GameObject personManager;
	public List<Vector3> sources = new List<Vector3>();
	public int nos_sources; 
	public List<GameObject> people = new List<GameObject>();
	public List<float> people_last_check = new List<float>();
	List<string> prefabNameList = new List<string>(new string[] { "chr_priest", "chr_hunter1", "chr_lady3","chr_mike","chr_bro" });
	List<string> GMBCString = new List<string> (new string[]{"Nice to see the weather change!","Thank you god!","It's a god's blessing!"});
	private GameObject[] sourceList;


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
	int chosenOne = new int();
	bool chosenMode = false;

	public Vector3 tornadoLoc = new Vector3();
	float tornadoTime = 25.0f;
	bool tornadoOn = false;
	List<int>tornadoHit = new List<int>();
	float tornadoRange = 18.0f;

	float hoverTextTimer = 5.0f;

	public void updateSources(){
		sourceList = GameObject.FindGameObjectsWithTag ("source");
		nos_sources = sourceList.Length;
		foreach (GameObject gob in sourceList) {
			sources.Add(gob.transform.position);
		}
	}

	// Use this for initialization
	void Start () {
		//personManager = new GameObject ();
		//personManager.name = "personManager";
		//PeopleStats ps = personManager.AddComponent<PeopleStats> ();
		// comment



	}

	public void initPerson(int nosPeople){
		updateSources ();

		for (int i=0; i<nosPeople; i++) {
						int rand_indx = Random.Range (0, nos_sources);
						Vector3 sourceLoc = sources [rand_indx];
						Vector3 targetLoc = sources [nos_sources - rand_indx - 1];
						int rand_chr = Random.Range (0, prefabNameList.Count);
						GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/" + prefabNameList [rand_chr]), sourceLoc, Quaternion.AngleAxis (270, Vector3.right)) as GameObject;
						obs.name = prefabNameList [rand_chr];
						obs.transform.parent = transform;
						NavMeshAgent agent = (NavMeshAgent)obs.AddComponent ("NavMeshAgent");
						NavAgentMovement agentMovement = obs.AddComponent<NavAgentMovement> ();
						agentMovement.target = targetLoc;
						obs.transform.GetChild(0).gameObject.SetActive(false);
						//obs.transform.GetComponent<Rigidbody> ().detectCollisions = false;
						obs.transform.FindChild (obs.name).transform.Rotate (Vector3.forward, 180);
						people.Add (obs);
				}
	}



	void checkCrowd(){
		//Debug.Log (tornadoTime+" ");

		if (Powermode == MODE.TORNADO)
						tornadoOn = true;

		if(tornadoOn == true && tornadoTime <= 0.0f){
			tornadoOn = false;
			tornadoTime = 25.0f;
		}else{
			if(tornadoOn == true){
				tornadoTime -= Time.fixedDeltaTime;
				tornadoLoc = GetComponentInParent<LevelController>().TornadoLoc;
			}
		}

		if (Powermode == MODE.GMBC || chosenMode == true) {
			if(hoverTextTimer<=0.0f){
				chosenMode = false;
				hoverTextTimer = 5.0f;
			}else{
				if(chosenMode == false){
					chosenMode = true;
					chosenOne = Random.Range(0,people.Count);
				}
				hoverTextTimer -= Time.fixedDeltaTime;
			}
		}

		int people_nos = people.Count;
		for (int i=0; i<people_nos; i++) {
			int rand_pos = Random.Range(0,nos_sources);
			NavAgentMovement person_script = people[i].GetComponent<NavAgentMovement>();
			//	Debug.Log(person_script.lastTimeUpdate+" ");
			if(person_script.health <= 0.0f){
				Vector3 spawnLoc = people[i].transform.position;
				Destroy(people[i]);
				GameObject.FindGameObjectWithTag("ZombieManager").GetComponent<ZombieManager>().addZombie(spawnLoc);
				people.RemoveAt(i);
				people_nos--;
				i--;
				continue;
			}



			if(tornadoOn == true){
				tornadoLoc.y = 0.0f;
				if(tornadoTime <= 0.0f)
					people[i].GetComponent<NavAgentMovement>().haltMovement(false);
				else{
					if((people[i].transform.position-tornadoLoc).magnitude <= tornadoRange){
						people[i].GetComponent<NavAgentMovement>().haltMovement(true);
					}else{
						if((people[i].transform.position-tornadoLoc).magnitude > tornadoRange){
							people[i].GetComponent<NavAgentMovement>().haltMovement(false);
						}
					}
				}
			}

			if(chosenMode == true){
				if(chosenOne == i){
					people[i].transform.GetChild(0).gameObject.SetActive(true);
					people[i].GetComponentInChildren<Text>().text = GMBCString[Random.Range(0,GMBCString.Count)];
				}
			}else{
				//Debug.Log (people[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
				people[i].transform.GetChild(0).gameObject.SetActive(false);
			}

			switch(Powermode){
			case MODE.THUNDER_CLAP:
				person_script.toggleScaredRun(true);
				person_script.currentlyScared = true;
				Debug.Log("Thunder Clap at "+pointOfContact+" ");
				break;
			}

			if(person_script.targetReached){
				person_script.setNewPath(sources[rand_pos]);
				//Debug.Log("Recompute the new path");
			}
		}
		Powermode = MODE.DEFAULT;
	}

	// Update is called once per frame
	void Update () {
		checkCrowd ();

			
		if (Input.GetKeyUp ("o")) {
			//Vector3 loc = new Vector3(Random.Range(-5f,5f),0.5f,Random.Range(-5f,5f));
			//GameObject obs = (GameObject)Instantiate(Resources.Load("Block_NavObs"),loc,Quaternion.identity) as GameObject;
		}
		}
}
