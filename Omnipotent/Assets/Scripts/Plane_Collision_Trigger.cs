using UnityEngine;
using System.Collections;

public class Plane_Collision_Trigger : MonoBehaviour {

	public AudioClip audioclip;
	private GameObject Explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "FireBALL")
		{
			Explosion = Instantiate(Resources.Load("Small Explosion"), other.transform.parent.position + new Vector3(0,1,0), Quaternion.identity) as GameObject;
			Explosion.name = "Explosion";
			Explosion.transform.parent = other.transform.parent;
			audio.PlayOneShot(audioclip);
			
//			DestroyObject(other.gameObject);
			StartCoroutine(W2SnD(other.gameObject));	//Wait for 2 Seconds and Destroy
		}
	}
	
	IEnumerator W2SnD(GameObject trail)
	{
		yield return new WaitForSeconds(3);
		Explosion.GetComponent<ParticleEmitter> ().emit = false;
		DestroyObject(trail);
	}
}
