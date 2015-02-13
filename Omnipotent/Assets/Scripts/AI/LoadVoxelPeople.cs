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
	List<string> HeyString = new List<string> (new string[]{"Hmmmm?","Who Is There??","Someone Called Me?"});
	List<string> BooString = new List<string> (new string[]{"Please Don't Do That!","I'm Scared!!","I Should Hurry Home!"});
	private GameObject[] sourceList;

	Stack<Vector3>houseLoc = new Stack<Vector3>();

	public enum MODE {
		DEFAULT,
		BUILD,
		THUNDER_CLAP,
		WINDY,
		GMBC,
		MJOLNIR,
		FIREBALL,
		TORNADO,
		BOO,
		HEY
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

	float interactTimer = 5.0f;
	bool interactNPC = false;
	int NPC;

	public cursor_handle csHandle;

	public void updateSources(){
		sourceList = GameObject.FindGameObjectsWithTag ("source");
		nos_sources = sourceList.Length;
		int index = 0;
		sources.Clear ();
		foreach (GameObject gob in sourceList) {
			if(gob.name.ToString().CompareTo("source_"+(nos_sources-1).ToString())==0)
				houseLoc.Push(gob.transform.position);
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

	public void initPersonRandom(int nosPeople){
		float l_secs = 1.0f;
		
		Vector3 c_housePos = sources[Random.Range(0,sources.Count)];
		
		for (int i=0; i<nosPeople; i++) {
			l_secs++;
			StartCoroutine(createPPL(c_housePos, l_secs));
		}
	}

	public void initPerson(int nosPeople){
		float l_secs = 1.0f;

		Vector3 c_housePos = houseLoc.Pop ();

		for (int i=0; i<nosPeople; i++) {
	l_secs++;
			StartCoroutine(createPPL(c_housePos, l_secs));
				}
	}

	IEnumerator createPPL(Vector3 newSpawnPos, float l_secs)
	{
		yield return new WaitForSeconds(l_secs);
		int rand_indx = Random.Range (0, nos_sources);
	//	Debug.Log("Rnd: " + rand_indx.ToString() + "," + nos_sources.ToString());
		Vector3 sourceLoc = newSpawnPos;
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
			if(!firemode){
				fireLoc = GetComponentInParent<LevelController>().PowerLoc;
				fireLoc.y=0;
			}
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

		if (Powermode == MODE.HEY || chosenMode == true) {

			if(hoverTextTimer<=0.0f){
				chosenMode = false;
				hoverTextTimer = 5.0f;
				for(int i=0;i<people.Count;i++){
				people[i].GetComponent<NavAgentMovement>().haltMovement(false);
					//Debug.Log (people[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
				people[i].transform.GetChild(0).gameObject.SetActive(false);
				if(people[i].transform.childCount==3){
						Transform toDestroyRays = people[i].transform.GetChild(2);
						Destroy (toDestroyRays.gameObject);
					}
				}
			}else{
				if(chosenMode == false){
					chosenMode = true;
					chosenOne = Random.Range(0,people.Count);
					Debug.Log("HEY ACTIVATED"+chosenOne);
				}else
				hoverTextTimer -= Time.fixedDeltaTime;
			}
		}

		if (Powermode == MODE.BOO || interactNPC == true) {

			if(interactTimer<=0.0f){
				interactNPC = false;
				interactTimer = 5.0f;
				people[NPC].transform.GetChild(0).gameObject.SetActive(false);
			}else{

				if(interactNPC == false){
				NPC = Random.Range(0,people.Count);
				interactNPC = true;
					Debug.Log("BOO ACTIVATED"+NPC);
				}else
					interactTimer-=Time.fixedDeltaTime;
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

			Vector3 newLoc = people[i].transform.position;
			newLoc.y = 0;

			if(fireTimer<=1.0f && fireTimer > -3.0f && (people[i].transform.position - fireLoc).magnitude<=15.0f && people[i].transform.childCount<3){
				GameObject head_fire = (GameObject)Instantiate (Resources.Load ("human_fire"),Vector3.zero,	Quaternion.identity) as GameObject;
				head_fire.transform.parent = people[i].transform;
				head_fire.transform.localPosition = new Vector3(0,0.7f,0);
				person_script.toggleScaredRun(true);
				//Debug.Log(people[i].name+" on fire");
			}
		    if(fireTimer<=-3.0f && (people[i].transform.childCount>2)){

				csHandle.PowerFireball.AddXP(1,-1);

				GameObject fireObj = people[i].transform.GetChild(2).gameObject;
				Destroy(fireObj);
				//Debug.Log(people[i].name + " off fire");
			}

			if(tornadoOn == true){
				tornadoLoc.y = 0.0f;
				if(tornadoTime <= 0.0f)
					people[i].GetComponent<NavAgentMovement>().haltMovement(false);
				else{
					Vector3 dir = people[i].transform.position-tornadoLoc;
					if(dir.magnitude <= tornadoRange){
						csHandle.PowerTornado.AddXP(1,-1);
						//people[i].GetComponent<NavAgentMovement>().haltMovement(true);
						//people[i].GetComponent<Rigidbody>().AddForce(10000,300000,100000);
						people[i].GetComponent<NavMeshAgent>().Stop();
						people[i].GetComponent<NavMeshAgent>().updatePosition = false;
						people[i].GetComponent<NavMeshAgent>().updateRotation = false;
						people[i].rigidbody.isKinematic = false;
						people[i].rigidbody.useGravity = true;
						//people[i].rigidbody.AddForce(new Vector3(1,20,1));
						people[i].rigidbody.constraints = RigidbodyConstraints.None;
						people[i].rigidbody.AddRelativeForce(0.5f*dir.normalized + 2*Vector3.up,ForceMode.Impulse);
					} else {
//						if((people[i].transform.position-tornadoLoc).magnitude > 2*tornadoRange){
//							people[i].GetComponent<NavMeshAgent>().Resume();
//							people[i].GetComponent<NavMeshAgent>().updatePosition = true;
//							people[i].GetComponent<NavMeshAgent>().updateRotation = true;
//							people[i].rigidbody.isKinematic = true;
//							people[i].rigidbody.useGravity = false;
							//people[i].GetComponent<NavAgentMovement>().haltMovement(false);
//						}
					}
				}
			}

			if(interactNPC == true){
				if(NPC == i){
					if(interactTimer==5.0f){
						Debug.Log("BOO"+i);
						csHandle.PowerBoo.AddXP(1,-1);
					people[i].transform.GetChild(0).gameObject.SetActive(true);
					people[i].GetComponentInChildren<Text>().text = BooString[Random.Range(0,BooString.Count)];
						person_script.toggleScaredRun(true);
					
					}
				}
			}

			if(chosenMode == true){
				if(chosenOne == i){
					if(hoverTextTimer == 5.0f){
					Debug.Log ("CHOSEN!!"+i);
					csHandle.PowerHey.AddXP(1,1);
					people[i].transform.GetChild(0).gameObject.SetActive(true);
					people[i].GetComponentInChildren<Text>().text = HeyString[Random.Range(0,HeyString.Count)];
					GameObject godRays = (GameObject)Instantiate (Resources.Load ("GodRays"),Vector3.zero,	Quaternion.identity) as GameObject;
					godRays.transform.parent = people[i].transform;
					godRays.transform.localPosition = new Vector3(0,0,0);
					person_script.haltMovement(true);
					person_script.health += 10.0f;
					}
				}
			}

			switch(Powermode){
			case MODE.THUNDER_CLAP:
				person_script.toggleScaredRun(true);
				person_script.currentlyScared = true;
				//csHandle.PowerThunderClap.AddXP(1,-1);
				Debug.Log("Thunder Clap at "+pointOfContact+" ");
				break;
			}

			if(person_script.targetReached){
				//Random random = new Random();
				//random.seed = i;
				//updateSources();
				//Debug.Log("Rnd: " + rand_pos.ToString() + "," + nos_sources.ToString()+" "+sources[rand_pos]);
				//Debug.Log(people[i].name+" at index "+i);
				person_script.setNewPath(sources[Random.Range(0,sources.Count)]);
				//Debug.Log("Recompute the new path");
			}
		}
		Powermode = MODE.DEFAULT;
	}

	// Update is called once per frame
	void FixedUpdate () {

		//Debug.Log("HUMAN MANAGER");
		checkCrowd ();

		}
}
