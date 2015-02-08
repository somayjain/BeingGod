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

	public float zoomOut = 100.0f;

	public float turnDrag = 5.0f;			// RigidBody Drag when rotating camera
	public float panDrag = 3.5f;			// RigidBody Drag when panning camera
	public float zoomDrag = 3.3f;			// RigidBody Drag when zooming camera
	
	private Vector3 mouseOrigin;			// Position of cursor when mouse dragging starts
	private bool isPanning;				// Is the camera being panned?
	private bool isRotating;			// Is the camera being rotated?
	private bool isZooming;				// Is the camera zooming?
	

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
	
	//
	// UPDATE: For input
	//
	
	void Update () 
	{
		if (cursor.isOnShelf () && !isPanning && !isRotating && !isZooming)
			return;

		// == Getting Input ==

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
		if (Input.mouseScrollDelta.magnitude == 0 || ((transform.position.y <= 5.0f && Input.mouseScrollDelta.y >= 0) || (transform.position.y >= zoomOut && Input.mouseScrollDelta.y <= 0)))
						isZooming = false;
				else
						isZooming = true;

		Vector3 cameraPosition = transform.position;
		cameraPosition.y = Mathf.Clamp (transform.position.y, 5.0f, zoomOut);
		transform.position = cameraPosition;
	}
	
	//
	// Fixed Update: For Physics
	//
	
	void FixedUpdate()
	{
		if (cursor.isOnShelf () && !isPanning && !isRotating && !isZooming)
			return;

		// == Movement Code ==
		
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