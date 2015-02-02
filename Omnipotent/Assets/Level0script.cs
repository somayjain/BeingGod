using UnityEngine;
using System.Collections.Generic;

public class Level0script : MonoBehaviour {
	public List<string> animalsList = new List<string>(new string []{"chr_zombie1","chr_zombie2","chr_zombie3","chr_zombie4"});
	public List<GameObject> ZombieList=new List<GameObject>();
	public List<Vector3> dest = new List<Vector3>();
	public int nos_dest=4; 
	public int nosZombies;

	public Vector3 hit3DLoc;

	List<int>fireBallHit = new List<int>();
	float timeToHit = 3.0f;
	bool fireTimer = false;

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
	double tornadoRange = 10.0f;
	double rayPowRange = 2.0f;

	// Use this for initialization
	void Start () {
		Debug.Log ("Level 0 started");
		nosZombies = Random.Range(10,20);
		Debug.Log (nosZombies+" Init ");
		for (int i=0; i<nos_dest; i++) {
			GameObject obj = GameObject.FindGameObjectWithTag("source"+i);
			Vector3 objLoc = obj.transform.position;
			dest.Add(objLoc);
			Debug.Log(obj.name);
		}

		for (int i=0; i<nosZombies; i++) {
			int rand_indx = Random.Range (0,nos_dest);
			Vector3 targetLoc = dest[rand_indx];
			int deltaVal = Random.Range(-10,10);
			Vector3 sourceLoc = new Vector3(gameObject.transform.position.x+deltaVal+i,1,gameObject.transform.position.z+deltaVal);
			int rand_chr = Random.Range(0,animalsList.Count);
			Debug.Log("Creating Zombie");
			GameObject obs = (GameObject)Instantiate (Resources.Load ("prefabs/"+animalsList[rand_chr]), sourceLoc,	Quaternion.identity) as GameObject;
			obs.name = animalsList[rand_chr]+i;
			obs.transform.parent = transform;
			Debug.Log("Instantiated Zombie");
			ZombieNavAgent zombScript = (ZombieNavAgent)obs.AddComponent("ZombieNavAgent");
			Debug.Log("Access zombScript");
			obs.transform.FindChild (animalsList[rand_chr]).transform.Rotate (Vector3.forward, 180);
			zombScript.target = targetLoc;
			ZombieList.Add(obs);
		}

	}

	void checkPowerHit(){

		if (fireTimer == true && timeToHit < 0.0f) {
			Debug.Log(timeToHit+" time to hit");
			timeToHit = 3.0f;
			int removed = 0;
			foreach(int index in fireBallHit){
				Destroy(ZombieList[index+removed]);
				ZombieList.RemoveAt(index+removed);
				removed--;
			}
			fireBallHit.Clear();
			nosZombies = ZombieList.Count;
		}
	
		for (int i=0; i<nosZombies; i++) {
			Vector3 zombLoc = ZombieList[i].transform.position;
			double hitDistance = (hit3DLoc - zombLoc).magnitude;
			if(Powermode == MODE.MJOLNIR ){
				if(hitDistance<=rayPowRange){
					 Debug.Log(ZombieList[i].name+" is hit ");
					//ZombieList.RemoveAt(i);
					Destroy(ZombieList[i]);
					ZombieList.RemoveAt(i);
		            nosZombies = ZombieList.Count;
					i--;
				}
			}
			if(Powermode == MODE.FIREBALL){
			    if(hitDistance <= rayPowRange){
					fireBallHit.Add (i);
					fireTimer = true;
			   }
			}
				if(Powermode==MODE.TORNADO && hitDistance <= tornadoRange)
					ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(true);

				if(Powermode == MODE.THUNDER_CLAP)
					ZombieList[i].GetComponent<ZombieNavAgent>().haltMovement(true);

		}
		Powermode = MODE.DEFAULT;
		///foreach (int index  in indicesToDelete) {
		//	ZombieList.RemoveAt(index);
	//	}
	}

	// Update is called once per frame
	void FixedUpdate () {
		checkPowerHit ();
		if(fireTimer==true){
			timeToHit -= Time.fixedDeltaTime;
		}
	}
}
