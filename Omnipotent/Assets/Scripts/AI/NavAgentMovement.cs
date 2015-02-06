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

	public bool collisionWithZombie = false;

	public float waitTimer = 5.0f;

	public float health = 10.0f;
	public float damage = 4f;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination(target);
		agent.stoppingDistance = Random.Range (0, 10);
		defaultSpeed = agent.speed;


		//gameObject.AddComponent<GUIText> ();
		//guiText.text = "HELLO!!";
	}
	
	public void setNewPath(Vector3 targetLoc){
		agent.SetDestination(targetLoc);
		targetReached = false;
	}

	void OnTriggerEnter(Collider collision){
		if (collision.tag == "zombie") {
			health -= damage;
			agent.speed += 2.0f;
		}
	}

	public void toggleScaredRun(bool scared){
		if (scared) {
						Debug.Log ("Run for your life!!! He seems angry!!");
						agent.speed = defaultSpeed + 5.0f;
				} else {
						Debug.Log("Phew! That was close!");
						agent.speed = defaultSpeed;
			            scaredRunTimer = 4.0f;
			             currentlyScared = false;
				}
	}
	
	// Update is called once per frame
	void Update () {
		if(currentlyScared)
		scaredRunTimer -= Time.fixedDeltaTime;
		if (scaredRunTimer <= 0) {
			scaredRunTimer = 0.0f;
						if (currentlyScared == true) {
							toggleScaredRun (false);
						}
				}

		if (agent.velocity.magnitude <= 0.2f) {
						Debug.Log ("Stopped1");
						waitTimer -= Time.fixedDeltaTime;
						//Debug.Log("Stopped2");
						if (waitTimer <= 0.0f || !agent.pathPending) {
								if (!agent.pathPending){
										//comments
										Debug.Log ("Stopped at dest");
										targetReached = true;
					                    waitTimer = 5.0f;
								}
								if(waitTimer <= 0.0f)
								{
					                    waitTimer = 5.0f;
										Debug.Log ("villager stuck");
								}
								
						}
				} else {
						waitTimer = 5.0f;
		}


	}
}
