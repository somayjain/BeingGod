using UnityEngine;
using System.Collections;

public class WildMovement : MonoBehaviour {


	public Vector3 currentDestination; 
	public NavMeshAgent agent;

	public float health = 20.0f;

	public bool attackMode = false;
	public bool hitMode = false;
	public bool deadMode = false;
	public float switchTimer = 2.0f;
	public float hitTimer = 2.0f;
	public float deadTimer = 2.0f;
	public bool agentReached = false;

	string attackAnim;
	string walkAnim;
	string deadAnim;
	string hitAnim;

	Vector3 lastPos = new Vector3();
	float lastCheck = 1.0f;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		if (gameObject.name == "Dino") {
						walkAnim = "Allosaurus_Run";
						attackAnim = "Allosaurus_Attack02";
						hitAnim = "Allosaurus_Hit01";
						deadAnim="Allosaurus_Die";
				} else {
			walkAnim = "Walk";
			attackAnim = "Attack";
			hitAnim="Dead";
			deadAnim="Dead";
		}
		agent.SetDestination (currentDestination);
		animation.CrossFade (walkAnim);
		agent.speed += 5.0f;

		lastPos = transform.position;

	}

	void OnTriggerEnter(Collider collision){
		if (collision.tag == "person") {
			//Debug.Log("Kill!!!");
			animation.CrossFade (attackAnim);
			attackMode = true;
			agent.Stop();
		}
	}


	public void resetPath(Vector3 newLoc){
		agent.SetDestination (newLoc);
		agent.stoppingDistance = Random.Range (0, 10);
		agentReached = false;
	}

	// Update is called once per frame
	void Update () {
		if (attackMode == true) {
			if(switchTimer<=0.0f){
				switchTimer = 2.0f;
		    //Debug.Log("set to Walk");
			animation.CrossFade(walkAnim);
			attackMode = false;
			agent.Resume();
			}else{
				switchTimer -= Time.deltaTime;
			}
			return;
		}


		if (hitMode == true) {
			if(hitTimer<=0.0f){
				hitTimer = 2.0f;
				//Debug.Log("set to Walk");
				animation.CrossFade(hitAnim);
				hitMode = false;
				agent.Resume();
			}else{
				hitTimer -= Time.deltaTime;
			}
			return;
		}

		if (deadMode == true || health <= 0.0f) {
			deadMode = true;
			if(deadTimer<=0.0f){
				//deadTimer = 2.0f;
				//Debug.Log("set to Walk");
				animation.CrossFade(deadAnim);
				//deadMode = false;
				//agent.Resume();
			}else{
				deadTimer -= Time.deltaTime;
			}
			return;
		}
		/*
		if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid) {
			agentReached = true;
			return;
		} */

		lastCheck -= Time.fixedDeltaTime;
		if (lastCheck <= 0.0f && agentReached == false) {
			if((transform.position-lastPos).magnitude <= 0.5f){
				/*if(!agent.pathPending){
					Vector3 newTarget = new Vector3(transform.position.x+3.0f,transform.position.y, transform.position.z+2.0f);
					agent.SetDestination(newTarget);
				}*/
				
				//if(agent.path.status == NavMeshPathStatus.PathPartial
				float randDelta = Random.Range(-10,10);
				float randD = randDelta/8.0f;
				agent.Warp(new Vector3(transform.position.x-randD,transform.position.y,transform.position.z-randD));
				//Debug.Log(transform.name+" stuck "+transform.position+" "+lastPos);
				agentReached = true;
			}
			lastCheck = 1.0f;
			lastPos = transform.position;
		}

	}
}
