using UnityEngine;
using System.Collections;

public class ZombieNavAgent : MonoBehaviour {
	public Vector3 target;
	public bool cursorHit = false;
	public NavMeshAgent agent;
	public Vector3 currentCursor3D = new Vector3(0,0,0);
	public bool powerhit = false;
	public float timeToHalt = 5.0f;
	public bool targetReached = false;

	public float waitTimer = 5.0f;

	Vector3 lastPos = new Vector3();
	float lastCheck = 1.0f;

	// Use this for initialization
	void Start () {
	    agent = (NavMeshAgent)gameObject.AddComponent("NavMeshAgent");
		agent = (NavMeshAgent)gameObject.GetComponent("NavMeshAgent");
		agent.SetDestination(target);
		agent.speed = 3.0f;
		agent.acceleration = 16.0f;
		targetReached = false;
		lastPos = transform.position;
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

	public void setNewPath(Vector3 newLoc){
		float randDelta = Random.Range(-10,10);
		float randD = randDelta/8.0f;
		agent.Warp(new Vector3(transform.position.x-randD,transform.position.y,transform.position.z-randD));
		agent.SetDestination (newLoc);
		agent.stoppingDistance = Random.Range (0, 10);
		targetReached = false;
	}

	void OnTriggerEnter(Collider collision){
		//Debug.Log ("Zombie collides: "+gameObject.name+" "+collision.tag);
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (powerhit == true) {
			return;
		}

		lastCheck -= Time.fixedDeltaTime;
		if (lastCheck <= 0.0f && targetReached == false) {
			if((transform.position-lastPos).magnitude <= 0.5f){
				/*if(!agent.pathPending){
					Vector3 newTarget = new Vector3(transform.position.x+3.0f,transform.position.y, transform.position.z+2.0f);
					agent.SetDestination(newTarget);
				}*/
				
				//if(agent.path.status == NavMeshPathStatus.PathPartial)

				
				//Debug.Log(transform.name+" stuck "+transform.position+" "+lastPos);
				targetReached = true;
			}
			lastCheck = 1.0f;
			lastPos = transform.position;
		}

		//Stopping on touch
		/*Vector3 distance = currentCursor3D - transform.position;
		if (distance.magnitude < 1.0f && cursorHit == false) {
						agent.Stop (false);
						cursorHit = true;
				} else {
					if(distance.magnitude>1.0f && cursorHit == true){
				cursorHit = false;
				agent.Resume();
			}
		}*/
		/*
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

*/
	}
}
