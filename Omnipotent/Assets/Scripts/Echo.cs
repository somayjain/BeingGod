using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class Echo : MonoBehaviour {
	/*
	private char[] databuff = new char[100000];
	private int num_samples = 0;
	private int bits_per_sample = 16;
	private AudioClip echoclip;
	private PXCMAudio.AudioInfo audioinfo;

	// Use this for initialization
	void Start () {
//		databuff = new char [100000];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("a")) {
				int size = (int)(num_samples * bits_per_sample / 8.0f);
				int nsamples = SenseToolkitManager.Instance.SpeechManager.abc ((byte[])(databuff), size, audioinfo);
				num_samples += nsamples;
		} else if (Input.GetKeyUp ("p")) {
				echoclip = AudioClip.Create("MyEcho", num_samples, audioinfo.nchannels, audioinfo.sampleRate, false, true);
				echoclip.SetData((float[])(databuff),0);
				GetComponent<AudioSource>().clip = echoclip;
			GetComponent<AudioSource>().PlayOneShot(echoclip);
		}
	}
	*/
}
