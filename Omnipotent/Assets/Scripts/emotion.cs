using UnityEngine;
using System.Collections;
using RSUnityToolkit;


public class emotion : MonoBehaviour {
	
	private string[] EmotionLabels = {"ANGER","CONTEMPT","DISGUST","FEAR","JOY","SADNESS","SURPRISE", "NONE", "NONE", "NONE"};
	private string[] SentimentLabels = {"NEGATIVE","POSITIVE","NEUTRAL"};
	
	public int NUM_EMOTIONS  = 10;
	public int NUM_PRIMARY_EMOTIONS = 7;
	
	public Power_GMBC controlweather;
	// Use this for initialization
	void Start () {
		
	}
	
	private void DrawLocation (PXCMEmotion.EmotionData[] data)
	{
		bool emotionPresent = false;
		int epidx = -1; int maxscoreE = -3; float maxscoreI = 0;
		for (int i = 0; i < NUM_PRIMARY_EMOTIONS; i++)
		{
			if (data[i].evidence  < maxscoreE) continue;
			if (data[i].intensity < maxscoreI) continue;
			maxscoreE = data[i].evidence;
			maxscoreI = data[i].intensity;
			epidx = i;
		}
		if ((epidx != -1) && (maxscoreI > 0.4))
		{
			//Debug.Log(EmotionLabels[epidx]);
			emotionPresent = true;
		}
		
		int spidx = -1;
		if (emotionPresent)
		{
			maxscoreE = -3; maxscoreI = 0;
			for (int i = 0; i < (NUM_EMOTIONS - NUM_PRIMARY_EMOTIONS); i++)
			{
				if (data[NUM_PRIMARY_EMOTIONS + i].evidence  < maxscoreE) continue;
				if (data[NUM_PRIMARY_EMOTIONS + i].intensity < maxscoreI) continue;
				maxscoreE = data[NUM_PRIMARY_EMOTIONS + i].evidence;
				maxscoreI = data[NUM_PRIMARY_EMOTIONS + i].intensity;
				spidx = i;
			}
			if ((spidx != -1))
			{
				Debug.Log(SentimentLabels[spidx]);		
				if (EmotionLabels [epidx] == "ANGER" && SentimentLabels [spidx] == "NEGATIVE")
					controlweather.EnableRain ();

				else if (EmotionLabels [epidx] == "JOY" && SentimentLabels [spidx] == "POSITIVE")
					controlweather.EnableSnow ( );
				
				else
					controlweather.Deactivate ();
			}
		}
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log ("1");
		
		PXCMEmotion ft = SenseToolkitManager.Instance.Emotion;
		//int numfaces = ft.QueryNumFaces ();
		//Debug.Log (numfaces.ToString);
		if ( ft != null) {
			
			//			//GZ DisplayPicture(pp.QueryImageByType(PXCMImage.ImageType.IMAGE_TYPE_COLOR));
			//			PXCMCapture.Sample sample = stkm.SenseManager.QueryEmotionSample ();
			//			if (sample == null) {
			//				stkm.SenseManager.ReleaseFrame ();
			//				Debug.Log ("3");
			//				return;
			//			}
			
			//Debug.Log ("4");
			PXCMEmotion.EmotionData[] arrData = new PXCMEmotion.EmotionData[NUM_EMOTIONS];
			if (ft.QueryAllEmotionData (0, out arrData) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
				DrawLocation (arrData);
		}
	}

	public void dummyFaceForEmotion (){
	}
}
