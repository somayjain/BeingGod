using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XP_handle : MonoBehaviour {

	public cursor_handle cursor;
	public GameObject faith;
	public GameObject fear;
	public Text XpText;
	public int XP_Limit, Faith, Fear;

	public float Inc_Time = 1.0f;
	
	private float Faith_ratio, Fear_ratio;
	private float Faith_min_ratio, Fear_min_ratio;
	private bool faith_updated, fear_updated;
	private float faith_time, fear_time;

	private int Faith_pool, Fear_pool;

	public float Faith_Degradation, Fear_Degradation;
	private float Faith_deg, Fear_deg;

	public bool LevelUpReached = true;

	// Use this for initialization
	void Start () {
		Faith_ratio = Faith / (float) XP_Limit;
		Fear_ratio = Fear / (float) XP_Limit;

		Faith_min_ratio = 0.0f;
		Fear_min_ratio = 0.0f;

		faith_time = 1.0f;
		fear_time = 1.0f;

		Faith_pool = 0;
		Fear_pool = 0;

		faith_updated = true;
		fear_updated = true;

		Faith_deg = 0.0f;
		Fear_deg = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		XpText.text = (Faith+Fear).ToString()+" / "+XP_Limit.ToString()+" XP";
		if (Faith_min_ratio + Fear_min_ratio >= 1.0f || XP_Limit == 0.0f) {
			LevelUpReached = true;
			return;
			// Level Up
		}

		if (faith_time > 0.0f) {
				Faith_min_ratio = Mathf.Lerp (Faith_min_ratio, Faith_ratio, Time.deltaTime / faith_time);
				faith_time = Mathf.Clamp (faith_time - Time.deltaTime, 0.0f, 10.0f);
		} else
				faith_updated = false;
		
		if (fear_time > 0.0f) {
				Fear_min_ratio = Mathf.Lerp (Fear_min_ratio, Fear_ratio, Time.deltaTime / fear_time);
				fear_time = Mathf.Clamp (fear_time - Time.deltaTime, 0.0f, 10.0f);
		} else
				fear_updated = false;

		if ( !faith_updated && !fear_updated && !LevelUpReached ) {
			if (Faith > 1) Faith_deg += Time.deltaTime * Faith_Degradation;
			else Faith_deg = 0.0f;
			if (Fear > 1) Fear_deg += Time.deltaTime * Fear_Degradation;
			else Fear_deg = 0.0f;

			if (Faith_deg > 1) {
				if (Faith_pool > 1)
					Faith_pool--;
				else if (Faith_pool == 0) {
					Faith--;
					Faith_ratio = Faith / (float) XP_Limit;
					Faith_min_ratio = Faith_ratio;
					faith_updated = true;
				}
				Faith_deg -= 1.0f;
			}

			if (Fear_deg > 1) {
				if (Fear_pool > 1)
					Fear_pool--;
				else if (Fear_pool == 0) {
					Fear--;
					Fear_ratio = Fear / (float) XP_Limit;
					Fear_min_ratio = Fear_ratio;
					fear_updated = true;
				}
				Fear_deg -= 1.0f;
			}
		}

		if (faith_updated)
			faith.GetComponent<RectTransform> ().anchorMax = new Vector2 (Faith_min_ratio, 1);
		if (fear_updated)
			fear.GetComponent<RectTransform> ().anchorMin = new Vector2 (1-Fear_min_ratio, 0);
	}

	public void AddXP (int xp, Powers.POWERTYPE powertype) {
		Debug.Log ("AddXP : " + LevelUpReached.ToString ());
		if (LevelUpReached)		return;

		Debug.Log ("Adding XP + " + xp + powertype.ToString ());

		switch (powertype) {
		case Powers.POWERTYPE.GOOD:
			if (Faith+Fear+xp > XP_Limit) {
				int faith_pool = Faith + Fear + xp - XP_Limit;
				Faith = Faith + xp - faith_pool;
				Faith_pool += faith_pool;
				Faith_ratio = Faith / (float) XP_Limit;
				faith_time += Inc_Time * (1 - faith_pool / (float)xp);
			} else {
				Faith += xp;
				Faith_ratio = Faith / (float) XP_Limit;
				faith_time += Inc_Time;
			}
			faith_updated = true;
			break;

		case Powers.POWERTYPE.EVIL:
			if (Faith+Fear+xp > XP_Limit) {
				int fear_pool = Faith + Fear + xp - XP_Limit;
				Fear = Fear + xp - fear_pool;
				Fear_pool += fear_pool;
				Fear_ratio = Fear / (float) XP_Limit;
				fear_time += Inc_Time * (1 - fear_pool / (float)xp);
			} else {
				Fear += xp;
				Fear_ratio = Fear / (float) XP_Limit;
				fear_time += Inc_Time;
			}
			fear_updated = true;
			break;

		case Powers.POWERTYPE.NEUTRAL:
			if (Faith+Fear+xp > XP_Limit) {
				Debug.Log("Here");
				int pool = (Faith + Fear + xp - XP_Limit) / 2;
				Faith_pool += pool;
				Fear_pool += pool;

				int unpool = xp - 2*pool;
				float faith_share = (Faith+Fear == 0 ? 0.5f : Faith / (float)(Faith+Fear) );
				Faith = Faith + Mathf.CeilToInt(unpool*faith_share);
				Fear = Fear + Mathf.CeilToInt(unpool*(1-faith_share));

				Faith_ratio = Faith / (float) XP_Limit;
				Fear_ratio = Fear / (float) XP_Limit;

				float time = (float) (0.5 * Inc_Time * unpool / (float)xp);
				faith_time += time;
				fear_time += time;
			} else {
				float faith_share = (Faith+Fear == 0 ? 0.5f : Faith / (float)(Faith+Fear) );
				Faith = Faith + Mathf.CeilToInt(xp*faith_share);
				Fear = Fear + xp - Mathf.CeilToInt(xp*faith_share);
				Debug.Log("fs="+faith_share.ToString()+", fa="+Mathf.CeilToInt(xp*faith_share).ToString()+", fe="+Mathf.FloorToInt(xp*(1-faith_share)).ToString());
				
				Faith_ratio = Faith / (float) XP_Limit;
				Fear_ratio = Fear / (float) XP_Limit;

				faith_time += Inc_Time/2.0f;
				fear_time += Inc_Time/2.0f;
			}
			faith_updated = true;
			fear_updated = true;

			Debug.Log (Faith.ToString()+" + "+ Fear.ToString() + " = " + XP_Limit.ToString());
			break;
		}
	}

	public void LevelUp ( int level ) {
//		if (!LevelUpReached) {
//			Debug.Log ("What are you doing ? I dont even have enuf XP for level " + level.ToString ());
//			Debug.Log ("My Faith: " + Faith.ToString () + " and Fear: " + Fear.ToString ());
//			return;
//		}
		switch (level) {
		case 1:
			XP_Limit = 0;
			//cursor.PowerFireball.Enable();
			cursor.PowerReach.Enable();
			break;

		case 2:
			XP_Limit = 50;

			cursor.PowerHey.Enable();
			cursor.PowerHey.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Hey");
			cursor.PowerHey.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);

			cursor.PowerBoo.Enable();
			cursor.PowerBoo.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Boo");
			cursor.PowerBoo.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);

			cursor.PowerGMBC.Enable();
			cursor.PowerGMBC.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("GMBC");
			cursor.PowerGMBC.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);

			cursor.PowerThunderClap.Enable();
			cursor.PowerThunderClap.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Thunderclap");
			cursor.PowerThunderClap.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);
			break;

		case 3:
			XP_Limit = 100;

			cursor.PowerMjolnir.Enable();
			cursor.PowerMjolnir.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Lightning");
			cursor.PowerMjolnir.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);

			cursor.PowerFireball.Enable();
			cursor.PowerFireball.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Fireball");
			cursor.PowerFireball.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);

			cursor.PowerTornado.Enable();
			cursor.PowerTornado.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Tornado");
			cursor.PowerTornado.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);
			break;

		case 4:
			XP_Limit = 500;

			cursor.PowerReach.Enable();
