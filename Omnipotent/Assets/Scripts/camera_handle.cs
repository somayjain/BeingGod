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

	public float handTurnSpeed = 35.0f;			// Speed of camera turning when mouse moves in along an axis
	public float handPanSpeed = 360.0f;			// Speed of the camera when being panned
	public float handZoomSpeed = 500.0f;		// Speed of the camera going back and forth

	public float zoomOut = 100.0f;

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

		// == Getting Input ==
		HandRenderer hr = cursor.GetComponentInChildren<HandRenderer> ();
		bool panmode = false, rotatemode = false, zoommode = false;
		panmode 	= (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == null) || (hr.getLeftHandGesture () == null && hr.getRightHandGesture () == "v_sign");
		rotatemode 	= ((hr.getLeftHandGesture () != null && hr.getRightHandGesture () == "v_sign" ) || (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () != null ));
		//rotatemode 	= (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == "v_sign");
		zoommode 	= (hr.getLeftHandGesture () == "v_sign" && hr.getRightHandGesture () == "v_sign");
		if (!rotatemode) Debug.Log (hr.leftPresent +" <-> " +hr.rightPresent+" == "+hr.getLeftHandGesture () + " <-> " + hr.getRightHandGesture ());

		handmode = panmode || rotatemode || zoommode;
		handmode = true;

		if (handmode) {
			if (rotatemode) {
		//		Debug.Log (hr.leftPresent +" <-> " +hr.rightPresent+" == "+hr.getLeftHandGesture () + " <-> " + hr.getRightHandGesture ());

				isRotating = true;
			}
			

			if(panmode) {
				Debug.Log (hr.leftPresent +" <-> " +hr.rightPresent+" == "+hr.getLeftHandGesture () + " <-> " + hr.getRightHandGesture ());

				if(!isPanning) {
					bool lho = hr.queryLeftHand3DCoordinates(out lefthandorigin);
					if (!lho)
						lho = hr.queryRightHand3DCoordinates(out righthandorigin);
					if (!lho) {
//						isPanning = false;
					} else
						isPanning = true;
				}
			}

			if (zoommode) {
				if(!isZooming) {
					bool lho = hr.queryLeftHand3DCoordinates(out lefthandorigin);
					bool rho = hr.queryRightHand3DCoordinates(out righthandorigin);
					if (!lho || !rho) {
						isZooming = false;
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
		if(Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}
		
		// Get the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}
		
		// Get the middle mouse button
		if(Input.GetMouseButtonDown(2))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isZooming = true;
		}

		// == Disable movements on Input Release ==
		
		if (!Input.GetMouseButton(0))  isPanning = false;
		if (!Input.GetMouseButton(1))  isRotating = false;
		if (!Input.GetMouseButton(2))  isZooming = false;

		// Get the mouse scroll
		// Debug.Log(Input.mouseScrollDelta.ToString());
		if (!zoommode && (Input.mouseScrollDelta.magnitude == 0 || ((transform.position.y <= 5.0f && Input.mouseScrollDelta.y >= 0) || (transform.position.y >= zoomOut && Input.mouseScrollDelta.y <= 0))))
						isZooming = false;
				else if (!zoommode)
						isZooming = true;
		}

		Vector3 screenCenter = new Vector3(Screen.width/2,Screen.height/2);
		Ray ray = Camera.main.ScreenPointToRay(screenCenter);
		float zoomIn = 5.0f;
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)) {
			zoomIn = hit.point.y + 3.0f;
		}

		Vector3 cameraPosition = transform.position;
		cameraPosition.y = Mathf.Clamp (transform.position.y, zoomIn, zoomOut);
		transform.position = cameraPosition;

		prevRealTime = thisRealTime;
		thisRealTime = Time.realtimeSinceStartup;
	}
	
	public float deltaTime {
		get {
			if (Time.timeScale > 0f)  return  Time.deltaTime / Time.timeScale;
			return Time.realtimeSinceStartup - prevRealTime; // Checks realtimeSinceStartup again because it may have changed since Update was called
		}
	}

	//
	// Fixed Update: For Physics
	//


	void FixedUpdate()
	{
		if (cursor.isOnShelf () && !isPanning && !isRotating && !isZooming)
			return;

		// == Movement Code ==
		if (handmode) {
			HandRenderer hr = cursor.GetComponentInChildren<HandRenderer> ();

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
			if (isPanning) {
				Vector3 hc;
				bool hdetected;

				hdetected = hr.queryLeftHand3DCoordinates(out hc);
				if (!hdetected) {
					hdetected = hr.queryRightHand3DCoordinates(out hc);
					if (hdetected) {
						Vector3 move = ( (hc.x-righthandorigin.x) * handPanSpeed * Vector3.ProjectOnPlane(transform.right, Vector3.up) ) + 
							( (hc.z-righthandorigin.z) * handPanSpeed * Vector3.ProjectOnPlane(transform.forward, Vector3.up)) ;
						Debug.Log ("Move " + move.ToString() + " = " + hc.ToString() + " + " + righthandorigin.ToString());
						rigidbody.drag = panDrag;
						rigidbody.AddForce(move, ForceMode.Acceleration);
					}
				}
				else {
				//if (hdetected) {
					Vector3 move = ( (hc.x-lefthandorigin.x) * handPanSpeed * Vector3.ProjectOnPlane(transform.right, Vector3.up) ) + 
						( (hc.z-lefthandorigin.z) * handPanSpeed * Vector3.ProjectOnPlane(transform.forward, Vector3.up)) ;
					Debug.Log ("Move " + move.ToString());
					rigidbody.drag = panDrag;
					rigidbody.AddForce(move, ForceMode.Acceleration);
				}// else
				//	isPanning = false;
			}
			if (isZooming)
			{
				// Get mouse displacement vector from original to current position
				// Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
				// Vector3 move = pos.y * zoomSpeed * transform.forward; 
				Vector3 lhc, rhc;
				bool lhdetected, rhdetected;
				lhdetected = hr.queryLeftHand3DCoordinates(out lhc);
				rhdetected = hr.queryRightHand3DCoordinates(out rhc);
				if (lhdetected && rhdetected) {
					float delta = ((rhc.x - lhc.x) - (righthandorigin.x - lefthandorigin.x));

					if ((Camera.main.transform.position.y < 5.0f && delta >= 0) || (Camera.main.transform.position.y > zoomOut && delta <= 0))
						return;

					Vector3 move = delta * handZoomSpeed * transform.forward;
					rigidbody.drag = zoomDrag;
					rigidbody.AddForce(move, ForceMode.Acceleration);
				}// else
				//	isZooming = false;
			}

		} else {
			// Rotate camera along X and Y axis
			if (isRotating)
			{
				/* TODO:
				 * Rotate about a point instead of rotate about camera position
				 */

				Vector3 screenCenter = new Vector3(Screen.width/2,Screen.height/2);
				
				Ray ray = Camera.main.ScreenPointToRay(screenCenter);
				
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit)) {
					Debug.DrawLine(ray.origin, hit.point);

					Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
					rigidbody.transform.RotateAround(hit.point, Vector3.up, pos.x*turnSpeed);
				}

				// Get mouse displacement vector from original to current position
	//			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
	//			
	//			// Set Drag
	//			rigidbody.angularDrag = turnDrag;
	//			
	//			// Two rotations are required, one for x-mouse movement and one for y-mouse movement
	//			rigidbody.AddTorque(-pos.y * turnSpeed * transform.right, ForceMode.Acceleration);
	//			rigidbody.AddTorque(pos.x * turnSpeed * transform.up, ForceMode.Acceleration);
			}
			
			// Move (pan) the camera on it's XY plane
			if (isPanning)
			{
				// Get mouse displacement vector from original to current position
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

	//			Vector3 move = new Vector3(-pos.x * panSpeed, 0, -pos.y * panSpeed);

				Vector3 move = ( -pos.x * panSpeed * Vector3.ProjectOnPlane(transform.right, Vector3.up) ) + 
								(-pos.y * panSpeed * Vector3.ProjectOnPlane(transform.forward, Vector3.up)) ;
				
				// Apply the pan's move vector in the orientation of the camera's front
				// Quaternion forwardRotation = Quaternion.LookRotation(transform.forward, transform.up);
				// move = forwardRotation * move;
				
				// Set Drag
				rigidbody.drag = panDrag;
				
				// Pan
				rigidbody.AddForce(move, ForceMode.Acceleration);
			}
			
			// Move the camera linearly along Z axis
			if (isZooming)
			{
				/* TODO: 
				 * Closer the camera, more the pan speed.
				 */
				if ((Camera.main.transform.position.y < 5.0f && Input.mouseScrollDelta.y >= 0) || (Camera.main.transform.position.y > zoomOut && Input.mouseScrollDelta.y <= 0))
					return;

				// Get mouse displacement vector from original to current position
				// Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
				// Vector3 move = pos.y * zoomSpeed * transform.forward; 
				Vector3 move = Input.mouseScrollDelta.y * zoomSpeed * transform.forward;

				// Set Drag
				rigidbody.drag = zoomDrag;
				
				// Zoom
				rigidbody.AddForce(move, ForceMode.Acceleration);
			}
		}
	}
}