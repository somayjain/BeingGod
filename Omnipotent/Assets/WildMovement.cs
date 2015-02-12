using UnityEngine;
using System.Collections;

public class WildMovement : MonoBehaviour {


	public Vector3 currentDestination; 
	public NavMeshAgent agent;

	public float health = 50.0f;

	public bool attackMode = false;
	public float switchTimer = 2.0f;
	public bool agentReached = false;

	string attackAnim;
	string walkAnim;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		if (gameObject.name == "Dino") {
						walkAnim = "Allosaurus_Run";
						attackAnim = "Allosaurus_Attack02";
				} else {
			walkAnim = "Walk";
			attackAnim = "Attack";
		}
		agent.SetDestination (currentDestination);
		animation.CrossFade (walkAnim);
		agent.speed += 5.0f;
	}

	void OnTriggerEnter(Collider collision){
		if (collision.tag == "person") {
			Debug.Log("Kill!!!");
			animation.CrossFade (attackAnim);
			attackMode = true;
			agent.Stop();
		}
	}


	public void resetPath(Vector3 newLoc){
		agent.SetDestination (newLoc);
		agentReached = true;
	}

	// Update is called once per frame
	void Update () {
		if (attackMode == true) {
			if(switchTimer<=0.0f){
				switchTimer = 2.0f;
		    Debug.Log("set to Walk");
			animation.CrossFade(walkAnim);
			attackMode = false;
			agent.Resume();
			}else{
				switchTimer -= Time.fixedDeltaTime;
			}
		}
		if (!agent.pathPending){
			agentReached = true;
		}
	}
}
