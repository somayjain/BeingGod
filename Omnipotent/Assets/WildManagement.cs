using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WildManagement : MonoBehaviour {

	public GameObject[] wildSpots;
	public int MaxActiveObj=5;
	List<GameObject>activeObj;
	string[] enemName = new string[]{"Dino","CaveWorm"};
	List<GameObject> enemList = new List<GameObject>();	


	public Vector3 hit3DLoc;
	List<int>fireBallHit = new List<int>();
	float timeToHit = 2.0f;
	bool fireTimer = false;
	double rayPowRange = 3.0f;
	
	List<int>tornadoHit = new List<int>();
	double tornadoTime = 25.0f;
	bool tornadoOn = false;
	double tornadoRange = 20.0f;
	Vector3 tornadoLoc = new Vector3();
	
	bool haltOn = false;
	float haltTimer = 3.0f;

	int bossNos;

	private cursor_handle csHandle;
	
	
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


	// Use this for initialization
	void Start () {
		csHandle = GameObject.FindWithTag ("Hand").GetComponent<cursor_handle> ();
	}

	public void checkEnem(){

		
		if (Powermode == MODE.FIREBALL || Powermode == MODE.MJOLNIR) {
			hit3DLoc = GetComponentInParent<LevelController>().PowerLoc;
			hit3DLoc.y=0.0f;
		}
		if (Powermode == MODE.FIREBALL || fireTimer == true) {
			fireTimer=true;
			if(timeToHit<=0.0f){
				fireTimer = false;
				timeToHit = 5.0f;
			}
			else{
				timeToHit-=Time.fixedDeltaTime;
			}
		}

		bossNos = enemList.Count;
		for (int i=0; i<bossNos; i++) {
			Vector3 bossLoc = enemList[i].transform.position;
			double hitDistance = (hit3DLoc - bossLoc).magnitude;

			if(enemList[i].GetComponent<WildMovement>().agentReached == true){
				enemList[i].GetComponent<WildMovement>().resetPath(wildSpots[Random.Range(0,wildSpots.Length)].transform.position);
				Debug.Log("resetting BOSS");
			}


			
			if(Powermode == MODE.MJOLNIR ){
				if(hitDistance<=rayPowRange){
					csHandle.PowerMjolnir.AddXP(1,1);
					Debug.Log(i+" enemy List "+enemList[i].GetComponent<WildMovement>().health);
					enemList[i].GetComponent<WildMovement>().health -= 10.0f;

				}
			}
			
			if(fireTimer == true){
				if(timeToHit<=0.0f){
					if((enemList[i].transform.position-hit3DLoc).magnitude <= 15.0f){
						csHandle.PowerFireball.AddXP(1,1);
						//Debug.Log("Ball hitting"+ZombieList[i].name);
						enemList[i].GetComponent<WildMovement>().health -= 10.0f;
					}
				}
			}
			if(enemList[i].GetComponent<WildMovement>().health <= 0.0f){
				Destroy(enemList[i]);
				enemList.RemoveAt(i);
				bossNos = enemList.Count;
				i--;
				continue;
			}
		}
		Powermode = MODE.DEFAULT;
		///foreach (int index  in indicesToDelete) {
		//	ZombieList.RemoveAt(index);
		//	}

	}

	IEnumerator createENE(float l_secs){
		yield return new WaitForSeconds (l_secs);
		int rand_indx = Random.Range (0,wildSpots.Length);
		Vector3 targetLoc = wildSpots[rand_indx].transform.position;
		int deltaVal = Random.Range(-10,10);
		Vector3 sourceLoc = wildSpots[Random.Range(0,wildSpots.Length)].transform.position;
		int rand_chr = Random.Range(0,enemName.Length);
		GameObject obs = (GameObject)Instantiate (Resources.Load (enemName[rand_chr]), sourceLoc,	Quaternion.identity) as GameObject;
		obs.name = enemName[rand_chr];
		obs.transform.parent = transform;
		enemList.Add (obs);
	}

	public void SpawnEnemy(int nos){
		float l_secs = 0.0f;
		for (int i=0; i<nos; i++) {
			StartCoroutine(createENE(l_secs));
			l_secs++;
		}
	}

	// Update is called once per frame
	void Update () {
		checkEnem ();
	}
}
