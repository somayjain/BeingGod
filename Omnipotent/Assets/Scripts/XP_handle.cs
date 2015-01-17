using UnityEngine;
using System.Collections;

public class XP_handle : MonoBehaviour {

	public GameObject faith;
	public GameObject fear;
	public int XP_limit, faith_val, fear_val;
//	public Vector3 level[];

	// Use this for initialization
	void Start () {
		float faith_ratio = faith_val / (float) XP_limit;
		float fear_ratio = fear_val / (float) XP_limit;
		RectTransform faith_rt = faith.GetComponent<RectTransform> ();
		faith_rt.anchorMax.Set (faith_ratio, 1);
//		faith.GetComponent(RectTransform).anchorMax.x = faith_ratio;
//		fear.RectTransform.anchorMin.x = 1 - fear_ratio;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
