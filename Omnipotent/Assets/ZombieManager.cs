using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ZombieManager : MonoBehaviour {
	public List<string> animalsList = new List<string>(new string []{"chr_zombie1","chr_zombie2","chr_zombie3","chr_zombie4"});
	public List<GameObject> ZombieList=new List<GameObject>();
	public GameObject[] destList;
	public List<Vector3> dest = new List<Vector3>();
	public int nos_dest=4; 
	public int nosZombies;
	public bool updateSources = false;
	public List<Vector3> zombSource = new List<Vector3>();

	public Vector3 hit3DLoc;
	List<int>fireBallHit = new List<int>();
	float timeToHit = 5.0f;
	bool fireTimer = false;
	double rayPowRange = 3.0f;

	List<int>tornadoHit = new List<int>();
	double tornadoTime = 25.0f;
	bool tornadoOn = false;
	double tornadoRange = 20.0f;
	Vector3 tornadoLoc = new Vector3();

	bool haltOn = false;
	float haltTimer = 3.0f;

	private cursor_handle csHandle ;


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

	public bool allZombiesDead = false;
	public bool zombsInit = false;

	// Use this for initialization
	void Start () {
		csHandle = GameObject.FindGameObjectWithTag ("Hand").GetComponent<cursor_handle> ();
		GameObject [] zSource = GameObject.FindGameObjectsWithTag("Zsource");
		foreach (GameObject zS in zSource) {
			zombSource.Add(zS.transform.position);
		}

		csHandle = GameObject.FindWithTag ("Hand").GetComponent<cursor_handle> ();

	}

	public void updateDest(){
		dest.Clear ();
		destList = GameObject.FindGameObjectsWithTag ("source");
		nos_dest = destList.Length;
		foreach (GameObject gob in destList) {
			dest.Add(gob.transform.position);
		}
	}

	public void deleteZombie(){
		int nosZombs = ZombieList.Count;

		for (int i=0; i<nosZombs; i++) {
			Destroy(ZombieList[i]);
		}
		zombsInit = false;
		ZombieList.Clear ();
	}

	public void addZombie(Vector3 spawnLoc){
		int rand_indx = Random.Range (0,nos_dest);
		Vector3 targetLoc = dest[rand_indx];
		int rand_chr = Random.Range(0,animalsList.Count);
		GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/"+animalsList[rand_chr]), spawnLoc,	Quaternion.identity) as GameObject;
		obs.name = animalsList[rand_chr];
		obs.transform.parent = transform;
		//obs.transform.GetComponent<Rigidbody>().detectCollisions = false;
		ZombieNavAgent zombScript = (ZombieNavAgent)obs.AddComponent("ZombieNavAgent");
		obs.transform.FindChild (animalsList[rand_chr]).transform.Rotate (Vector3.forward, 180);
		zombScript.target = targetLoc;
		ZombieList.Add(obs);
	}

	IEnumerator createZOM(Vector3 newSpawnPos, float l_secs)
	{
		yield return new WaitForSeconds(l_secs);
		int rand_indx = Random.Range (0,nos_dest);
		Vector3 targetLoc = newSpawnPos;
		int deltaVal = Random.Range(-10,10);
		Vector3 sourceLoc = zombSource[Random.Range(0,zombSource.Count)];
		int rand_chr = Random.Range(0,animalsList.Count);
		GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/"+animalsList[rand_chr]), sourceLoc,	Quaternion.identity) as GameObject;
		obs.name = animalsList[rand_chr];
		obs.transform.parent = transform;
		//obs.transform.GetComponent<Rigidbody>().detectCollisions = false;
		ZombieNavAgent zombScript = (ZombieNavAgent)obs.AddComponent("ZombieNavAgent");
		obs.transform.FindChild (animalsList[rand_chr]).transform.Rotate (Vector3.forward, 180);
		zombScript.target = targetLoc;
		ZombieList.Add(obs);
		zombsInit = true;
	}
	
	//initZombiesinLevel
	public void initZombies(int spawnZombies){
		float l_secs=0.0f;
		updateDest ();
		for (int i=0; i<spawnZombies; i++) {
			Vector3 destLoc = dest[Random.Range(0,dest.Count)];
			StartCoroutine(createZOM(destLoc, l_secs));
			l_secs++;
		}
	}

	public bool checkZombsDead(){
		if (ZombieList.Count == 0)
						allZombiesDead = true;
				else
						allZombiesDead = false;
		if (zombsInit == true && allZombiesDead == true)
						return true;
				else
						return false;
	}

	void checkPowerHit(){

		if (haltOn == true || Powermode == MODE.THUNDER_CLAP) {
			haltOn = true;
			if(haltTimer <= 0.0f){
				haltTimer = 3.0f;
				haltOn = false;
			}
			haltTimer-=Time.fixedDeltaTime;
		}

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

		if (Powermode == MODE.TORNADO)
						tornadoOn = true;

		if(tornadoOn == true && tornadoTime <= 0.0f){
			//Debug.Log ("Allowing all "+tornadoHit.Count);
			tornadoOn = false;
			tornadoTime = 25.0f;
		}else{
			if(tornadoOn == true){
				tornadoTime -= Time.fixedDeltaTime;
				tornadoLoc = GetComponentInParent<LevelController>().TornadoLoc;
				//Debug.Log(tornadoTime+" ");
			}
		}
			nosZombies = ZombieList.Count;
		for (int i=0; i<nosZombies; i++) {
			Vector3 zombLoc = ZombieList[i].transform.position;
			Vector3 newZombLoc = zombLoc;
			newZombLoc.y=0;
			double hitDistance = (hit3DLoc - newZombLoc).magnitude;


			if(Powermode == MODE.MJOLNIR ){
				if(hitDistance<=5.0f){
					//Debug.Log(ZombieList[i].name+" "+ZombieList[i].transform.position+" due to "+hit3DLoc);
					csHandle.PowerMjolnir.AddXP(1,1);
					Destroy(ZombieList[i]);
					ZombieList.RemoveAt(i);
		            nosZombies = ZombieList.Count;
					i--;
					continue;
				}
			}

			if(fireTimer == true){
				if(timeToHit<=0.0f){

						if((ZombieList[i].transform.position-hit3DLoc).magnitude <= 15.0f){
						    csHandle.PowerFireball.AddXP(1,1);

							//Debug.Log("Ball hitting"+ZombieList[i].name);
							if(ZombieList[i]!=null)
								Destroy(ZombieList[i]);
							if(i<ZombieList.Count)
								ZombieList.RemoveAt(i);
							i--;
							nosZombies = ZombieList.Count; 
						    continue;
						}
					}
			}

			if(tornadoOn == true){
				tornadoLoc.y=0.0f;
				if(tornadoTime<=0.0f)
					ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(false);
				else{
				if((ZombieList[i].transform.position-tornadoLoc).magnitude <= tornadoRange){
					csHandle.PowerTornado.AddXP(1,1);

					//Debug.Log ("halting: "+people[i].name);
					ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(true);
				}else{
					if((ZombieList[i].transform.position-tornadoLoc).magnitude > tornadoRange){
						ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(false);
					}
				 }
				}
			}

			if(haltOn == true){
				if(haltTimer>=0.0f)
					ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(true);
				else
					ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(false);
			}

			if(ZombieList[i].GetComponent<ZombieNavAgent>().targetReached == true){
				ZombieList[i].GetComponent<ZombieNavAgent>().setNewPath(dest[Random.Range(0,dest.Count)]);
			}

		}
		Powermode = MODE.DEFAULT;
		///foreach (int index  in indicesToDelete) {
		//	ZombieList.RemoveAt(index);
	//	}


	}




	// Update is called once per frame
	void FixedUpdate () {
		checkPowerHit ();
	}
}