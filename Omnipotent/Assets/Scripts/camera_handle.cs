// Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
// for using the mouse displacement for calculating the amount of camera movement and panning code.

using UnityEngine;
using System.Collections;

public class camera_handle : MonoBehaviour {

	//
	// VARIABLES
	//

	public cursor_handle cursor;
	
	public float turnSpeed = 35.0f;			// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 360.0f;			// Speed of the camera when being panned
	public float zoomSpeed = 500.0f;		// Speed of the camera going back and forth

	public float handTurnSpeed = 2.0f;		// Speed of camera turning when mouse moves in along an axis
	public Vector2 handPanSpeed = new Vector2 (20.0f, 50.0f);			// Speed of the camera when being panned
	public float handZoomSpeed = 8.0f;		// Speed of the camera going back and forth

	public float zoomOut = 100.0f;
	public float autoforce = 25.0f;

	public float turnDrag = 5.0f;			// RigidBody Drag when rotating camera
	public float panDrag = 3.5f;			// RigidBody Drag when panning camera
	public float zoomDrag = 3.3f;			// RigidBody Drag when zooming camera
	
	private Vector3 mouseOrigin;			// Position of cursor when mouse dragging starts
	private bool isPanning;				// Is the camera being panned?
	private bool isRotating;			// Is the camera being rotated?
	private bool isZooming;				// Is the camera zooming?

	private bool handmode = false;
	private Vector3 lefthandorigin, righthandorigin;

	// Use this for initialization
	void Start () {
		isPanning = false;
		isRotating = false;
		isZooming = false;
	}

	//
	// AWAKE
	//
	
	void Awake()
	{
		// Setup camera physics properties
		// gameObject.AddComponent<Rigidbody>();
		// rigidbody.useGravity = false;
	}

	
	private float prevRealTime;
	private float thisRealTime;
	
	//
	// UPDATE: For input
	//

