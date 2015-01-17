using UnityEngine;
using System.Collections;

public class NavAgentMovement : MonoBehaviour {

	public Vector3 target;
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		target = new Vector3 (0, 0, 0);
		agent.SetDestination(target);
	}
	
	// Update is called once per frame
	void Update () {

		if (!agent.pathPending)
		{
			//Debug.Log("Stopped1");
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				//Debug.Log("Stopped2");
				if (agent.velocity.sqrMagnitude == 0f)
				{
					Debug.Log("Stopped3");
					target = new Vector3(Random.Range(-5,5),0.5f,Random.Range(-5,5));
					agent.SetDestination(target);
				}
			}
		}
	}
}
