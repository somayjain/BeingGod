using UnityEngine;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

	public int currentLevel;

	public GameObject zombieManager;
	public GameObject buildingManager;
	public GameObject peopleManager;


	public List<string>currentPower = new List<string>();
	//Power Chart
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


	//Levels: min requirements
	int minHouses2Build=3 ;
	int zombies2Kill   =10;
	int humans2create  =15;


	bool autoSpawn = false;
	float time2Spawn = 10.0f;

	void initMinReqs(int level){
		int nosZombies = level * Random.Range (5,zombies2Kill);
		int nosHouses = level * Random.Range (1, minHouses2Build);

		zombieManager.GetComponent<ZombieManager> ().initZombies (nosZombies);
		//housemanager.init(nosHouses)
		buildingManager.GetComponent<houseManager> ().nosHouses = level * minHouses2Build;

	}


	public void updateSources(){
		peopleManager.GetComponent<LoadVoxelPeople> ().updateSources ();
		zombieManager.GetComponent<ZombieManager> ().updateDest ();
	}

	// Use this for initialization
	void Start () {
		currentLevel = 1;

		//init Person manager
		peopleManager = GameObject.FindGameObjectWithTag ("PeopleManager");
		int nosPeople = currentLevel * Random.Range (10,humans2create);
		peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (nosPeople);

		// Init Zombie manager
		zombieManager = (GameObject)Instantiate (Resources.Load ("ZombieManager"), gameObject.transform.position, Quaternion.identity) as GameObject;
		zombieManager.transform.parent = transform;
		GameObject.FindGameObjectWithTag("Hand").GetComponent<cursor_handle>().currentLevel = zombieManager;
		buildingManager = GameObject.FindGameObjectWithTag ("Building");
		initMinReqs (currentLevel);
	}

	public void setLevel(int level){
		currentLevel = level;
	}

	public void setPower(string power, Vector3 powerLocation){

	}

	// Update is called once per frame
	void Update () {
		bool zombieStatus = zombieManager.GetComponent<ZombieManager> ().allZombiesDead;
		// bool houseStatus = getstatus
		if (zombieStatus /*&& houseStatus*/  && currentLevel <=3) {
			currentLevel++;
			initMinReqs(currentLevel);
		}else{
			if(currentLevel==4){
				currentLevel++;
				autoSpawn = true;
			}
		}
		if (autoSpawn == true) {
			time2Spawn -= Time.fixedDeltaTime;
			if(time2Spawn <= 0.0f){
				int nosZombies = Random.Range (1,3);
				zombieManager.GetComponent<ZombieManager> ().initZombies (nosZombies);
				time2Spawn = 10.0f;
			}
		}
	}
}
