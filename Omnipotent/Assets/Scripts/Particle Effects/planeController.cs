using UnityEngine;
using System.Collections;

public class planeController : MonoBehaviour {
	public AudioClip a;
	public GameObject x;
	//private float emissionval = 200;
	// Use this for initialization
	void Start () {
		x = GameObject.FindWithTag("Explosion");
		x.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "FireBALL")
		{	//Destroy(other.gameObject);
			audio.PlayOneShot(a);
			x.SetActive(true);
			other.gameObject.SetActive(false);
			StartCoroutine(W2SnD());	//Wait for 2 Seconds and Destroy
		}
	}
	IEnumerator W2SnD()
	{	yield return new WaitForSeconds(2);
		/*emissionval -= 100*Time.deltaTime;
		x.GetComponent<ParticleEmitter>().maxEmission = emissionval;
		if(emissionval<=0)
			DestroyObject(x);*/
		DestroyObject(x);
	}
}
