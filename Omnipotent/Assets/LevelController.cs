using UnityEngine;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

	public int currentLevel;

	public GameObject zombieManager;
	public GameObject buildingManager;
	public GameObject peopleManager;

	public bool zombieStatus ;
	public int humanStatus ;
	public int houseStatus ;

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
	public Vector3 PowerLoc = new Vector3();

	//Power
	public void setPower(MODE currentMode){
		Powermode = currentMode;
	}

	public void setPowerLoc(Vector3 powL){
		PowerLoc = powL;
	}

	//Levels: min requirements
	int minHouses2Build=3 ;
	int zombies2Kill   =10;
	int humans2create  =15;


	bool autoSpawn = false;
	float time2Spawn = 10.0f;

	void initMinReqs(int level){
		int nosZombies = level * Random.Range (5,zombies2Kill);
		int nosHouses = level * Random.Range (1,minHouses2Build);

		zombieManager.GetComponent<ZombieManager> ().initZombies (nosZombies);
		//housemanager.init(nosHouses)
		minHouses2Build = level * minHouses2Build;

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
		//peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (nosPeople);

		buildingManager = GameObject.FindGameObjectWithTag ("Building");

		// Init Zombie manager
		zombieManager = (GameObject)Instantiate (Resources.Load ("ZombieManager"), gameObject.transform.position, Quaternion.identity) as GameObject;
		zombieManager.transform.parent = transform;
		GameObject.FindGameObjectWithTag("Hand").GetComponent<cursor_handle>().currentLevel = zombieManager;
		buildingManager = GameObject.FindGameObjectWithTag ("Building");
	}

	public void setLevel(int level){
		currentLevel = level;
	}

	public void setPower(string power, Vector3 powerLocation){

	}


	private void resetLevel(){
		zombieManager.GetComponent<ZombieManager> ().deleteZombie ();
		if (currentLevel > 1) {
						int nosPeople = currentLevel * Random.Range (10, humans2create);
						peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (nosPeople);
						initMinReqs (currentLevel);
				}
	}

	// Update is called once per frame
	void Update () {
		zombieStatus = zombieManager.GetComponent<ZombieManager> ().allZombiesDead;
		humanStatus = peopleManager.GetComponent<LoadVoxelPeople> ().people.Count;
		houseStatus = buildingManager.GetComponent<houseManager> ().nosHouses;
		// bool houseStatus = getstatus

		if (currentLevel == 1) {
			if(buildingManager.GetComponent<houseManager>().nosHouses == minHouses2Build){
			int nosPeople = currentLevel * Random.Range (10,humans2create);
			peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (nosPeople);
				currentLevel++;
				initMinReqs(currentLevel);
				return;
			}

		}

		if (currentLevel > 1) {

			if(humanStatus == 0){
				resetLevel();
			}
			if(houseStatus == 0){
				currentLevel=1;
				resetLevel();
			}
		}

		if (currentLevel == 2) {
			//conditions to check level 2 criterias		
		}

		if (zombieStatus /*&& houseStatus*/  && currentLevel ==3) {
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
