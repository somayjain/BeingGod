using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class trackHand : MonoBehaviour {

	private PXCMHandData.JointData[,] joints;
	private int[] handIds; //Array of HandIds for HandRenderer
    private PXCMHandData.BodySideType[] bodySides; //Array of BodySides for HandRenderer
    private HandRenderer handRenderer; //Rendererer for Visualization

	// Use this for initialization
	void Start () {
		handIds = new int[2];
        bodySides = new PXCMHandData.BodySideType[2];
        handRenderer = gameObject.GetComponent<HandRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {

		PXCMHandData _outputData = SenseToolkitManager.Instance.HandDataOutput;
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

				//Debug.Log(_handData.QueryMassCenterImage().x.ToString() + " " + _handData.QueryMassCenterImage().y.ToString());

	        }
	        handRenderer.DisplaySmoothenedJoints(joints, handIds, bodySides);
	    }
	}

	public void dummyHandDetected (){
	}
}

