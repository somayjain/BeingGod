using UnityEngine;
using System.Collections;

public class planeController : MonoBehaviour {
	public AudioClip a;
	public GameObject x, fire;
	//GameObject xnew, fire;
	Vector3 posx = new Vector3(0.0f, -1.85f, 0.0f);
	Vector3 posf = new Vector3(0.0f, 11.0f, -2.0f);
	void Start () {
		//x = GameObject.FindWithTag("Explosion");
		//x.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "FireBALL")
		{	//x = GameObject.FindWithTag("Explosion");
			fire = Instantiate(Resources.Load("FireBall")) as GameObject;
			x = Instantiate(Resources.Load("Small Explosion")) as GameObject;
			//xnew = Instantiate(x, posx, Quaternion.identity) as GameObject;
			//fire = Instantiate(other.gameObject, posf, Quaternion.identity) as GameObject;
			//xnew.tag = "Explosion";
			//xnew.SetActive(false);
			//fire.tag = "FireBALL";
			//fire.rigidbody.useGravity = false;
			//fire.SetActive(false);
			audio.PlayOneShot(a);
			x.SetActive(true);
			//other.gameObject.SetActive(false);
			//other.gameObject.transform.position = posf;
			//other.gameObject.rigidbody.useGravity = false;
			DestroyObject(other.gameObject);
			StartCoroutine(W2SnD());	//Wait for 2 Seconds and Destroy
			//x = xnew;
		}
	}
	IEnumerator W2SnD()
	{	yield return new WaitForSeconds(2);
		DestroyObject(x);
	}
}
