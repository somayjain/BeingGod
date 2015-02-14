using UnityEngine;
using System.Collections;

public class HorseNav : MonoBehaviour {

	public GameObject[] horses;
	public GameObject[] HD;



	// Use this for initialization
	void Start () {
		for (int i=0; i<horses.Length; i++) {
			horses[i].GetComponent<HorseAgent>().newPath(HD[Random.Range(0,HD.Length)].transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<horses.Length; i++) {
			if(horses[i].GetComponent<HorseAgent>().targetReached == true){
				horses[i].GetComponent<HorseAgent>().newPath(HD[Random.Range(0,HD.Length)].transform.position);
			}
		}
	}
}
