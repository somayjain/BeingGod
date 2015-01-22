using UnityEngine;
using System.Collections;

public class NavAgentMovement : MonoBehaviour {

	public Vector3 target;
	public NavMeshAgent agent;
	private bool switchLoc = false;
	public bool targetReached = false;
	public float lastTimeUpdate = 0.0f;
	public Vector3 lastLocUpdate;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination(target);
		lastLocUpdate = agent.transform.position;
	}

	public void setNewPath(Vector3 targetLoc){
		agent.SetDestination(targetLoc);
		targetReached = false;
	}

	// Update is called once per frame
	void Update () {

		lastTimeUpdate += Time.fixedDeltaTime;


		if (!agent.pathPending)
		{
			//Debug.Log("Stopped1");
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				//Debug.Log("Stopped2");
				if (agent.velocity.sqrMagnitude == 0f)
				{
					Debug.Log("Stopped");
					targetReached = true;
				}
			}
		}


	}
}
