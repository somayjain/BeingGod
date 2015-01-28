using UnityEngine;
using System.Collections;

public class cursor_handle : MonoBehaviour {

	public Camera camera;

	[Header("Cursors")]
	public Texture2D normal;
	public Texture2D closed;
	public Texture2D fireball_cursor;

	public GameObject sphere;

	private bool onShelf = false;

	public enum BUILD {
		NONE,
		HOUSE,
		HOSPITAL,
		BUSSTOP
	};

//	private int build = 0;
	private BUILD build = BUILD.NONE;

	/* Modes:
	 * 0 - Default (Pan/Rotate/Zoom)
	 * 1 - Build
	 * 2 - 
	 * 3 - 
	 * 4 - 
	 * 5 - 
	 * 6 - 
	 */
	public enum MODE {
		DEFAULT,
		BUILD,
		THUNDER_CLAP,
		WINDY,
		GMBC,
		MJOLNIR,
		FIREBALL,
		TORNADO
	}
	public MODE mode = MODE.DEFAULT;

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

		switch (mode) {
		case MODE.DEFAULT:
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2))
				Cursor.SetCursor (closed, Vector2.zero, CursorMode.Auto);
			else if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2))
				Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
			break;

		case MODE.BUILD:
			if (Input.GetMouseButtonDown (0))
				// create object and place at mouse posisition
			{}
			Debug.Log (build.ToString ());
			if (Input.GetMouseButtonUp (0) && build != BUILD.NONE)
			{
				Debug.Log ("Build done");
				// Place the object;
			}

			break;

		case MODE.FIREBALL:
			Cursor.SetCursor (fireball_cursor, Vector2.zero, CursorMode.Auto);
			if (Input.GetMouseButtonUp(2))
			{
				// Get position
				// place animation at position
				setMode(MODE.DEFAULT);
				Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
			}
			break;
		}
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

	public void Build(BUILD build_mode) {
		build = build_mode;
	}

	public void setMode(MODE _mode) {
			mode = _mode;
	}

	public void OnPress () {
		Cursor.SetCursor (closed, Vector2.zero, CursorMode.Auto);
	}
	public void OnRelease () {
		Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
	}
}