	void Update () 
	{
		if (cursor.isOnHUD () && !isPanning && !isRotating && !isZooming)
			return;

		if (cursor.mode != cursor_handle.MODE.BUILD) {
						// == Getting Input ==
						HandRenderer hr = cursor.GetComponentInChildren<HandRenderer> ();
						bool panmode = false, rotatemode = false, zoommode = false;
						// panmode 	= (hr.getLeftHandGesture () == "fist" && hr.getRightHandGesture () == null) || (hr.getLeftHandGesture () == null && hr.getRightHandGesture () == "fist");
						panmode = (hr.getNumHandsDetected () == 1 && (hr.getLeftHandGesture () == "fist" || hr.getRightHandGesture () == "fist"));
						rotatemode = (hr.getNumHandsDetected () == 2 && ((hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == "v_sign")));// || (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () != null )) );
						//rotatemode 	= (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == "v_sign");
						zoommode = (hr.getNumHandsDetected () == 2 && ((hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == "v_sign")));// || (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () != null )) );
						// hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == "v_sign");
						if (!rotatemode)
								Debug.Log (hr.leftPresent + " <-> " + hr.rightPresent + " == " + hr.getLeftHandGesture () + " <-> " + hr.getRightHandGesture ());

						handmode = panmode || rotatemode || zoommode;
						//handmode = true;

						if (handmode) {
								Debug.Log ("Pan: " + isPanning.ToString () + ", Rot: " + isRotating.ToString () + ", " + panmode.ToString () + ", " + rotatemode.ToString ());
								if (!isPanning && rotatemode) {
										//		Debug.Log (hr.leftPresent +" <-> " +hr.rightPresent+" == "+hr.getLeftHandGesture () + " <-> " + hr.getRightHandGesture ());

										isRotating = true;
								} else if (!isRotating && !isZooming && panmode) {
										Debug.Log (hr.leftPresent + " <-> " + hr.rightPresent + " == " + hr.getLeftHandGesture () + " <-> " + hr.getRightHandGesture ());

										if (!isPanning) {
												bool lho = hr.queryLeftHand3DCoordinates (out lefthandorigin);
												if (!lho)
														lho = hr.queryRightHand3DCoordinates (out righthandorigin);
												if (!lho) {
//						isPanning = false;
												} else
														isPanning = true;
										}
								}

								if (!isPanning && zoommode) {
										if (!isZooming) {
												bool lho = hr.queryLeftHand3DCoordinates (out lefthandorigin);
												bool rho = hr.queryRightHand3DCoordinates (out righthandorigin);
												if (!lho || !rho) {
//						isZooming = false;
												} else
														isZooming = true;
										}
								}
								if (hr.getLeftHandGesture () == "spreadfingers" || hr.getRightHandGesture () == "spreadfingers") {
										isPanning = false;
										isRotating = false;
										isZooming = false;
								}
								Debug.Log ("ispanning " + isPanning + " isR " + isRotating);

						} else {

								// Get the left mouse button
								if (Input.GetMouseButtonDown (0)) {
										// Get mouse origin
										mouseOrigin = Input.mousePosition;
										isPanning = true;
								}
			
								// Get the right mouse button
								if (Input.GetMouseButtonDown (1)) {
										// Get mouse origin
										mouseOrigin = Input.mousePosition;
										isRotating = true;
								}
			
								// Get the middle mouse button
								if (Input.GetMouseButtonDown (2)) {
										// Get mouse origin
										mouseOrigin = Input.mousePosition;
										isZooming = true;
								}

								// == Disable movements on Input Release ==
			
								if (!Input.GetMouseButton (0))
										isPanning = false;
								if (!Input.GetMouseButton (1))
										isRotating = false;
								if (!Input.GetMouseButton (2))
										isZooming = false;

								// Get the mouse scroll
								// Debug.Log(Input.mouseScrollDelta.ToString());
								if (!zoommode && (Input.mouseScrollDelta.magnitude == 0 || ((transform.position.y <= 5.0f && Input.mouseScrollDelta.y >= 0) || (transform.position.y >= zoomOut && Input.mouseScrollDelta.y <= 0))))
										isZooming = false;
								else if (!zoommode)
										isZooming = true;
						}
				}

		Vector3 screenCenter = new Vector3(Screen.width/2,Screen.height/2);
		Ray ray = Camera.main.ScreenPointToRay(screenCenter);
		float zoomIn = 5.0f;
		RaycastHit hit;		
		if(Physics.Raycast (ray, out hit, 10.0f)) {
			zoomIn = hit.point.y + 3.0f;
		}

		Vector3 cameraPosition = transform.position;
		// Debug.Log ("Camera: " + cameraPosition.ToString ()+" zin: " + zoomIn.ToString ()+ ", zout: " + zoomOut.ToString() + " clamp: " + Mathf.Clamp (transform.position.y, zoomIn, zoomOut).ToString() );
//		cameraPosition.y = Mathf.Clamp (transform.position.y, zoomIn, zoomOut);
//		transform.position = cameraPosition;

		prevRealTime = thisRealTime;
		thisRealTime = Time.realtimeSinceStartup;

		if (isRotating || isZooming) {
			transform.GetChild(0).GetComponent<TrackingAction>().enabled = false;
		} else
			transform.GetChild(0).GetComponent<TrackingAction>().enabled = true;
	}
	
	public float deltaTime {
		get {
			if (Time.timeScale > 0f)  return  Time.deltaTime / Time.timeScale;
			return Time.realtimeSinceStartup - prevRealTime; // Checks realtimeSinceStartup again because it may have changed since Update was called
		}
	}

