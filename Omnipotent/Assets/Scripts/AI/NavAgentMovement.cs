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

		if (!agent.pathPending)
		{
			//Debug.Log("Stopped1");
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				//Debug.Log("Stopped2");
				if (agent.velocity.sqrMagnitude <= 0.1f)
				{
					//comments
				//	Debug.Log("Stopped");
					targetReached = true;
				}
			}
		}


	}
}
