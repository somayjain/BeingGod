using UnityEngine;
using System.Collections;


public class fireball : MonoBehaviour {
	public bool gravity = false;
	AudioSource audio;
	// Use this for initialization
	void Start () {
		//myPlane.transform.position;
		rigidbody.useGravity = gravity;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("f"))
		{	gravity = !gravity;
			rigidbody.useGravity = gravity;
		}
//		Type = myPlane.GetType();
		/*void OnTriggerEnter(Collider other) 
		{
			if(other.gameObject.tag == "Pickup")
			{
				other.gameObject.SetActive(false);
				count=count+1;
				if(count==1)
				{
					winText.text="YOU WIN !!\n Press Enter to Restart OR Escape to Quit OR TAB to go to Next Level";
					if(levelno==2)
					{	levelMesg.text="YOU HAVE FINISHED THE GAME.........";
					}
				}
			}
		}*/
	}
}
