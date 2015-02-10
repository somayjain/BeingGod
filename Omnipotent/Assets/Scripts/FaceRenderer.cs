/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;

public class FaceRenderer : MonoBehaviour
{

		public GameObject LandmarkPrefab; //Prefab for Joints
		public GameObject BonePrefab; //Prafab for Bones
		public GameObject EyePrefab;//Prefab for Palm Center
		public GameObject myfaceCube;
		public GameObject PoseGUIText;
		public GameObject ExpressionGUIText;
		public GameObject AlertGUIText;
		private PXCMFaceData.LandmarkPoint[] points;
		private GameObject[] myLandmarks; //Array of Joint GameObjects
		private GameObject[] myBones; //Array of Bone GameObjects
		private int MaxPoints = 78;
		private Color color;
		private Texture2D detectionTexture;

		// Use this for initialization
		void Start ()
		{
				myLandmarks = new GameObject[MaxPoints];
				myBones = new GameObject[MaxPoints];

				for (int j = 0; j < MaxPoints; j++) {
						if (j == 76 || j == 77)
								myLandmarks [j] = (GameObject)Instantiate (EyePrefab, Vector3.zero, Quaternion.identity);
						else
								myLandmarks [j] = (GameObject)Instantiate (LandmarkPrefab, Vector3.zero, Quaternion.identity);

						myBones [j] = (GameObject)Instantiate (BonePrefab, Vector3.zero, Quaternion.identity);

						myLandmarks [j].SetActive (false);
						myBones [j].SetActive (false);
				}

				color = Color.grey;
				detectionTexture = new Texture2D (1, 1);
				color.a = 0.4f;
				detectionTexture.SetPixel (0, 0, color);
				detectionTexture.wrapMode = TextureWrapMode.Repeat;
				detectionTexture.Apply ();

				myfaceCube.renderer.material.mainTexture = detectionTexture;
		}

		public void SetDetectionRect (PXCMRectI32 _Rect)
		{
				myfaceCube.transform.position = new Vector3 (_Rect.x + 100f, -1 * _Rect.y - 120f) * 0.05f;
				myfaceCube.transform.localScale = new Vector3 (_Rect.w, _Rect.h) * 0.05f;
		}

		public void DisplayJoints2D (PXCMFaceData.LandmarkPoint[] _points)
		{
				for (int j = 0; j < MaxPoints; j++)
						if (_points [j] != null && _points [j].confidenceImage == 100) {
								myLandmarks [j].SetActive (true);
								myLandmarks [j].transform.position = new Vector3 (_points [j].image.x, -1 * _points [j].image.y) * 0.05f;
						} else {
								myLandmarks [j].SetActive (false);      
						}

				for (int j = 0; j < MaxPoints; j++) {
						//Rescale Eyes
						if (j == 76 || j == 77) {
								float factor = myLandmarks [12].transform.position.y - myLandmarks [16].transform.position.y - 0.04f;
								if (factor > 0.5)
										factor = 0.5f;
								myLandmarks [j].transform.localScale = new Vector3 (factor, factor, myLandmarks [j].transform.localScale.z);
						}
				}


				for (int j = 0; j < MaxPoints; j++) {
						if (j != 52 && j != 69 && j != 9 && j != 25 && j != 29 && j != 32 && j != 4 && j != 17 && j != 44 && j != 75 && j != 76 && j != 77 && j != 72)
								UpdateBoneTransform (myBones [j], myLandmarks [j], myLandmarks [j + 1]);

						//Eyes
						UpdateBoneTransform (myBones [52], myLandmarks [17], myLandmarks [10]); //52
						UpdateBoneTransform (myBones [69], myLandmarks [18], myLandmarks [25]); //69

						//Lips
						UpdateBoneTransform (myBones [9], myLandmarks [44], myLandmarks [33]); //9
						UpdateBoneTransform (myBones [25], myLandmarks [52], myLandmarks [45]); //25
				}
		}

		void UpdateBoneTransform (GameObject _bone, GameObject _prevJoint, GameObject _nextJoint)
		{

				if (_prevJoint.activeSelf == false || _nextJoint.activeSelf == false) {
						_bone.SetActive (false);
				} else {
						_bone.SetActive (true);
						// Update Position
						_bone.transform.position = ((_nextJoint.transform.position - _prevJoint.transform.position) / 2f) + _prevJoint.transform.position;

						// Update Scale
						_bone.transform.localScale = new Vector3 (0.15f, (_nextJoint.transform.position - _prevJoint.transform.position).magnitude - (_prevJoint.transform.position - _nextJoint.transform.position).magnitude / 2f, 0.15f);

						// Update Rotation
						_bone.transform.rotation = Quaternion.FromToRotation (Vector3.up, _nextJoint.transform.position - _prevJoint.transform.position);
				}
		}

		public void DisplayPoseQuaternion (PXCMFaceData.PoseQuaternion poseQuaternion)
		{
				PoseGUIText.GetComponent<TextMesh> ().text = "Quaternion Pose Data \nx: " + poseQuaternion.x + "\ny: " + poseQuaternion.y + "\nz: " + poseQuaternion.z + "\nw: " + poseQuaternion.w;
		}

		public void DisplayExpression (PXCMFaceData.ExpressionsData.FaceExpressionResult expressionResult, PXCMFaceData.ExpressionsData.FaceExpression faceExpression)
		{
				ExpressionGUIText.GetComponent<TextMesh> ().text = faceExpression.ToString () + "\n" + expressionResult.intensity.ToString ();
		}

		public void DisplayAlerts (PXCMFaceData.AlertData _alertData)
		{
				StopCoroutine ("TimedAlert");
				StartCoroutine ("TimedAlert", _alertData.label.ToString ());
				if (_alertData.label == PXCMFaceData.AlertData.AlertType.ALERT_FACE_LOST || _alertData.label == PXCMFaceData.AlertData.AlertType.ALERT_FACE_OUT_OF_FOV || _alertData.label == PXCMFaceData.AlertData.AlertType.ALERT_FACE_OCCLUDED) {
						for (int j = 0; j < MaxPoints; j++) {
								myLandmarks [j].SetActive (false); 
								myBones [j].SetActive (false);
						}
				}
				PoseGUIText.GetComponent<TextMesh> ().text = "Quaternion Pose Data \nx: " + "\ny: " + "\nz: " + "\nw: ";
				ExpressionGUIText.GetComponent<TextMesh> ().text = "Expression Data";
				myfaceCube.transform.position = new Vector3 (Screen.width * 2, Screen.height * 2);
      
		}

		IEnumerator TimedAlert (string _label)
		{
				AlertGUIText.GetComponent<TextMesh> ().text = "Last Alert : " + _label;
				yield return new WaitForSeconds (10f);
				AlertGUIText.GetComponent<TextMesh> ().text = "";
		}
}