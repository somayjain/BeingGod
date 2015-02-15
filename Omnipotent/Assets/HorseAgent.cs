using UnityEngine;
using System.Collections;

public class HorseAgent : MonoBehaviour {

	private NavMeshAgent agent;
	public bool targetReached = false;
	private Vector3 lastPos;
	float lastCheck = 1.0f;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}


	public void newPath(Vector3 newLoc){
		//Debug.Log (newLoc);
		lastPos = newLoc;
		agent.SetDestination(newLoc);
		targetReached = false;
	}

	// Update is called once per frame
	void Update () {
		
		lastCheck -= Time.deltaTime;
		if (lastCheck <= 0.0f && targetReached == false) {
			if((transform.position-lastPos).magnitude <= 0.5f){
				targetReached = true;
			}
			lastCheck = 1.0f;
			lastPos = transform.position;
		}

	}
}
