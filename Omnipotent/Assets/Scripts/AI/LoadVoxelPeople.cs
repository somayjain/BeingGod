using UnityEngine;
using System.Collections;
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

	//Fire
	float fireTimer = 6.0f;
	bool firemode = false;
	Vector3 fireLoc = new Vector3();
	float hoverTextTimer = 5.0f;

	Vector3 newHouseLoc = new Vector3();

	public void updateSources(){
		sourceList = GameObject.FindGameObjectsWithTag ("source");
		nos_sources = sourceList.Length;
		int index = 0;
		sources.Clear ();
		foreach (GameObject gob in sourceList) {
			if(gob.name.ToString().CompareTo("source_"+(nos_sources-1).ToString())==0)
				newHouseLoc = gob.transform.position;
			Vector3 pos = gob.transform.position;
//			sources.Add(gob.transform.position);
			sources.Add(pos);
			Debug.Log(sources[index]+" House at "+index+" "+gob.transform.position);
			index++;
		}
	}

	// Use this for initialization
	void Start () {
		//personManager = new GameObject ();
		//personManager.name = "personManager";
		//PeopleStats ps = personManager.AddComponent<PeopleStats> ();
		// comment



	}

	private float secs = 1.0f;
	public void initPerson(int nosPeople){
		updateSources ();
		secs = 1.0f;

		for (int i=0; i<nosPeople; i++) {
	secs++;
			StartCoroutine(createPPL());
//						int rand_indx = Random.Range (0, nos_sources);
//			            Vector3 sourceLoc = newHouseLoc;
//						Vector3 targetLoc = sources [rand_indx];
//						int rand_chr = Random.Range (0, prefabNameList.Count);
//						GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/" + prefabNameList [rand_chr]), sourceLoc, Quaternion.AngleAxis (270, Vector3.right)) as GameObject;
//						obs.name = prefabNameList [rand_chr];
//						obs.transform.parent = transform;
//						NavMeshAgent agent = (NavMeshAgent)obs.AddComponent ("NavMeshAgent");
//						NavAgentMovement agentMovement = obs.AddComponent<NavAgentMovement> ();
//						agentMovement.target = targetLoc;
//						obs.transform.GetChild(0).gameObject.SetActive(false);
//						//obs.transform.GetComponent<Rigidbody> ().detectCollisions = false;
//						obs.transform.FindChild (obs.name).transform.Rotate (Vector3.forward, 180);
//						people.Add (obs);
				}
	}

	IEnumerator createPPL()
	{
		yield return new WaitForSeconds(secs);
		int rand_indx = Random.Range (0, nos_sources);
	//	Debug.Log("Rnd: " + rand_indx.ToString() + "," + nos_sources.ToString());
		Vector3 sourceLoc = newHouseLoc;
		Vector3 targetLoc = sources [rand_indx];
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

	void checkCrowd(){
		//Debug.Log (tornadoTime+" ");

		if (Powermode == MODE.TORNADO)
						tornadoOn = true;

		if (Powermode == MODE.FIREBALL || firemode == true) {
			if(!firemode)
				fireLoc = GetComponentInParent<LevelController>().PowerLoc;
			firemode = true;
			if(fireTimer<-3.0f){
				fireTimer = 6.0f;
				firemode = false;
			}else{
				fireTimer -= Time.fixedDeltaTime;
			}
		}
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

			//people[i].transform.GetChild(0);

			if(fireTimer<=2.0f && fireTimer > -3.0f && (people[i].transform.position - fireLoc).magnitude<=15.0f && people[i].transform.childCount<3){
				GameObject head_fire = (GameObject)Instantiate (Resources.Load ("human_fire"),Vector3.zero,	Quaternion.identity) as GameObject;
				head_fire.transform.parent = people[i].transform;
				head_fire.transform.localPosition = new Vector3(0,0.7f,0);
				person_script.toggleScaredRun(true);
				//Debug.Log(people[i].name+" on fire");
			}
		    if(fireTimer<=-3.0f && (people[i].transform.childCount>2)){
				GameObject fireObj = people[i].transform.GetChild(2).gameObject;
				Destroy(fireObj);
				//Debug.Log(people[i].name + " off fire");
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
				//Random random = new Random();
				//random.seed = i;
				//updateSources();
				//Debug.Log("Rnd: " + rand_pos.ToString() + "," + nos_sources.ToString()+" "+sources[rand_pos]);
				person_script.setNewPath(sources[rand_pos]);
				//Debug.Log("Recompute the new path");
			}
		}
		Powermode = MODE.DEFAULT;
	}

	// Update is called once per frame
	void Update () {

		Debug.Log("HUMAN MANAGER");
		checkCrowd ();

		}
}
