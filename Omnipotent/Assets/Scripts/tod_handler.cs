using UnityEngine;
using System.Collections;

public class tod_handler : MonoBehaviour {

	public GameObject sun;
	public GameObject moon;
	[Range(0, 360)]
	public float angle;
	public float radius;
	public float game_mins_per_second = 10;

	private float time_ratio;

	// Use this for initialization
	void Start () {
		time_ratio = 360 / (24 * 60 / game_mins_per_second);
	}
	
	// Update is called once per frame
	void Update () {
		float delta_angle = (float) (Time.deltaTime * time_ratio);
		angle += delta_angle;
		float sun_angle = angle * (float) Mathf.PI / (float)180.0;
		float moon_angle = (180 + angle) * (float) Mathf.PI / (float)180.0;

		Rect sun_rect = sun.GetComponent<RectTransform> ().rect;
		sun.GetComponent<RectTransform> ().anchoredPosition = new Vector2(radius * Mathf.Cos (sun_angle), radius * Mathf.Sin (sun_angle));
		moon.GetComponent<RectTransform> ().anchoredPosition = new Vector2(radius * Mathf.Cos (moon_angle), radius * Mathf.Sin (moon_angle));
	}
}
