using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Power_GMBC : Powers {

	public GameObject rain;
	public GameObject snow;

	private bool emit = false;

	// Use this for initialization
	void Start () {
		enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (cursor.mode == cursor_handle.MODE.BUILD)
			GetComponent<Button> ().interactable = false;
		else if (cursor.mode == cursor_handle.MODE.DEFAULT)
			GetComponent<Button> ().interactable = true;

		if ( !enabled )	return;
	}
	// ANGER NEGATIVE
	public void EnableRain () {
		if ( !enabled ) return;
		Levelcontroller.Powermode = LevelController.MODE.GMBC;
		snow.GetComponent<ParticleEmitter>().emit = false;
		rain.GetComponent<ParticleEmitter>().emit = true;
	}
	// JOY + POSITIVE
	public void EnableSnow () {
		if ( !enabled ) return;
		Levelcontroller.Powermode = LevelController.MODE.GMBC;
		rain.GetComponent<ParticleEmitter>().emit = false;
		snow.GetComponent<ParticleEmitter>().emit = true;
	}
	// NEUTRAL
	public void Deactivate () {
		if ( !enabled ) return;
		rain.GetComponent<ParticleEmitter>().emit = false;
		snow.GetComponent<ParticleEmitter>().emit = false;
	}

	public void OnClick () {
		cursor.setMode (cursor_handle.MODE.GMBC);
	}
}
