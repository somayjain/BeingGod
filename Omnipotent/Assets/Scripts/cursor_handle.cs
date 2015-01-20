using UnityEngine;
using System.Collections;

public class cursor_handle : MonoBehaviour {

	public Camera camera;
	public GameObject sphere;

	private bool onShelf = false;
	private int build = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			sphere.transform.position = new Vector3(hit.point.x, hit.point.y + 1.0f, hit.point.z);
		}
//		Instantiate (object, mousePo, Quaternion.identity);
	}

	public void EnterShelf () {
		onShelf = true;
	}
	public void ExitShelf () {
		onShelf = false;
	}
	public bool isOnShelf () {
		return onShelf;
	}

	public void Build(int build_id) {
		build = build_id;
	}
}