//			cursor.PowerReach.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Reach");
			cursor.PowerReach.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);
			break;

		case 5:
			XP_Limit = 1000;

			cursor.PowerHoG.Enable();
			cursor.PowerHoG.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("HoG");
			cursor.PowerHoG.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(45f,45f);
			break;
		}

		if (Faith_pool > 0) {
			Faith = Faith_pool;
			Faith_ratio = Faith / XP_Limit;
			Faith_min_ratio = 0.0f;
			faith_time = 0.5f;
			faith_updated = true;
			Faith_pool = 0;
			Faith_deg = 0.0f;
		} else {
			Faith = 0;
			Faith_ratio = 0.0f;
			Faith_min_ratio = 0.0f;
			faith_time = 0.0f;
			faith_updated = true;
			Faith_deg = 0.0f;
			faith.GetComponent<RectTransform> ().anchorMax = new Vector2 (Faith_min_ratio, 1);
		}
		
		if (Fear_pool > 0) {
			Fear = Fear_pool;
			Fear_ratio = Fear / XP_Limit;
			Fear_min_ratio = 0.0f;
			fear_time = 0.5f;
			fear_updated = true;
			Fear_pool = 0;
			Faith_deg = 0.0f;
		} else {
			Fear = 0;
			Fear_ratio = 0.0f;
			Fear_min_ratio = 0.0f;
			fear_time = 0.0f;
			fear_updated = true;
			Fear_deg = 0.0f;
			fear.GetComponent<RectTransform> ().anchorMin = new Vector2 (1-Fear_min_ratio, 0);
		}
		
		LevelUpReached = false;
	}
}
