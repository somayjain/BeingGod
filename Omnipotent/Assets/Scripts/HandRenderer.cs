/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandRenderer : MonoBehaviour {

    public GameObject JointPrefab; //Prefab for Joints
    public GameObject TipPrefab; //Prefab for Finger Tips
    public GameObject BonePrefab; //Prafab for Bones
    public GameObject PalmCenterPrefab;//Prefab for Palm Center
	public GameObject myGestureTextLeft;//Pointer to LeftGUIText
	public GameObject myGestureTextRight;//Pointer to RightGUIText

    public GameObject handofGod;

	public Vector3 bonescale = new Vector3 (0.5f,0.5f,0.5f);

    private int MaxHands; //Max no. of Hands
    private GameObject[,] myJoints; //Array of Joint GameObjects
    private GameObject[,] myBones; //Array of Bone GameObjects
    private int[] handIds; //Array of handIds
    private PXCMHandData.BodySideType[] bodySide; //Array of BodySides
    
    private Queue avgQueue; //contains jointData values over a set of frames
    private Vector3[,] sumOfJointPositions; //sum of pos over a set of frames
    private int SmoothingFactor;

	private int NumHands;
	private Vector3 leftPalmCenterJoint, rightPalmCenterJoint;
	private PXCMHandData outputData;
	private PXCMHandData.GestureData gestureData;
	private string lefthandgesture, righthandgesture;
	public bool leftPresent, rightPresent;

	private enum HAND_GESTURE {
		NONE,
		FIST,
		FULLPINCH,
		SPREADFINGERS,
		SWIPE,
		TAP,
		THUMBDOWN,
		THUMBUP,
		TWOFINGERSPINCH,
		V_SIGN,
		WAVE
	};
	public int AveragedFrameCount = 20;
	private Dictionary<string, HAND_GESTURE> stringGestureMap;
	private Queue<HAND_GESTURE> detectedLeftGestures;
	private Queue<HAND_GESTURE> detectedRightGestures;

	// Use this for initialization
	void Start() {
        MaxHands = 2;
		NumHands = 0;
        SmoothingFactor = 14;
        myJoints = new GameObject[MaxHands, PXCMHandData.NUMBER_OF_JOINTS];
        myBones = new GameObject[MaxHands, PXCMHandData.NUMBER_OF_JOINTS];
        sumOfJointPositions = new Vector3[MaxHands, PXCMHandData.NUMBER_OF_JOINTS];
        avgQueue = new Queue();
        handIds = new int[MaxHands];
        bodySide = new PXCMHandData.BodySideType[MaxHands];

		stringGestureMap = new Dictionary<string, HAND_GESTURE> ();
		stringGestureMap.Add ("", HAND_GESTURE.NONE);
		stringGestureMap.Add ("fist", HAND_GESTURE.FIST);
		stringGestureMap.Add ("full_pinch", HAND_GESTURE.FULLPINCH);
		stringGestureMap.Add ("spreadfingers", HAND_GESTURE.SPREADFINGERS);
		stringGestureMap.Add ("swipe", HAND_GESTURE.SWIPE);
		stringGestureMap.Add ("tap", HAND_GESTURE.TAP);
		stringGestureMap.Add ("thumb_down", HAND_GESTURE.THUMBDOWN);
		stringGestureMap.Add ("thumb_UP", HAND_GESTURE.THUMBUP);
		stringGestureMap.Add ("two_fingers_pinch", HAND_GESTURE.TWOFINGERSPINCH);
		stringGestureMap.Add ("v_sign", HAND_GESTURE.V_SIGN);
		stringGestureMap.Add ("wave", HAND_GESTURE.WAVE);

		detectedLeftGestures = new Queue<HAND_GESTURE>();
		detectedRightGestures = new Queue<HAND_GESTURE>();

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

	public void DisplaySmoothenedJoints(PXCMHandData _outputData, PXCMHandData.JointData[,] _myJointData, int[] _handIds, PXCMHandData.BodySideType[] _bodySides, int num_hands)
    {
//		lefthandgesture = "";
//		righthandgesture = "";
		NumHands = num_hands;
		leftPresent = false;
		rightPresent = false;

		float factor = 10.0f;
		float xscale = (Screen.width / 640.0f)/bonescale.x, yscale = (Screen.height/480.0f)/bonescale.y;
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
                        sumOfJointPositions[i, j] += new Vector3(-1 * temp[i, j].positionWorld.x * xscale, temp[i, j].positionWorld.y * yscale, temp[i, j].positionWorld.z * 2.0f);
                    }
                    else
                        myJoints[i, j].SetActive(false);

		for (int i = 0; i < num_hands; i++) {
			if (bodySide [i] == PXCMHandData.BodySideType.BODY_SIDE_LEFT) {
					// Left Hand
					leftPalmCenterJoint = (sumOfJointPositions [i, 1] / avgQueue.Count) * factor;
					leftPresent = true;
			} else if (bodySide [i] == PXCMHandData.BodySideType.BODY_SIDE_RIGHT) {
					// Right Hand
					rightPalmCenterJoint = (sumOfJointPositions [i, 1] / avgQueue.Count) * factor;
					rightPresent = true;
			}
		}

		for (int i = 0; i < MaxHands; i++)
		{
			palmCenterPos = (sumOfJointPositions[i, 1] / avgQueue.Count) * factor;

			for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
            {
				pos = (sumOfJointPositions[i, j] / avgQueue.Count) * factor;
				//Debug.Log ("Joint " + j + ": " + (pos - palmCenterPos).ToString ());
                
				myJoints[i, j].transform.localPosition = pos;
//				if (j==1) Debug.Log("pos: "+pos.ToString()+",pcp:"+palmCenterPos.ToString()+",lpc:"+leftPalmCenterJoint.ToString()+",rpc:"+rightPalmCenterJoint.ToString());
                sumOfJointPositions[i, j] = Vector3.zero;
//				if (j==1) Debug.Log("After pos: "+pos.ToString()+",pcp:"+palmCenterPos.ToString()+",lpc:"+leftPalmCenterJoint.ToString()+",rpc:"+rightPalmCenterJoint.ToString());
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

		if (!leftPresent) {
				myGestureTextLeft.GetComponent<Text> ().text = "";
				lefthandgesture = "";
		}
		if (!rightPresent) {
				myGestureTextRight.GetComponent<Text> ().text = "";
				lefthandgesture = "";
		}
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
			_bone.transform.localScale = new Vector3 (bonescale.x, (_nextJoint.transform.position - _prevJoint.transform.position).magnitude - (_prevJoint.transform.position - _nextJoint.transform.position).magnitude / 2f, bonescale.y);
			
			// Update Rotation
			_bone.transform.rotation = Quaternion.FromToRotation (Vector3.up, _nextJoint.transform.position - _prevJoint.transform.position);
		}
		
	}

	
	public void DisplayGestures(PXCMHandData.GestureData _gestureData)
	{
		gestureData = _gestureData;
		
		//		myGestureTextLeft.GetComponent<Text> ().text = "Left Hand: " + getLeftHandGesture ();
		//		myGestureTextRight.GetComponent<Text> ().text = "Right Hand: " + getRightHandGesture ();
		
		for (int i = 0; i < MaxHands; i++)
			if (gestureData.handId == handIds[i])
			{
				if (bodySide[i] == PXCMHandData.BodySideType.BODY_SIDE_LEFT)
				{
					lefthandgesture = gestureData.name.ToString();
					//				myGestureTextLeft.GetComponent<Text>().text =  "Left Hand : "+ gestureData.name.ToString();
//					if(detectedLeftGestures.Count >= AveragedFrameCount)
//						detectedLeftGestures.Dequeue();
//					detectedLeftGestures.Enqueue(stringGestureMap[lefthandgesture]);
					//				myGestureTextLeft.GetComponent<Text>().text =  "Left Hand : "+ getAveragedLeftHandGesture()+"-"+gestureData.name.ToString();
				}
				else if (bodySide[i] == PXCMHandData.BodySideType.BODY_SIDE_RIGHT)
				{
					righthandgesture = gestureData.name.ToString();
					//				myGestureTextRight.GetComponent<Text>().text = "Right Hand : "+ gestureData.name.ToString(); 
//					if(detectedRightGestures.Count >= AveragedFrameCount)
//						detectedRightGestures.Dequeue();
//					detectedRightGestures.Enqueue(stringGestureMap[righthandgesture]);
					//				myGestureTextRight.GetComponent<Text>().text = "Right Hand : "+ getAveragedRightHandGesture()+"-"+gestureData.name.ToString(); 
				}
				break;
			}
	}
	
	public void DisplayGest()
	{
		myGestureTextLeft.GetComponent<Text>().text =  "Left Hand : "+ NumHands.ToString()+ " " + getLeftHandGesture();	
		myGestureTextRight.GetComponent<Text>().text = "Right Hand : "+ getRightHandGesture(); 
	}

	public int getNumHandsDetected() {
		return NumHands;
	}

	public string getLeftHandGesture()
	{
		if (leftPresent)
			return lefthandgesture;
		else
			return null;
	}
	public string getRightHandGesture()
	{
		if (rightPresent)
			return righthandgesture;
		else
			return null;
	}
	
	public string getAveragedLeftHandGesture ()
	{
		HAND_GESTURE[] gestures = detectedLeftGestures.ToArray ();
		
		Dictionary<HAND_GESTURE, int> count = new Dictionary<HAND_GESTURE, int> ();
		HAND_GESTURE bestG = HAND_GESTURE.NONE;
		int bestC = 0;
		
		for (int i=0; i<gestures.GetLength(0); i++) {
			if(count.ContainsKey(gestures[i]))
				count[gestures[i]] ++;
			else
				count[gestures[i]] = 1;
			if(count[gestures[i]] >= bestC) {
				bestC = count[gestures[i]];
				bestG = gestures[i];
			}
		}
		return bestG.ToString ();
	}
	public string getAveragedRightHandGesture ()
	{
		HAND_GESTURE[] gestures = detectedRightGestures.ToArray ();
		
		Dictionary<HAND_GESTURE, int> count = new Dictionary<HAND_GESTURE, int> ();
		HAND_GESTURE bestG = HAND_GESTURE.NONE;
		int bestC = 0;
		
		for (int i=0; i<gestures.GetLength(0); i++) {
			if(count.ContainsKey(gestures[i]))
				count[gestures[i]] ++;
			else
				count[gestures[i]] = 1;
			if(count[gestures[i]] >= bestC) {
				bestC = count[gestures[i]];
				bestG = gestures[i];
			}
		}
		return bestG.ToString ();
	}

	public bool queryLeftHand2DCoordinates(out Vector2 point)
	{
		if (leftPresent) {
			Vector3 pos = transform.TransformPoint (leftPalmCenterJoint);
			point = Camera.main.camera.WorldToScreenPoint(pos);
			return true;
		}
		else
		{
			point = Vector2.zero;
			return false;
		}
	}

	public bool queryRightHand2DCoordinates(out Vector2 point)
	{
		if (rightPresent) {
			Vector3 pos = transform.TransformPoint (rightPalmCenterJoint);
			point = Camera.main.camera.WorldToScreenPoint(pos);
			return true;
		}
		else
		{
			point = Vector2.zero;
			return false;
		}
	}

	public bool queryLeftHand3DCoordinates(out Vector3 point)
	{
		if (leftPresent) {
				point = leftPalmCenterJoint;
				return true;
		} else {
				point = Vector3.zero;
				return false;
		}
	}

	public bool queryRightHand3DCoordinates(out Vector3 point)
	{
		if (rightPresent) {
				point = rightPalmCenterJoint;
				return true;
		} else {
				point = Vector3.zero;
				return false;
		}
	}
	public void makeNull()
	{
		outputData = null;
//		lefthandgesture = "";
//		righthandgesture = "";
		leftPresent = false;
		rightPresent = false;
//		myGestureTextLeft.GetComponent<Text> ().text = "";
//		myGestureTextRight.GetComponent<Text> ().text = "";
	}
}
