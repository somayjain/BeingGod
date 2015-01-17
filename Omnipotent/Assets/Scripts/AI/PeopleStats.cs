using UnityEngine;
using System.Collections;

public class PeopleStats : MonoBehaviour {
	private int peoplePresent=0;
	private int fearPeople=0;
	private int faithPeople=0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void addPeople(){
		peoplePresent++;
	}
	public int totalPeople(){
		return peoplePresent;
	}
}
