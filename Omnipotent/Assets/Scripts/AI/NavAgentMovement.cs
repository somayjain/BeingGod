using UnityEngine;
using System.Collections;

public class NavAgentMovement : MonoBehaviour {

	public Vector3 target;
	public NavMeshAgent agent;
	private bool switchLoc = false;
	public bool targetReached = false;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination(target);
		agent.stoppingDistance = Random.Range (0, 10);
	}

	public void setNewPath(Vector3 targetLoc){
		agent.SetDestination(targetLoc);
		targetReached = false;
	}

	// Update is called once per frame
	void Update () {



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
