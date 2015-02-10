/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;

public class HandRenderer : MonoBehaviour {

    public GameObject JointPrefab; //Prefab for Joints
    public GameObject TipPrefab; //Prefab for Finger Tips
    public GameObject BonePrefab; //Prafab for Bones
    public GameObject PalmCenterPrefab;//Prefab for Palm Center

    public GameObject handofGod;

    private int MaxHands; //Max no. of Hands
    private GameObject[,] myJoints; //Array of Joint GameObjects
    private GameObject[,] myBones; //Array of Bone GameObjects
    private int[] handIds; //Array of handIds
    private PXCMHandData.BodySideType[] bodySide; //Array of BodySides
    
    private Queue avgQueue; //contains jointData values over a set of frames
    private Vector3[,] sumOfJointPositions; //sum of pos over a set of frames
    private int SmoothingFactor;

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

    public void DisplaySmoothenedJoints(PXCMHandData.JointData[,] _myJointData, int[] _handIds, PXCMHandData.BodySideType[] _bodySides)
    {
        handIds = _handIds;
        bodySide = _bodySides;

            avgQueue.Enqueue(_myJointData);

            foreach (PXCMHandData.JointData[,] temp in avgQueue)
                for (int i = 0; i < MaxHands; i++)
                    for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
                        if (temp[i, j] != null && _myJointData[i, j] != null)
                        {
                            myJoints[i, j].SetActive(true);
                            sumOfJointPositions[i, j] += new Vector3(-1 * temp[i, j].positionWorld.x * 1.3f, temp[i, j].positionWorld.y, temp[i, j].positionWorld.z);
                        }
                        else
                            myJoints[i, j].SetActive(false);

            for (int i = 0; i < MaxHands; i++)
                for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
                {
                    myJoints[i, j].transform.localPosition = (sumOfJointPositions[i, j] / avgQueue.Count) * 100f;
                    sumOfJointPositions[i, j] = Vector3.zero;
                }

            if (avgQueue.Count >= SmoothingFactor)
                avgQueue.Dequeue();

    }

}