	private void PanLeft () {
		rigidbody.drag = panDrag;
		rigidbody.AddForce (-autoforce * Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up), ForceMode.Acceleration);
	}
	private void PanRight () {
		rigidbody.drag = panDrag;
		rigidbody.AddForce (autoforce * Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up), ForceMode.Acceleration);
	}
	private void PanTop () {
		rigidbody.drag = panDrag;
		rigidbody.AddForce (autoforce * Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up), ForceMode.Acceleration);
	}
	private void PanBottom () {
		rigidbody.drag = panDrag;
		rigidbody.AddForce (-autoforce * Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up), ForceMode.Acceleration);
	}

	//
	// Fixed Update: For Physics
	//


	void FixedUpdate()
	{
		if (cursor.isOnShelf () && !isPanning && !isRotating && !isZooming)
			return;

		if (cursor.mode == cursor_handle.MODE.BUILD) {
			// If hand2d near screen edge pan
			Debug.Log ("Cursor pos: "+cursor.cursor2d.ToString());
			if (cursor.cursor2d.x < Screen.width/3.0f)
					PanLeft ();
			else if (cursor.cursor2d.x > Screen.width*2.0f/3.0f)
					PanRight ();
			if (cursor.cursor2d.y < Screen.height/3.0f)
					PanBottom ();
			else if (cursor.cursor2d.y > Screen.height*2.0f/3.0f)
					PanTop ();

			return;
		}


		// == Movement Code ==
		if (handmode) {
			HandRenderer hr = cursor.GetComponentInChildren<HandRenderer> ();

			if (isPanning) {
				Vector3 hc;
				bool hdetected;

				hdetected = hr.queryLeftHand3DCoordinates(out hc);
				if (!hdetected) {
					hdetected = hr.queryRightHand3DCoordinates(out hc);
					if (hdetected) {
						Vector3 move = ( (righthandorigin.x-hc.x) * handPanSpeed.x * Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up) ) + 
							( (hc.z-righthandorigin.z) * handPanSpeed.y * Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up)) ;
						Debug.Log ("Move " + move.ToString() + " = " + hc.ToString() + " + " + righthandorigin.ToString());
						rigidbody.drag = panDrag;
						rigidbody.AddForce(move, ForceMode.Acceleration);
					}
				}
				else {
				//if (hdetected) {
					Vector3 move = ( (lefthandorigin.x-hc.x) * handPanSpeed.x * Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up) ) + 
						( (hc.z-lefthandorigin.z) * handPanSpeed.y * Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up)) ;
					Debug.Log ("Move " + move.ToString());
					rigidbody.drag = panDrag;
					rigidbody.AddForce(move, ForceMode.Acceleration);
				}// else
				//	isPanning = false;
			} else {
				if (isRotating) {
					Vector3 screenCenter = new Vector3(Screen.width/2,Screen.height/2);
					Ray ray = Camera.main.ScreenPointToRay(screenCenter);				
					RaycastHit hit;
					
					if(Physics.Raycast(ray, out hit)) {
						Debug.DrawLine(ray.origin, hit.point);
						
						Vector3 lhc, rhc;
						bool lhdetected, rhdetected;
						lhdetected = hr.queryLeftHand3DCoordinates(out lhc);
						rhdetected = hr.queryRightHand3DCoordinates(out rhc);
						Debug.Log(lhdetected.ToString() + " " + rhdetected.ToString());
						if (lhdetected && rhdetected) {
							Vector3 move = (rhc - lhc);
							//						Debug.Log("Rot: " + move.ToString());
							rigidbody.transform.RotateAround(hit.point, Vector3.up, move.z*handTurnSpeed);
						}// else
						//	isRotating = false;
						Debug.Log("Rota is " + isRotating.ToString());
					}
				}
				if (isZooming)
				{
					Vector3 lhc, rhc;
					bool lhdetected, rhdetected;
					lhdetected = hr.queryLeftHand3DCoordinates(out lhc);
					rhdetected = hr.queryRightHand3DCoordinates(out rhc);
					if (lhdetected && rhdetected) {
						float delta = ((rhc.x - lhc.x) - (righthandorigin.x - lefthandorigin.x));

						if ((transform.position.y < 5.0f && delta >= 0) || (transform.position.y > zoomOut && delta <= 0))
							return;

						Vector3 move = delta * handZoomSpeed * Camera.main.transform.forward;
						Debug.Log (move.ToString());
						//if (delta > 1) transform.position = transform.position + Vector3.up;
						rigidbody.drag = zoomDrag;
						rigidbody.AddForce(move , ForceMode.Force);
					}// else
					//	isZooming = false;
				}
			}

		} else {
			// Rotate camera along X and Y axis
			if (isRotating)
			{
				Vector3 screenCenter = new Vector3(Screen.width/2,Screen.height/2);
				Ray ray = Camera.main.ScreenPointToRay(screenCenter);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit)) {
					Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
					rigidbody.transform.RotateAround(hit.point, Vector3.up, pos.x*turnSpeed);
				}
			}
			
			// Move (pan) the camera on it's XY plane
			if (isPanning)
			{
				// Get mouse displacement vector from original to current position
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

				Vector3 move = ( -pos.x * panSpeed * Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up) ) + 
								(-pos.y * panSpeed * Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up)) ;
				rigidbody.drag = panDrag;
				rigidbody.AddForce(move, ForceMode.Acceleration);
			}
			
			// Move the camera linearly along Z axis
			if (isZooming)
			{
				/* TODO: 
				 * Closer the camera, more the pan speed.
				 */
				if ((transform.position.y < 5.0f && Input.mouseScrollDelta.y >= 0) || (transform.position.y > zoomOut && Input.mouseScrollDelta.y <= 0))
					return;
				Vector3 move = Input.mouseScrollDelta.y * zoomSpeed * Camera.main.transform.forward;
				rigidbody.drag = zoomDrag;
				rigidbody.AddForce(move, ForceMode.Acceleration);
			}
		}
	}
}