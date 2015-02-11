/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;

public class HandTracking : MonoBehaviour {

    private HandRenderer handRenderer; //Rendererer for Visualization

    private PXCMSenseManager psm; //SenseManager Instance
    private pxcmStatus sts; //Check error status
    private PXCMHandModule handAnalyzer; //HandModule Instance

    private PXCMHandData.JointData[,] joints; //Array of Joints
    private int[] handIds; //Array of HandIds for HandRenderer
    private PXCMHandData.BodySideType[] bodySides; //Array of BodySides for HandRenderer

    /// <summary>
    /// Use this for initialization
    /// Unity function called on the frame when a script is enabled 
    /// just before any of the Update methods is called the first time.
    /// </summary>
	void Start () {

        handRenderer = gameObject.GetComponent<HandRenderer>();
        handIds = new int[2];
        bodySides = new PXCMHandData.BodySideType[2];

        /* Initialize a PXCMSenseManager instance */
        psm = PXCMSenseManager.CreateInstance();
        if (psm == null)
        {
            Debug.LogError("SenseManager Initialization Failed");
            return;
        }

        /* Enable the hand tracking module*/
        sts = psm.EnableHand();
        if (sts != pxcmStatus.PXCM_STATUS_NO_ERROR) Debug.LogError("PXCSenseManager.EnableHand: " + sts);

        /* Retrieve an instance of hand to configure */
        handAnalyzer = psm.QueryHand();
        if (handAnalyzer == null) Debug.LogError("PXCSenseManager.QueryHand");

        /* Initialize the execution pipeline */
        sts = psm.Init();
        if (sts != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.LogError("PXCMSenseManager.Init Failed");
            OnDisable();
            return;
        }

        /* Retrieve a PXCMHandConfiguration instance from a hand to enable Gestures and Alerts */
        PXCMHandConfiguration config = handAnalyzer.CreateActiveConfiguration();
        config.EnableAllGestures();
        config.EnableAllAlerts();
        config.ApplyChanges();
        config.Dispose();

	}

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
	void Update () {

        /* Make sure PXCMSenseManager Instance is Initialized */
        if (psm == null) return;

        /* Wait until any frame data is available true(aligned) false(unaligned) */
        if (psm.AcquireFrame(false,0) != pxcmStatus.PXCM_STATUS_NO_ERROR) return;

        /* Retrieve am instance of hand tracking Module */
        handAnalyzer = psm.QueryHand();
        if (handAnalyzer != null)
        {
            /* Retrieve an instance of hand tracking Data */
            PXCMHandData _outputData = handAnalyzer.CreateOutput();
            if (_outputData != null)
            {
                _outputData.Update();

                /* Retrieve Hand Joints*/      
                joints = new PXCMHandData.JointData[2, PXCMHandData.NUMBER_OF_JOINTS];
                for (int i = 0; i < _outputData.QueryNumberOfHands(); i++)
                {
                    PXCMHandData.IHand _handData;
                    _outputData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_FIXED, i, out _handData);
                    for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
                        _handData.QueryTrackedJoint((PXCMHandData.JointType)j, out joints[i, j]);

                    handIds[i] = _handData.QueryUniqueId();
                    bodySides[i] = _handData.QueryBodySide();

					Debug.Log(_handData.QueryMassCenterImage().x.ToString() + " " + _handData.QueryMassCenterImage().y.ToString());

                }

                handRenderer.DisplaySmoothenedJoints(joints, handIds, bodySides);
               //handRenderer.DisplayJoints(joints, handIds, bodySides);

                /* Retrieve Gesture Data */
                PXCMHandData.GestureData _gestureData;
                for (int i = 0; i < _outputData.QueryFiredGesturesNumber(); i++)
                    if (_outputData.QueryFiredGestureData(i, out _gestureData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
                        handRenderer.DisplayGestures(_gestureData);

                /* Retrieve Alert Data */
                PXCMHandData.AlertData _alertData;
                for (int i = 0; i < _outputData.QueryFiredAlertsNumber(); i++)
                    if (_outputData.QueryFiredAlertData(i, out _alertData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
                        handRenderer.DisplayAlerts(_alertData);


			}
			_outputData.Dispose();
        }

        /* Realease the frame to process the next frame */
        psm.ReleaseFrame();

	}

    /// <summary>
    /// Unity function that is called when the behaviour becomes disabled () or inactive.
    /// Used for clean-up in the end
    /// </summary>
    void OnDisable()
    {
		handAnalyzer.Dispose();
        if (psm == null) return;
        psm.Dispose();
    }
}
