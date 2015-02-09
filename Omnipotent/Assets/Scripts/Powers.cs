using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {

	public enum POWERTYPE
	{
		GOOD, 
		NEUTRAL, 
		EVIL
	};

	public XP_handle XP;
	public cursor_handle cursor;

	public float Cooldown = 10.0f;
	public float CastTime = 3.0f;
	
	protected float time_left = 0.0f;
	protected bool refresh = true;
	protected bool active = false;
	
	public POWERTYPE PowerType = POWERTYPE.NEUTRAL;
	public int XP_per_NPC = 1;
	
	protected bool enabled = false;

	public void Enable () {
		enabled = true;
		
		// Change the power icon.
	}
	public void Disable () {
		enabled = false;
		
		// Change the Power icon.
	}

	protected Vector3 location;

	public Vector3 Location () {
		return location;
	}

	protected void UpdateLast () {
		if (!refresh) {
			time_left -= Time.deltaTime;
			
			Vector2 size = GetComponent<RectTransform> ().sizeDelta;
			size.x *= time_left / Cooldown;
			transform.GetChild (1).GetComponent<RectTransform> ().sizeDelta = size;
		}
	}
}
