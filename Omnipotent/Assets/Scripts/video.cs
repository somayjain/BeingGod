using UnityEngine;
using System.Collections;

public class video : MonoBehaviour {

	public MovieTexture movTexture;
	//public AudioClip audio;
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = movTexture;
		//AudioSource.PlayClipAtPoint (audio, transform.position);
		audio.Play ();
		movTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if ( !movTexture.isPlaying )
			Application.LoadLevel(1);
	}
}
