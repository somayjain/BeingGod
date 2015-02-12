/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandRenderer : MonoBehaviour {

    public GameObject JointPrefab; //Prefab for Joints
    public GameObject TipPrefab; //Prefab for Finger Tips
    public GameObject BonePrefab; //Prafab for Bones
    public GameObject PalmCenterPrefab;//Prefab for Palm Center
	public GameObject myGestureTextLeft;//Pointer to LeftGUIText
	public GameObject myGestureTextRight;//Pointer to RightGUIText

    public GameObject handofGod;

    private int MaxHands; //Max no. of Hands
    private GameObject[,] myJoints; //Array of Joint GameObjects
    private GameObject[,] myBones; //Array of Bone GameObjects
    private int[] handIds; //Array of handIds
    private PXCMHandData.BodySideType[] bodySide; //Array of BodySides
    
    private Queue avgQueue; //contains jointData values over a set of frames
    private Vector3[,] sumOfJointPositions; //sum of pos over a set of frames
    private int SmoothingFactor;

	private Vector3 leftPalmCenterJoint, rightPalmCenterJoint;
	private PXCMHandData outputData;

	// Use this for initialization
	void Start() {
        MaxHands = 2;
        SmoothingFactor = 4;
        myJoints = new GameObject[MaxHands, PXCMHandData.NUMBER_OF_JOINTS];
        myBones = new GameObject[MaxHands, PXCMHandData.NUMBER_OF_JOINTS];
        sumOfJointPositions = new Vector3[MaxHands, PXCMHandData.NUMBER_OF_JOINTS];
        avgQueue = new Queue();
        handIds = new int[MaxHands];
        bodySide = new PXCMHandData.BodySideType[MaxHands];

        handIds[0] = handIds[1] = -1;
        
        for (int i = 0; i < MaxHands; i++)
            for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
            {
                if (j == 1)
                {
                    myJoints[i, j] = (GameObject)Instantiate(PalmCenterPrefab, Vector3.zero, Quaternion.identity);
                    myJoints[i, j].transform.parent = handofGod.transform;    
                }
                else if (j == 21 || j == 17 || j == 13 || j == 9 || j == 5)
                {
                    myJoints[i, j] = (GameObject)Instantiate(TipPrefab, Vector3.zero, Quaternion.identity);
                    myJoints[i, j].transform.parent = handofGod.transform;
                }
                else
                {
                    myJoints[i, j] = (GameObject)Instantiate(JointPrefab, Vector3.zero, Quaternion.identity);
                    myJoints[i, j].transform.parent = handofGod.transform;
                }

                if (j != 1)
                {
                    myBones[i, j] = (GameObject)Instantiate(BonePrefab, Vector3.zero, Quaternion.identity);
                    myBones[i, j].transform.parent = handofGod.transform;
                }
            }

        
	}

	public void DisplayGestures(PXCMHandData.GestureData gestureData)
	{
		for (int i = 0; i < MaxHands; i++)
			if (gestureData.handId == handIds[i])
		{
			if (bodySide[i] == PXCMHandData.BodySideType.BODY_SIDE_LEFT)
			{
				myGestureTextLeft.GetComponent<Text>().text =  "Left Hand : "+ gestureData.name.ToString();
			}
			else if (bodySide[i] == PXCMHandData.BodySideType.BODY_SIDE_RIGHT)
			{
				myGestureTextRight.GetComponent<Text>().text = "Right Hand : "+ gestureData.name.ToString(); 
			}
			break;
		}
	}


	public void DisplayJoints(PXCMHandData.JointData[,] _myJointData, int[] _handIds, PXCMHandData.BodySideType[] _bodySides)
    {
        handIds = _handIds;
        bodySide = _bodySides;
        
                for (int i = 0; i < MaxHands; i++)
                    for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
                        if (_myJointData[i, j] != null)
                        {
                            myJoints[i, j].SetActive(true);
                            myJoints[i, j].transform.localPosition = new Vector3(_myJointData[i, j].positionWorld.x, _myJointData[i, j].positionWorld.y, _myJointData[i, j].positionWorld.z) * 100f;
                        }
                        else
                            myJoints[i, j].SetActive(false);

    }

	public void DisplaySmoothenedJoints(PXCMHandData _outputData, PXCMHandData.JointData[,] _myJointData, int[] _handIds, PXCMHandData.BodySideType[] _bodySides)
    {
		float factor = 30.0f;
        handIds = _handIds;
        bodySide = _bodySides;
		outputData = _outputData;

		Vector3 pos, palmCenterPos;
        avgQueue.Enqueue(_myJointData);

        foreach (PXCMHandData.JointData[,] temp in avgQueue)
            for (int i = 0; i < MaxHands; i++)
                for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
                    if (temp[i, j] != null && _myJointData[i, j] != null)
                    {
                        myJoints[i, j].SetActive(true);
                        sumOfJointPositions[i, j] += new Vector3(-1 * temp[i, j].positionWorld.x * 2.0f, temp[i, j].positionWorld.y * 1.2f, temp[i, j].positionWorld.z);
                    }
                    else
                        myJoints[i, j].SetActive(false);

        for (int i = 0; i < MaxHands; i++)
		{
			palmCenterPos = (sumOfJointPositions[i, 1] / avgQueue.Count) * factor;
			if(bodySide[i] == (PXCMHandData.BodySideType)1)
				// Left Hand
				leftPalmCenterJoint = (sumOfJointPositions[i, 1] / avgQueue.Count) * factor;
			
			else if(bodySide[i] == (PXCMHandData.BodySideType)2)
				// Right Hand
				rightPalmCenterJoint = (sumOfJointPositions[i, 1] / avgQueue.Count) * factor;
			
			for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
            {
				pos = (sumOfJointPositions[i, j] / avgQueue.Count) * factor;
				//Debug.Log ("Joint " + j + ": " + (pos - palmCenterPos).ToString ());
                
				myJoints[i, j].transform.localPosition = pos;
                sumOfJointPositions[i, j] = Vector3.zero;
            }
		}

		for (int i = 0; i < MaxHands; i++)
			for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++) {		
				
				if (j != 21 && j != 0 && j != 1 && j != 5 && j != 9 && j != 13 && j != 17)
					UpdateBoneTransform (myBones [i, j], myJoints [i, j], myJoints [i, j + 1]);
				
				UpdateBoneTransform (myBones [i, 21], myJoints [i, 0], myJoints [i, 2]);
				UpdateBoneTransform (myBones [i, 17], myJoints [i, 0], myJoints [i, 18]);
				
				UpdateBoneTransform (myBones [i, 5], myJoints [i, 14], myJoints [i, 18]);
				UpdateBoneTransform (myBones [i, 9], myJoints [i, 10], myJoints [i, 14]);
				UpdateBoneTransform (myBones [i, 13], myJoints [i, 6], myJoints [i, 10]);
				UpdateBoneTransform (myBones [i, 0], myJoints [i, 2], myJoints [i, 6]);
		}

        if (avgQueue.Count >= SmoothingFactor)
            avgQueue.Dequeue();

    }

	//Update Bones
	void UpdateBoneTransform (GameObject _bone, GameObject _prevJoint, GameObject _nextJoint)
	{
		
		if (_prevJoint.activeSelf == false || _nextJoint.activeSelf == false)
			_bone.SetActive (false);
		else {
			_bone.SetActive (true);
			
			// Update Position
			_bone.transform.position = ((_nextJoint.transform.position - _prevJoint.transform.position) / 2f) + _prevJoint.transform.position;
			
			// Update Scale
			_bone.transform.localScale = new Vector3 (0.8f, (_nextJoint.transform.position - _prevJoint.transform.position).magnitude - (_prevJoint.transform.position - _nextJoint.transform.position).magnitude / 2f, 0.8f);
			
			// Update Rotation
			_bone.transform.rotation = Quaternion.FromToRotation (Vector3.up, _nextJoint.transform.position - _prevJoint.transform.position);
		}
		
	}

	public Vector2 queryLeftHand2DCoordinates()
	{
		if (outputData != null) {
			Vector3 pos = transform.TransformPoint (leftPalmCenterJoint);
			//Debug.Log ("Left hand Screen coords " + Camera.main.camera.WorldToScreenPoint (pos).ToString ());
			return Camera.main.camera.WorldToScreenPoint(pos);
		}
		else
		{
			//Debug.Log("No Hand");
			return new Vector2(-1f, -1f);
		}
	}

	public Vector2 queryRightHand2DCoordinates()
	{
		if (outputData != null) {
			Vector3 pos = transform.TransformPoint (rightPalmCenterJoint);
			//Debug.Log ("Right hand Screen coords " + Camera.main.camera.WorldToScreenPoint (pos).ToString ());
			return Camera.main.camera.WorldToScreenPoint(pos);
		}
		else
		{
			//Debug.Log("No Hand");
			return new Vector2(-1f, -1f);
		}
	}

	public Vector3 queryLeftHand3DCoordinates()
	{
		if (outputData != null) {
			return transform.TransformPoint(leftPalmCenterJoint);
		}
		else
			return new Vector3(-1f, -1f, -1f);
	}

	public Vector3 queryRightHand3DCoordinates()
	{
		if (outputData != null) {
			return transform.TransformPoint(leftPalmCenterJoint);
		}
		else
			return new Vector3(-1f, -1f, -1f);
	}
	public void makeNull()
	{
		outputData = null;
		myGestureTextLeft.GetComponent<Text> ().text = "";
		myGestureTextRight.GetComponent<Text> ().text = "";
	}
}
