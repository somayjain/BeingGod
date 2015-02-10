/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/

using UnityEngine;
using System.Collections;

public class FaceTracking: MonoBehaviour {

    public GameObject colorPlane; //Object to display color feed texture on
    public Texture2D colorTexture2D; //Color Texture
    private PXCMImage colorImage = null;//PXCMImage for color 

    private FaceRenderer faceRenderer; //Rendererer for Visualization
    private int MaxPoints = 78;
    private PXCMSenseManager psm; //SenseManager Instance
    private pxcmStatus sts; //Check error status
    private PXCMFaceModule faceAnalyzer; //FaceModule Instance


    /// <summary>
    /// Use this for initialization
    /// Unity function called on the frame when a script is enabled 
    /// just before any of the Update methods is called the first time.
    /// </summary>
	void Start () {

        faceRenderer = gameObject.GetComponent<FaceRenderer>();

        /* Initialize a PXCMSenseManager instance */
        psm = PXCMSenseManager.CreateInstance();
        if (psm == null)
        {
            Debug.LogError("SenseManager Initialization Failed");
            return;
        }

        /* Enable the color stream of size 640x480 */
        psm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 640, 480);

        /* Enable the face tracking module*/
        sts = psm.EnableFace();
        if (sts != pxcmStatus.PXCM_STATUS_NO_ERROR) Debug.LogError("PXCSenseManager.EnableFace: " + sts);

        /* Retrieve an instance of face to configure */
        faceAnalyzer = psm.QueryFace();
        if (faceAnalyzer == null) Debug.LogError("PXCSenseManager.QueryFace");

        /* Initialize the execution pipeline */
        sts = psm.Init();
        if (sts != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.LogError("PXCMSenseManager.Init Failed");
            OnDisable();
            return;
        }

        /* Retrieve a PXCMFaceConfiguration instance from a face to enable Gestures and Alerts */
        PXCMFaceConfiguration config = faceAnalyzer.CreateActiveConfiguration(); 
		config.detection.isEnabled = true; // 3D detection is the default tracking mode.
		config.landmarks.isEnabled = true;
        config.pose.isEnabled = true;
        config.QueryExpressions().Enable();
        config.QueryExpressions().EnableExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_MOUTH_OPEN);
        config.EnableAllAlerts();
        config.ApplyChanges();
        config.Dispose();

	}



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        /* Make sure PXCMSenseManager Instance is Initialized */
        if (psm == null) return;

        /* Wait until any frame data is available true(aligned) false(unaligned) */
        if (psm.AcquireFrame(true) != pxcmStatus.PXCM_STATUS_NO_ERROR) return;

         /* Retrieve face tracking data if ready */
        faceAnalyzer = psm.QueryFace();
        if (faceAnalyzer != null)
        {
            PXCMFaceData _outputData = faceAnalyzer.CreateOutput();
            if (_outputData != null)
            {
                _outputData.Update();
  
                for (int i = 0; i < _outputData.QueryNumberOfDetectedFaces(); i++)
                {                 
                    PXCMFaceData.Face _iFace = _outputData.QueryFaceByIndex(i);
                    if (_iFace != null)
                    {
                        /* Retrieve 78 Landmark Points */
                        PXCMFaceData.LandmarksData LandmarkData = _iFace.QueryLandmarks();
                        if (LandmarkData != null)
                        {
                            PXCMFaceData.LandmarkPoint[] landmarkPoints = new PXCMFaceData.LandmarkPoint[MaxPoints];
                            if(LandmarkData.QueryPoints(out landmarkPoints))
                                faceRenderer.DisplayJoints2D(landmarkPoints);
                        }

                        /* Retrieve Detection Data */
                        PXCMFaceData.DetectionData detectionData = _iFace.QueryDetection();
                        if (detectionData != null)
                        {
                            PXCMRectI32 rect;
                            if (detectionData.QueryBoundingRect(out rect))
                                faceRenderer.SetDetectionRect(rect);
                        }

                        /* Retrieve Pose Data */
                        PXCMFaceData.PoseData poseData = _iFace.QueryPose();
                        if (poseData != null)
                        {
                            PXCMFaceData.PoseQuaternion poseQuaternion;
                            if (poseData.QueryPoseQuaternion(out poseQuaternion))
                                faceRenderer.DisplayPoseQuaternion(poseQuaternion);
                        }

                        /* Retrieve Expression Data */
                        PXCMFaceData.ExpressionsData expressionData = _iFace.QueryExpressions();
                        if (expressionData != null)
                        {
                            PXCMFaceData.ExpressionsData.FaceExpressionResult expressionResult;
                            if (expressionData.QueryExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_MOUTH_OPEN, out expressionResult))
                                faceRenderer.DisplayExpression(expressionResult, PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_MOUTH_OPEN);
                        }

                    }
                }
                
                /* Retrieve Alert Data */
                PXCMFaceData.AlertData _alertData;
                for (int i = 0; i < _outputData.QueryFiredAlertsNumber(); i++)
                    if (_outputData.QueryFiredAlertData(i, out _alertData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
                        faceRenderer.DisplayAlerts(_alertData);
            }
            _outputData.Dispose();
        }

        /* Retrieve a sample from the camera */
        PXCMCapture.Sample sample = psm.QueryFaceSample();
        if (sample != null)
        {
            colorImage = sample.color;
            if (colorImage != null)
            {
                if (colorTexture2D == null)
                {
                    /* If not allocated, allocate a Texture2D */
                    colorTexture2D = new Texture2D(colorImage.info.width, colorImage.info.height, TextureFormat.ARGB32, false);

                    /* Associate the Texture2D with a gameObject */
                    colorPlane.renderer.material.mainTexture = colorTexture2D;
                    //colorPlane.renderer.material.mainTextureScale = new Vector2(-1f, 1f);
                }

                /* Retrieve the image data in Texture2D */
                PXCMImage.ImageData colorImageData;
                colorImage.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out colorImageData);
                colorImageData.ToTexture2D(0, colorTexture2D);
                colorImage.ReleaseAccess(colorImageData);

                /* Apply the texture to the GameObject to display on */
                colorTexture2D.Apply();
            }
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
		faceAnalyzer.Dispose();
        if (psm == null) return;
        psm.Dispose();
    }
}
