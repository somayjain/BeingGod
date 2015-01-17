using UnityEngine;
using System.Collections;

public class detectedGesture_handle : MonoBehaviour {

	private int id = 1;
	private int count = 0;
	public GameObject image;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		count++;
		if (count >= 100) {
			count = 0;
			id = ((id + 1) % 28) + 1;
			id.ToString();

			Sprite newsprite
				= Resources.Load ("gestures_"+id.ToString(), typeof(Sprite)) as Sprite;

//			image.GetComponent<ScriptableObject>().sprite = newsprite;
		}
	}
}
