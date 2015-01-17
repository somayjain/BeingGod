using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{

		public Terrain myTerrain;
		public Camera myCamera;
		private TerrainData myTerrainData;
		public bool editMountainFlag;
		public bool editForestFlag;
		private float max_X;
		private float max_Y; //Height
		private float max_Z;
		private Vector3 terrainPosition;
		private Vector3 startDrag ;
		private Vector3 endDrag;
		private bool mouseLeft;
		private bool mouseRight;
		public float scrollStep;
		public double panStep;
		public double rotationStep;
		public Vector3 myMouseposition;
		// Use this for initialization
		void Start ()
		{
				myTerrainData = myTerrain.terrainData;
				max_X = myTerrainData.size.x;
				max_Y = myTerrainData.size.y;
				max_Z = myTerrainData.size.z;
				terrainPosition = myTerrain.GetPosition ();
				editMountainFlag = false;
				editForestFlag = false;
				mouseLeft = false;
				mouseRight = false;
				scrollStep = 5;
				panStep = 0.001;
				rotationStep = 1;
		}
	
		// Update is called once per frame
		void Update ()
		{
				// Checking the keypress for mode detection
				editMountainFlag = Input.GetKeyDown (KeyCode.M); // Checking for Mountain mode
				editForestFlag = Input.GetKeyDown (KeyCode.F); // Checking for Forest mode
				if (editMountainFlag) {  // MODE#1 : create mountains
						print ("mountainCursor");
						// Write editMountainCode here
						editMountainFlag = false;
				} else if (editForestFlag) { // MODE#2 : create forests
						print ("forestCursor");
						// Write editForestCode here
						editForestFlag = false;
				} else { // MODE#3 : (DEFAULT) Manipulate camera
						cameraMovement ();
				}


		}

		void cameraMovement ()
		{
				myMouseposition = Input.mousePosition;
				// LEFT BUTTON ; Panning button
				if (Input.GetMouseButtonDown (0) && !mouseLeft) { 
						startDrag = Input.mousePosition;
						mouseLeft = true;
				}
				if (Input.GetMouseButtonUp (0)) {	
						endDrag = Input.mousePosition;
						mouseLeft = false;
						Vector3 panValue = endDrag - startDrag;
						panValue.z = panValue.y;
						panValue.y = 0; // No ZOOMING 
						Vector3 foo = myCamera.transform.position - panValue;
						// Limiting Cursor movement within the terrain
						if (foo.x < terrainPosition.x) {
								foo.x = terrainPosition.x;
						}
						if (foo.z < terrainPosition.z) {
								foo.z = terrainPosition.z;
						}
						if (foo.x > max_X) {
								foo.x = max_X;
						}
						if (foo.z > max_Z) {
								foo.z = max_Z;
						}
						myCamera.transform.position = foo;
				}

				// SCROLL BUTTON (FOR ZOOMING IN/OUT)
				float scroll = Input.GetAxis ("Mouse ScrollWheel"); 
				if (scroll > 0) {	//ZOOM IN
						float scrollValue = -scroll * scrollStep;
						Vector3 foo = new Vector3 (myCamera.transform.position.x, myCamera.transform.position.y + scrollValue, myCamera.transform.position.z);
						if (foo.y < 5) {
								foo.y = 5;
						}
						myCamera.transform.position = foo;
				} else if (scroll < 0) {	//ZOOM OUT
						float scrollValue = -scroll * scrollStep;
						Vector3 foo = new Vector3 (myCamera.transform.position.x, myCamera.transform.position.y + scrollValue, myCamera.transform.position.z);
						if (foo.y > max_Y + 50) {
								foo.y = max_Y + 50;
						}
						myCamera.transform.position = foo;
				}

				// RIGHT BUTTON ; Tilting button
				if (Input.GetMouseButtonDown (1) && !mouseRight) { 
						startDrag = Input.mousePosition;
						mouseRight = true;
				}
				if (Input.GetMouseButtonUp (1)) {	
//					float aH=Input.GetAxis("Horizontal"); 
//					float aV=Input.GetAxis("Vertical"); 
//					float mX=Input.GetAxis("Mouse X"); 
//					float mY=Input.GetAxis("Mouse Y"); 
//					string foo_S="Horizontal="+aH.ToString()+" Vertical="+aV.ToString()+"\tMouse X="+mX.ToString()+" Mouse Y="+mY.ToString(); 
//					Debug.Log(foo_S);

//						endDrag = Input.mousePosition;
//						mouseRight = false;
//						Vector3 rotValue = endDrag - startDrag;
//						rotValue.z = rotValue.y;
//						rotValue.y = 0; // No ZOOMING 
//						//Vector3 fooRot = myCamera.transform.Rotate - rotValue;
//						Quaternion target = Quaternion.Euler(rotValue);
//						Vector3 fooRot = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 2.0);
//						// Limiting Cursor movement within the terrain
//						if (fooRot.x < terrainPosition.x) {
//								fooRot.x = terrainPosition.x;
//						}
//						if (fooRot.z < terrainPosition.z) {
//								fooRot.z = terrainPosition.z;
//						}
//						if (fooRot.x > max_X) {
//								fooRot.x = max_X;
//						}
//						if (fooRot.z > max_Z) {
//								fooRot.z = max_Z;
//						}
						//string msg = 
						
//						float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
//						float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
//						Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
//						transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
				}
		}
}
