using UnityEngine;
using System.Collections;

public class NavAgentMovement : MonoBehaviour {

	public Vector3 target;
	public NavMeshAgent agent;
	private bool switchLoc = false;
	public bool targetReached = false;
	public bool currentlyScared = false;
	public float scaredRunTimer = 4.0f;
	public float defaultSpeed;

	bool powerhit = false;

	public bool collisionWithZombie = false;

	public float waitTimer = 3.0f;

	public float health = 10.0f;
	public float damage = 4f;
	// Use this for initialization

	public bool deathCause = true;


	public Vector3 lastPos = new Vector3();
	public float lastCheck = 1.0f;
	public float Ddist;
	//public bool pendingPath = true;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination(target);
		//agent.stoppingDistance = Random.Range (0, 10);
		agent.speed += 4.0f;
		defaultSpeed = agent.speed;
		agent.acceleration = 20.0f;
		lastPos = transform.position;

		//gameObject.AddComponent<GUIText> ();
		//guiText.text = "HELLO!!";
	}
	
	public void setNewPath(Vector3 targetLoc){

		//agent.transform.Rotate (0,30,0);
		bool success = agent.SetDestination(targetLoc);
		//Debug.Log (targetLoc+"Done reset"+transform.position+" "+success);
		agent.stoppingDistance = Random.Range (0, 10);
		targetReached = false;
		//toggleScaredRun (true);
	}

	void OnTriggerEnter(Collider collision){
		if (collision.tag == "zombie") {
			health -= damage;
			toggleScaredRun(true);
			if(health<=0.0f)
			deathCause = true;
		}
		if (collision.tag == "Dino" || collision.tag == "Spidey") {
			health -= damage;
			toggleScaredRun(true);
			if(health<=0.0f)
			deathCause = false;
		}
	}

	public void toggleScaredRun(bool scared){
		if (scared) {
						//Debug.Log ("Run for your life!!! He seems angry!!");
						agent.speed = defaultSpeed + 5.0f;
				} else {
						//Debug.Log("Phew! That was close!");
						agent.speed = defaultSpeed;
			            scaredRunTimer = 4.0f;
			            currentlyScared = false;
				}
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
	
	// Update is called once per frame
	void FixedUpdate () {
		if (powerhit == true) {
			//Debug.Log(gameObject.name+" stopping ");
						return;
				}
		//Debug.Log (powerhit + " ");

		if(currentlyScared)
		scaredRunTimer -= Time.fixedDeltaTime;
		if (scaredRunTimer <= 0) {
			scaredRunTimer = 0.0f;
						if (currentlyScared == true) {
							toggleScaredRun (false);
						}
				}
		
		/*
		if (/*agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid ||*/  //!agent.hasPath) {
			//Debug.Log(agent.pathStatus+" "+agent.hasPath+" "+transform.name);
			/*targetReached = true;
			return;
		} 
		*/


		lastCheck -= Time.fixedDeltaTime;
		Ddist = (transform.position-lastPos).magnitude;
		if (lastCheck <= 0.0f && targetReached == false) {
			if((transform.position-lastPos).magnitude <= 0.5f){
				/*if(!agent.pathPending){
					Vector3 newTarget = new Vector3(transform.position.x+3.0f,transform.position.y, transform.position.z+2.0f);
					agent.SetDestination(newTarget);
				}*/
		
				//if(agent.path.status == NavMeshPathStatus.PathPartial)
				float randDelta = Random.Range(-10,10);
				float randD = randDelta/8.0f;
				agent.Warp(new Vector3(transform.position.x-randD,transform.position.y,transform.position.z-randD));

				//Debug.Log(transform.name+" stuck "+transform.position+" "+lastPos);
				targetReached = true;
			}
			lastCheck = 1.0f;
			lastPos = transform.position;
		}
		/*
		
		if (agent.velocity.magnitude <= 0.2f) {
						//Debug.Log ("Stopped1");
						waitTimer -= Time.fixedDeltaTime;
						//Debug.Log("Stopped2");
						if (waitTimer <= 0.0f || !agent.pathPending) {
								if (!agent.pathPending){
										//comments
										//Debug.Log ("Stopped at dest");
										targetReached = true;
					                    waitTimer = 3.0f;
								}
								if(waitTimer <= 0.0f)
								{
					                    waitTimer = 3.0f;
										Debug.Log ("villager stuck");
								}
								
						}
				} else {
						waitTimer = 5.0f;
		}

*/
	}
}
