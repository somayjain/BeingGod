using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class cursor_handle : MonoBehaviour {

	public Camera camera;

	public Vector3 cursor3d;
	public Vector2 cursor2d;
	public GameObject cursor;

	[Header("Cursors")]
	public Texture2D normal;
	public Texture2D closed;
	public Texture2D thunderclap_cursor;
	public Texture2D mjolnir_cursor;
	public Texture2D fireball_cursor;
	public Texture2D tornado_cursor;

	public enum HAND_SIDES {
		Left,
		Right
	};

	[Header("")]
	public GameObject sphere;
	public HAND_SIDES PreferredHand = HAND_SIDES.Left;

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
	public Power_GMBC PowerGMBC;

	public GameObject PeopleManager;
	public GameObject currentLevel;
	// Use this for initialization
	void Start () {
		PeopleManager = GameObject.FindGameObjectWithTag("PeopleManager");
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray;
		bool hand2d = false;

		if (PreferredHand == HAND_SIDES.Right) {
						hand2d = GetComponentInChildren<HandRenderer> ().queryRightHand2DCoordinates (out cursor2d);
						if (!hand2d)
								hand2d = GetComponentInChildren<HandRenderer> ().queryLeftHand2DCoordinates (out cursor2d);
				} else {
						hand2d = GetComponentInChildren<HandRenderer> ().queryLeftHand2DCoordinates (out cursor2d);
						if (!hand2d)
							hand2d = GetComponentInChildren<HandRenderer> ().queryRightHand2DCoordinates (out cursor2d);
				}

		if (!hand2d) {
				cursor.SetActive (false);
				cursor2d = Input.mousePosition;
				ray = camera.ScreenPointToRay (Input.mousePosition);
		} else {
				cursor.SetActive (true);
				cursor.GetComponent<RectTransform> ().position = cursor2d;
				ray = camera.ScreenPointToRay (cursor2d);
		}

		RaycastHit hit;
		int terrainlayermask = 1 << 9;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, terrainlayermask)) {
			sphere.transform.position = new Vector3 (hit.point.x, hit.point.y + 1.0f, hit.point.z);
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

	public bool setMode(MODE _mode) {
		if (mode == MODE.DEFAULT || _mode == MODE.DEFAULT) {
				mode = _mode;
				return true;
		} else
				return false;
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
		if (!isOnHUD () && !isOnShelf () && setMode (MODE.FIREBALL) ) {
			PowerFireball.Trigger (cursor3d);
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard Fire");
	}
	public void soundClap(Trigger trgr){
		if ( !isOnHUD() && !isOnShelf() && setMode (MODE.THUNDER_CLAP) ) {
			// Trigger Thunder Clap
			// PeopleManager.GetComponent<LoadVoxelPeople>().Powermode = LoadVoxelPeople.MODE.THUNDER_CLAP;
			// PeopleManager.GetComponent<LoadVoxelPeople>().pointOfContact = cursor3d;
			PowerThunderClap.Trigger( cursor3d );
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard Clap");
	}
	public void soundBolt(Trigger trgr){
		if (!isOnHUD () && !isOnShelf () && setMode (MODE.MJOLNIR) ) {
			PowerMjolnir.Trigger (cursor3d);
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard bolt");
	}
	public void soundTornado(Trigger trgr) {
		if ( !isOnHUD() && !isOnShelf() && setMode(MODE.TORNADO) ) {
			// setMode(MODE.TORNADO);
			PowerTornado.Trigger( cursor3d );
			setMode (MODE.DEFAULT);
		}
		Debug.Log ("I heard Tornado");
	}

	public void soundBuild(Trigger trgr) {
		setMode(MODE.BUILD);
		Debug.Log ("I heard build");
	}

	public void soundExitBuild(Trigger trgr) {
		setMode(MODE.DEFAULT);
		Debug.Log ("I heard exit build");
	}

	public void soundHey (Trigger trgr) {
		if (setMode (MODE.HEY)) {
			PowerHey.Trigger (cursor3d);
			setMode (MODE.DEFAULT);
		}
	}

	public void soundBoo (Trigger trgr) {
		if (setMode (MODE.BOO)) {
			PowerBoo.Trigger (cursor3d);
			setMode (MODE.DEFAULT);
		}
	}

	public void HandDetected (Trigger trgr) {

	}
}
