using UnityEngine;
using System.Collections;

public class objectController : MonoBehaviour {
	public bool rainStatus = true;
	public bool snowStatus = true;
	public bool lightningStatus = true;
	public bool fireballStatus = true;
	public bool twisterStatus = true;
	public bool tUpStatus = true;
	public bool tDownStatus = true;
	public bool cloudStatus = true;
	//public bool explosionStatus = false;
	//public GameObject[] arr;
	public GameObject rain, light, snow, fire, tor, tup, tdown, clouds;
	// Use this for initialization
	void Start () {
//		arr = new GameObject[numObjs];
//		arr[0] = GameObject.FindWithTag("RainParticles");
//		arr[1] = GameObject.FindWithTag("Snow");
//		arr[2] = GameObject.FindWithTag("Lightning");
//		arr[3] = GameObject.FindWithTag("FireBALL");
//		arr[4] = GameObject.FindWithTag("Tornado");
//		arr[5] = GameObject.FindWithTag("TUP");
//		arr[6] = GameObject.FindWithTag("TDOWN");
//		arr[7] = GameObject.FindWithTag("Explosion");
//		arr[8] = GameObject.FindWithTag("Clouds");
//		for(int i=0; i<numObjs; i++)
//			arr[i].SetActive(false);
		rain = GameObject.FindWithTag("RainParticles");
		snow = GameObject.FindWithTag("Snow");
		light = GameObject.FindWithTag("Lightning");
		fire = GameObject.FindWithTag("FireBALL");
		tor = GameObject.FindWithTag("Tornado");
		tup = GameObject.FindWithTag("TUP");
		tdown = GameObject.FindWithTag("TDOWN");
		clouds = GameObject.FindWithTag("Clouds");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("r"))
		{	rainStatus = !rainStatus;
			rain.SetActive(rainStatus);
		}
		if(Input.GetKeyUp("s"))
		{	snowStatus = !snowStatus;
			snow.SetActive(snowStatus);
		}
		if(Input.GetKeyUp("c"))
		{	cloudStatus = !cloudStatus;
			clouds.SetActive(cloudStatus);
		}
		if(Input.GetKeyUp("t"))
		{	twisterStatus = !twisterStatus;
			tUpStatus = !tUpStatus;
			tDownStatus = !tDownStatus;
			tor.SetActive(twisterStatus);
			tup.SetActive(tUpStatus);
			tdown.SetActive(tDownStatus);
		}
		if(Input.GetKeyUp("l"))
		{	lightningStatus = !lightningStatus;
			light.SetActive(lightningStatus);
		}
	}
}
