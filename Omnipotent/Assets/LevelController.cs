using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

	bool gamePaused = false;
	bool levelInProgress = false;
	public int currentLevel=0;
	bool levelInit = true;
	float tutTimer = 15.0f;
	public GameObject TutImage;
	public Toggle[] toggle;
	public int[] objForLevels;

	public int nosOfObjectives;
	public bool[]objectiveComplete=new bool[]{false,false, false, false, false};
	public int[] ObjectiveNos = new int[]{0,2,3,4,0};
	public int freeLevel = 4;

	public GameObject zombieManager;
	public GameObject buildingManager;
	public GameObject peopleManager;

	public bool zombieStatus ;
	public int humanStatus ;
	public int houseStatus ;

	int prevHouseNos;

	public XP_handle xp_handler;

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
		TORNADO,
		BOO,
		HEY
	}
	public MODE Powermode = MODE.DEFAULT;

	public Vector3 PowerLoc = new Vector3();
	public Vector3 TornadoLoc = new Vector3 ();
	

	//Levels: min requirements
	int minHouses2Build=5 ;


	bool autoSpawn = false;
	float time2Spawn = 10.0f;

	void initMinReqs(int level){
		//int nosZombies = level * Random.Range (5,zombies2Kill);
		//int nosHouses = level * Random.Range (1,minHouses2Build);

		//zombieManager.GetComponent<ZombieManager> ().initZombies (nosZombies);
		//housemanager.init(nosHouses)
		//minHouses2Build = level * minHouses2Build;

	}

	void updateObjectives(){
		if (currentLevel == 1) {
			if(buildingManager.GetComponent<houseManager>().nosHouses >= minHouses2Build){
				toggle[1].isOn = true;
				toggle[0].isOn = true;
				toggle[0].transform.GetChild(1).GetComponent<Text>().color=Color.green;
				toggle[1].transform.GetChild(1).GetComponent<Text>().color=Color.green;
				objectiveComplete[0] = true;
				objectiveComplete[1] = true;
				Debug.Log("changed color");
			}
		}
		if (currentLevel == 2) {
			if(Powermode == MODE.GMBC){
				objectiveComplete[0] = true;
				toggle[0].isOn = true;
				toggle[0].transform.GetChild(1).GetComponent<Text>().color=Color.green;
				Debug.Log("changed weather");
			}
			if(Powermode == MODE.HEY){
				objectiveComplete[1] = true;
				toggle[1].isOn = true;
				toggle[1].transform.GetChild(1).GetComponent<Text>().color=Color.green;
				Debug.Log("changed Hey");
			}
			if(Powermode == MODE.BOO){
				objectiveComplete[2] = true;
				toggle[2].isOn = true;
				toggle[2].transform.GetChild(1).GetComponent<Text>().color=Color.green;
				Debug.Log("changed Boo");
			}
			if(xp_handler.Faith+xp_handler.Fear==xp_handler.XP_Limit){
				objectiveComplete[3] = true;
				toggle[3].isOn = true;
				toggle[3].transform.GetChild(1).GetComponent<Text>().color=Color.green;
				Debug.Log("changed XP");
			}
		}
	}

	void displayObjective(){
		if (currentLevel == 1) {
			toggle[0].transform.GetChild(1).GetComponent<Text>().text = "Explore The World!";
			toggle[0].transform.GetChild(1).GetComponent<Text>().color=Color.red;
			toggle[0].gameObject.SetActive(true);
			toggle[0].isOn = false;
				
			toggle[1].transform.GetChild(1).GetComponent<Text>().text = "Create 5 Houses!";
			toggle[1].transform.GetChild(1).GetComponent<Text>().color=Color.red;
			toggle[1].gameObject.SetActive(true);
			toggle[1].isOn = false;
			}
		if (currentLevel == 2) {
			toggle[0].transform.GetChild(1).GetComponent<Text>().text = "Change The Weather!!";
			toggle[0].gameObject.SetActive(true);
			toggle[0].isOn = false;
			toggle[0].transform.GetChild(1).GetComponent<Text>().color=Color.red;

			toggle[1].transform.GetChild(1).GetComponent<Text>().text = "!!Hey You!!";
			toggle[1].transform.GetChild(1).GetComponent<Text>().color=Color.red;
			toggle[1].gameObject.SetActive(true);
			toggle[1].isOn = false;

			toggle[2].transform.GetChild(1).GetComponent<Text>().text = "!!BOO!!";
			toggle[2].gameObject.SetActive(true);
			toggle[2].isOn = false;
			toggle[2].transform.GetChild(1).GetComponent<Text>().color=Color.red;

			toggle[3].transform.GetChild(1).GetComponent<Text>().text = "Fill The XP Bar!";
			toggle[3].gameObject.SetActive(true);
			toggle[3].isOn = false;
			toggle[3].transform.GetChild(1).GetComponent<Text>().color=Color.red;

			
		}
	}


	bool ObjectiveCompleted(){
		for (int i=0; i<nosOfObjectives; i++) {
			if(objectiveComplete[i]==false)
				return false;
		}
		return true;
	}

	void resetObjectives(){
		for (int i=0; i<nosOfObjectives; i++) {
			toggle[i].gameObject.SetActive(false);
			objectiveComplete [i] = false;
		}
		nosOfObjectives = ObjectiveNos [currentLevel];
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
		//int nosPeople = currentLevel * Random.Range (10,humans2create);
		//peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (nosPeople);

		buildingManager = GameObject.FindGameObjectWithTag ("Building");

		// Init Zombie manager
		zombieManager = (GameObject)Instantiate (Resources.Load ("ZombieManager"), gameObject.transform.position, Quaternion.identity) as GameObject;
		zombieManager.transform.parent = transform;
		GameObject.FindGameObjectWithTag("Hand").GetComponent<cursor_handle>().currentLevel = zombieManager;
		buildingManager = GameObject.FindGameObjectWithTag ("Building");
		prevHouseNos = 0;
	}

	public void setLevel(int level){
		currentLevel = level;
	}


	private void resetLevel(){
		zombieManager.GetComponent<ZombieManager> ().deleteZombie ();
		if (currentLevel > 1) {
						//int nosPeople = currentLevel * Random.Range (10, humans2create);
						//peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (nosPeople);
						initMinReqs (currentLevel);
				}
	}


	void setCurrentPower(){
		if (MODE.FIREBALL == Powermode) {
			zombieManager.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.FIREBALL;
			peopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.FIREBALL;
		}
		if (MODE.TORNADO == Powermode) {
			zombieManager.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.TORNADO;
			peopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.TORNADO;
		}
		if (MODE.MJOLNIR == Powermode) {
			zombieManager.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.MJOLNIR;
			peopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.MJOLNIR;
		}
		if (MODE.THUNDER_CLAP == Powermode) {
			zombieManager.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.THUNDER_CLAP;
			peopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.THUNDER_CLAP;
		}
		if (MODE.HEY == Powermode) {
			peopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.HEY;
		}
		if (MODE.BOO == Powermode) {
			peopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.BOO;
		}
		Powermode = MODE.DEFAULT;	
	}

	// Level Logic

	void LevelLogic(){
		if (gamePaused) {
				} else {
			if(currentLevel == 1){
				//Debug.Log("Every Frame_0");
				if(levelInit == true){
					xp_handler.LevelUp(currentLevel);
					nosOfObjectives = ObjectiveNos[currentLevel];
					levelInit = false;
					tutTimer = 15.0f;
					Sprite newTutImage = Resources.Load<Sprite>("intel.svg");
					TutImage.GetComponent<Image>().sprite = newTutImage;
					TutImage.SetActive(true);
					displayObjective();
				}else{
					if(tutTimer <= 0.0f){
						TutImage.SetActive(false);
						updateObjectives();
						bool nextLevelStatus = ObjectiveCompleted();
						if(nextLevelStatus == true){
							levelInit = true;
							currentLevel++;
							resetObjectives();
						}
					}else{
						tutTimer-=Time.fixedDeltaTime;
					}
				}
			}else{
				if(currentLevel == 2){
					if(levelInit == true){
						xp_handler.LevelUp(currentLevel);
						nosOfObjectives = ObjectiveNos[currentLevel];
						levelInit = false;
						tutTimer = 15.0f;
						Sprite newTutImage = Resources.Load<Sprite>("intel.svg");
						TutImage.GetComponent<Image>().sprite = newTutImage;
						TutImage.SetActive(true);
						displayObjective();
					}else{
						if(tutTimer <= 0.0f){
							TutImage.SetActive(false);
							updateObjectives();
							bool nextLevelStatus = ObjectiveCompleted();
							if(nextLevelStatus == true){
								levelInit = true;
								currentLevel++;
								resetObjectives();
							}
						}else{
							tutTimer-=Time.fixedDeltaTime;
						}
					}
				}
				//Debug.Log("Every Frame_3");
			}
		}
		setCurrentPower ();
	}


	// Update is called once per frame
	void Update () {
				LevelLogic ();

				// bool houseStatus = getstatus
				if (buildingManager.GetComponent<houseManager> ().houseCreated) {
						buildingManager.GetComponent<houseManager> ().houseCreated = false;
						updateSources ();
						peopleManager.GetComponent<LoadVoxelPeople> ().initPerson (5);
				}
				return;

				zombieStatus = zombieManager.GetComponent<ZombieManager> ().allZombiesDead;
				humanStatus = peopleManager.GetComponent<LoadVoxelPeople> ().people.Count;
				houseStatus = buildingManager.GetComponent<houseManager> ().nosHouses;

				/*
		if (currentLevel == 1) {
			if(buildingManager.GetComponent<houseManager>().nosHouses == minHouses2Build){
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

		if (zombieStatus && houseStatus && currentLevel ==3) {
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
		}*/
	}
}
