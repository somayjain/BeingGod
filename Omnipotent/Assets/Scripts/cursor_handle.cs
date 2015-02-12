using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class cursor_handle : MonoBehaviour {

	public Camera camera;

	public Vector3 cursor3d;
	public GameObject cursor;

	[Header("Cursors")]
	public Texture2D normal;
	public Texture2D closed;
	public Texture2D thunderclap_cursor;
	public Texture2D mjolnir_cursor;
	public Texture2D fireball_cursor;
	public Texture2D tornado_cursor;

	[Header("")]
	public GameObject sphere;

	private bool onShelf = false;
	private bool onHUD = false;

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
		TORNADO,
		HEY,
		BOO,
		HOG,
		PAN,
		ZOOM,
		ROTATE
	}
	public MODE mode = MODE.DEFAULT;

	[Header("Powers")]
	public Power_ThunderClap PowerThunderClap;
	public Power_Mjolnir PowerMjolnir;
	public Power_Fireball PowerFireball;
	public Power_Tornado PowerTornado;
	public Power_HoG PowerHoG;
	public Power_Hey PowerHey;
	public Power_Boo PowerBoo;

	public GameObject PeopleManager;
	public GameObject currentLevel;
	// Use this for initialization
	void Start () {
		PeopleManager = GameObject.FindGameObjectWithTag("PeopleManager");
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 handpos = GetComponentInChildren<HandRenderer> ().queryLeftHand2DCoordinates ();
		if (handpos.x == -1 && handpos.y == -1) {
						cursor.SetActive (false);
				} else {
						cursor.SetActive (true);
						cursor.GetComponent<RectTransform> ().position = new Vector3 (handpos.x, handpos.y, 0);
				}
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			sphere.transform.position = new Vector3(hit.point.x, hit.point.y + 1.0f, hit.point.z);
			cursor3d = hit.point;
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

		case MODE.THUNDER_CLAP:
			Cursor.SetCursor (thunderclap_cursor, Vector2.zero, CursorMode.Auto);
			if ( !isOnHUD() && !isOnShelf() ) {
				if (Input.GetMouseButtonUp(0)) {
					// Trigger Thunder Clap
//					PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.THUNDER_CLAP;
//					PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
					PowerThunderClap.Trigger( cursor3d );
					setMode (MODE.DEFAULT);
				}
			}
			break;

		case MODE.WINDY:
			break;
		case MODE.GMBC:
			break;
		case MODE.MJOLNIR:
			Cursor.SetCursor (mjolnir_cursor, Vector2.zero, CursorMode.Auto);
			if ( !isOnHUD() && !isOnShelf() ) {
				if (Input.GetMouseButtonUp(0)) {
					// Trigger Mjolnir at cursor3d			
					/*
					 * PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.MJOLNIR;
					PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
					if(currentLevel != null){
						currentLevel.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.MJOLNIR;
						currentLevel.GetComponent<ZombieManager>().hit3DLoc = cursor3d;
					} */

					PowerMjolnir.Trigger( cursor3d );
					setMode (MODE.DEFAULT);
				}
			}
			break;
		
		case MODE.FIREBALL:
			Cursor.SetCursor (fireball_cursor, Vector2.zero, CursorMode.Auto);
			if ( !isOnHUD() && !isOnShelf() ) {
				if (Input.GetMouseButtonUp(0)) {
					// Trigger Fireball at cursor3d
//					PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.FIREBALL;
//					PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
//					if(currentLevel != null){
//						currentLevel.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.FIREBALL;
//						currentLevel.GetComponent<ZombieManager>().hit3DLoc = cursor3d;
//					}
					PowerFireball.Trigger( cursor3d );
					setMode (MODE.DEFAULT);
				}
			}
			break;

		case MODE.TORNADO:
			Cursor.SetCursor (tornado_cursor, Vector2.zero, CursorMode.Auto);
			if ( !isOnHUD() && !isOnShelf() ) {
				if (Input.GetMouseButtonUp(0)) {
					// Trigger Tornado at cursor3d
//					PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.TORNADO;
//					PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
//					if(currentLevel != null){
//						currentLevel.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.TORNADO;
//						currentLevel.GetComponent<ZombieManager>().hit3DLoc = cursor3d;
//					}
					PowerTornado.Trigger( cursor3d );
					setMode (MODE.DEFAULT);
				}
			}
			break;

		case MODE.BOO:
			PowerBoo.Trigger ( cursor3d );
			setMode (MODE.DEFAULT);
			break;
		
		case MODE.HEY:
			PowerHey.Trigger ( cursor3d );
			setMode (MODE.DEFAULT);
			break;

		case MODE.HOG:
			Cursor.SetCursor (tornado_cursor, Vector2.zero, CursorMode.Auto);
			if ( !isOnHUD() && !isOnShelf() ) {
				if (Input.GetMouseButtonUp(0)) {
					PowerHoG.Trigger( cursor3d );
					setMode (MODE.DEFAULT);
				}
			}
			break;

		}
	}

	public void OnShelf () {
		onShelf = true;
	}
	public void OffShelf () {
		onShelf = false;
	}
	public bool isOnShelf () {
		return onShelf;
	}

	public void OnHUD () {
		onHUD = true;
	}
	public void OffHUD () {
		onHUD = false;
	}
	public bool isOnHUD () {
		return onHUD;
	}

	public void Build(BUILD build_mode) {
		build = build_mode;
	}

	public void setMode(MODE _mode) {
		mode = _mode;
		if(mode == MODE.DEFAULT)
			Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
	}

	public void OnPress () {
		Cursor.SetCursor (closed, Vector2.zero, CursorMode.Auto);
	}
	public void OnRelease () {
		Cursor.SetCursor (normal, Vector2.zero, CursorMode.Auto);
	}
	public void TriggerPower (Trigger trgr){
		Debug.Log ("Hand Closed, message received");
		switch (mode) {
				case MODE.MJOLNIR:
						PowerMjolnir.Trigger (cursor3d);
						setMode (MODE.DEFAULT);
						break;
				}
	}
	public void soundFire(Trigger trgr){
		if (!isOnHUD () && !isOnShelf ()) {
			setMode (MODE.FIREBALL);
			PowerFireball.Trigger (cursor3d);
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard Fire");
	}
	public void soundClap(Trigger trgr){
		setMode (MODE.THUNDER_CLAP);
		if ( !isOnHUD() && !isOnShelf() ) {
			// Trigger Thunder Clap
			PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.THUNDER_CLAP;
			PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
			PowerThunderClap.Trigger( cursor3d );
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard Clap");
		setMode (MODE.DEFAULT);
	}
	public void soundBolt(Trigger trgr){
		setMode (MODE.MJOLNIR);
		if (!isOnHUD () && !isOnShelf ()) {
			// Trigger Mjolnir at cursor3d			
//			PeopleManager.GetComponent<LoadVoxelPeople> ().Powermode = LoadVoxelPeople.MODE.MJOLNIR;
//			PeopleManager.GetComponent<LoadVoxelPeople> ().pointOfContact = cursor3d;
//			if (currentLevel != null) {
//				currentLevel.GetComponent<ZombieManager> ().Powermode = ZombieManager.MODE.MJOLNIR;
//				currentLevel.GetComponent<ZombieManager> ().hit3DLoc = cursor3d;
//			}
			PowerMjolnir.Trigger (cursor3d);
		}
		setMode (MODE.DEFAULT);
		Debug.Log ("I heard bolt");
	}
	public void soundTornado(Trigger trgr) {
		if ( !isOnHUD() && !isOnShelf() ) {
			setMode(MODE.TORNADO);
			// Trigger Tornado at cursor3d
//			PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.TORNADO;
//			PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
//			if(currentLevel != null){
//				currentLevel.GetComponent<ZombieManager>().Powermode = ZombieManager.MODE.TORNADO;
//				currentLevel.GetComponent<ZombieManager>().hit3DLoc = cursor3d;
//			}
			PowerTornado.Trigger( cursor3d );
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard Tornado");
	}
	public void HandDetected (Trigger trgr) {

	}
}
