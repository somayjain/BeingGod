using UnityEngine;
using System.Collections;

public class ZombieNavAgent : MonoBehaviour {
	public Vector3 target;
	public bool cursorHit = false;
	public NavMeshAgent agent;
	public Vector3 currentCursor3D = new Vector3(0,0,0);
	public bool powerhit = false;
	public float timeToHalt = 5.0f;
	public bool targetReached = true;

	public float waitTimer = 5.0f;

	// Use this for initialization
	void Start () {
	    agent = (NavMeshAgent)gameObject.AddComponent("NavMeshAgent");
		agent = (NavMeshAgent)gameObject.GetComponent("NavMeshAgent");
		agent.SetDestination(target);
		agent.speed = 1.5f;
		targetReached = false;
	}

	Vector3 getCursorLoc(){
		return GameObject.FindGameObjectWithTag ("Hand").GetComponent<cursor_handle>().cursor3d	;
	}

	public void haltMovement(bool halt){
		if (halt) {
						powerhit = true;
						agent.Stop ();
				} else {
			            powerhit = false;
						agent.Resume ();
				}
		
	}

	void OnTriggerEnter(Collider collision){
		//Debug.Log ("Zombie collides: "+gameObject.name+" "+collision.tag);
	}
	// Update is called once per frame
	void Update () {
		if (powerhit == true) {
			return;
		}


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

		if (agent.velocity.magnitude <= 0.2f && cursorHit==false) {
			waitTimer -= Time.fixedDeltaTime;
			//Debug.Log("Stopped2");
			if (waitTimer <= 0.0f || !agent.pathPending) {
				if (!agent.pathPending){
					//comments
					//Debug.Log ("Stopped at dest, destroy !!");
					targetReached = true;
					waitTimer = 5.0f;
				}
				if(waitTimer <= 0.0f)
				{
					waitTimer = 5.0f;
					//Debug.Log ("zombie stuck!");
				}
				
			}
		} else {
			waitTimer = 5.0f;
		}


	}
}
