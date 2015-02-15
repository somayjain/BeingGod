using UnityEngine;
using System.Collections;
using RSUnityToolkit;
using UnityEngine.UI;

public class trackHand : MonoBehaviour {

	private PXCMHandData.JointData[,] joints;
	private int[] handIds; //Array of HandIds for HandRenderer
    private PXCMHandData.BodySideType[] bodySides; //Array of BodySides for HandRenderer
    private HandRenderer handRenderer; //Rendererer for Visualization
	private PXCMHandData _outputData;

	private bool isleftCalibrated, isrightCalibrated;
	// Use this for initialization
	void Start () {
		handIds = new int[2];
        bodySides = new PXCMHandData.BodySideType[2];
        handRenderer = gameObject.GetComponent<HandRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// For testing only.
		//handRenderer.queryLeftHand2DCoordinates ();
		
		_outputData = SenseToolkitManager.Instance.HandDataOutput;
		if (_outputData != null)
		{
            _outputData.Update();

			/* Retrieve Hand Joints*/      
	        joints = new PXCMHandData.JointData[2, PXCMHandData.NUMBER_OF_JOINTS];
	        for (int i = 0; i < _outputData.QueryNumberOfHands(); i++)
	        {
	            PXCMHandData.IHand _handData;
	            _outputData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_FIXED, i, out _handData);

				//Debug.Log(_handData.IsCalibrated().ToString());

				//if(_handData!=null){}// Debug.Log(_handData.ToString());

				//else { Debug.Log ("No data"); continue; }
	            
				for (int j = 0; j < PXCMHandData.NUMBER_OF_JOINTS; j++)
	                _handData.QueryTrackedJoint((PXCMHandData.JointType)j, out joints[i, j]);

	            handIds[i] = _handData.QueryUniqueId();
	            bodySides[i] = _handData.QueryBodySide();

				if (!_handData.IsCalibrated()) 
				{					
					if(bodySides[i] == PXCMHandData.BodySideType.BODY_SIDE_LEFT)
					{
						Debug.Log("Left Uncalibrated !!");
						isleftCalibrated = false;
						handRenderer.myGestureTextLeft.GetComponent<Text> ().text = "Hand Not Calibrated";
					}
					else if(bodySides[i] == PXCMHandData.BodySideType.BODY_SIDE_RIGHT)
					{
						Debug.Log("Right Uncalibrated !!"); 
						isrightCalibrated = false;
						handRenderer.myGestureTextRight.GetComponent<Text> ().text = "Hand Not Calibrated";
						
					}
				}
				else
				{
					if(bodySides[i] == PXCMHandData.BodySideType.BODY_SIDE_LEFT)
						isleftCalibrated = true;
					else if(bodySides[i] == PXCMHandData.BodySideType.BODY_SIDE_RIGHT)
						isrightCalibrated = true;
				}
	        }
			handRenderer.DisplaySmoothenedJoints(_outputData, joints, handIds, bodySides, _outputData.QueryNumberOfHands());
			
			/* Retrieve Alert Data */
			/*
			PXCMHandData.AlertData _alertData;
			for (int i = 0; i < _outputData.QueryFiredAlertsNumber(); i++)
				if (_outputData.QueryFiredAlertData(i, out _alertData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
					handRenderer.DisplayAlerts(_alertData);
			*/

			/* Retrieve Gesture Data */
			PXCMHandData.GestureData _gestureData;
			for (int i = 0; i < _outputData.QueryFiredGesturesNumber(); i++)
				if (_outputData.QueryFiredGestureData(i, out _gestureData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
					handRenderer.DisplayGestures(_gestureData);
			
			if(_outputData.QueryNumberOfHands()==0)
				handRenderer.makeNull();
	    }
		else
		{
			handRenderer.makeNull();
		}

		// For testing
		//handRenderer.DisplayGest ();
		displayHandGestures ();
	}
	public void displayHandGestures()
	{
		if(handRenderer.getLeftHandGesture() == null)
		{
			// Hand is not present
			handRenderer.myGestureTextLeft.GetComponent<Text> ().text = "";
		}
		else
		{
			if(isleftCalibrated)
			{
				handRenderer.myGestureTextLeft.GetComponent<Text> ().text = "Left Hand :" + handRenderer.getLeftHandGesture();
			}
			else
			{
				handRenderer.myGestureTextLeft.GetComponent<Text> ().text = "Left Hand Not Calibrated. Spread your fingers.";
			}
		}


		if(handRenderer.getRightHandGesture() == null)
		{
			// Hand is not present
			handRenderer.myGestureTextRight.GetComponent<Text> ().text = "";
		}
		else
		{
			if(isleftCalibrated)
			{
				handRenderer.myGestureTextRight.GetComponent<Text> ().text = "Right Hand :" + handRenderer.getRightHandGesture();
			}
			else
			{
				handRenderer.myGestureTextRight.GetComponent<Text> ().text = "Right Hand Not Calibrated. Spread your fingers.";
			}
		}
	}
	public void dummyHandDetected (){
	}
}

