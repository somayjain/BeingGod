using UnityEngine;
using System.Collections;

public class cursor_handle : MonoBehaviour {

	public Camera camera;
	[Header("Cursors")]
	public Texture2D normal;
	public Texture2D closed;

	public GameObject sphere;

	private bool onShelf = false;
	private int build = 0;

	public int mode = 0;

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
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2))
						Cursor.SetCursor (closed, Vector2.zero, CursorMode.Auto);
		else if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2))
						Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
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

	public void setMode(int _mode) {
			mode = _mode;
	}

	public void OnPress () {
		Cursor.SetCursor (closed, Vector2.zero, CursorMode.Auto);
	}
	public void OnRelease () {
		Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
	}
}
