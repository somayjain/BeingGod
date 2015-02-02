using UnityEngine;
using System.Collections;

public class ZombieNavAgent : MonoBehaviour {
	public Vector3 target;
	public bool cursorHit = false;
	public NavMeshAgent agent;
	public Vector3 currentCursor3D = new Vector3(0,0,0);
	public bool powerhit = false;
	public float timeToHalt = 5.0f;
	// Use this for initialization
	void Start () {
	    agent = (NavMeshAgent)gameObject.AddComponent("NavMeshAgent");
		agent = (NavMeshAgent)gameObject.GetComponent("NavMeshAgent");
		agent.SetDestination(target);
		agent.speed = 0.5f;
	}

	Vector3 getCursorLoc(){
		return GameObject.FindGameObjectWithTag ("Hand").GetComponent<cursor_handle>().cursor3d	;
	}

	public void haltMovement(bool halt){
		if (halt) {
						powerhit = true;
						agent.Stop (false);
				} else {
			            powerhit = false;
						agent.Resume ();
				}
		
	}

	// Update is called once per frame
	void Update () {
		currentCursor3D = getCursorLoc ();


		//Stopping on touch
		Vector3 distance = currentCursor3D - transform.position;
		if (distance.magnitude < 1.0f && cursorHit == false) {
						agent.Stop (false);
						cursorHit = true;
				} else {
					if(distance.magnitude>1.0f && cursorHit == true){
				cursorHit = false;
				agent.Resume();
			}
			}

		if (powerhit == true) {
			timeToHalt -= Time.fixedDeltaTime;
			if(timeToHalt<= 0.0f){
				haltMovement(false);
				timeToHalt = 5.0f;
			}
		}
	}
}
