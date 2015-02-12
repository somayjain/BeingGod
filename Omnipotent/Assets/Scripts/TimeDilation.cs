using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RSUnityToolkit;

public class TimeDilation : MonoBehaviour {

	private float last_scale = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float scale = (float) GetComponent<Slider> ().value * 0.1f;
		if (scale != last_scale) {
				if (scale > 1.0f)		scale = 9 * scale - 8.0f;
				Time.timeScale = scale;
				last_scale = scale;
		}
	}

	// used by SendMessage components
	public void increaseSpeed(Trigger trgr) {
		float speed = (float)GetComponent<Slider> ().value;
		if(speed + 1.0f <= 20)
			GetComponent<Slider> ().value = speed + 1.0f;
		Debug.Log ("increase to " + speed);
	}

	public void decreaseSpeed(Trigger trgr) {
		float speed = (float)GetComponent<Slider> ().value;
		if(speed - 1.0f > 0)
			GetComponent<Slider> ().value = speed - 1.0f;
		Debug.Log ("decrease to " + speed);
	}
}
