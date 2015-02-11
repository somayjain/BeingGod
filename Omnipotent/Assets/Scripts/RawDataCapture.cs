/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;

public class RawDataCapture : MonoBehaviour {

    public GameObject depthPlane; //Object to display depth feed texture on
    public GameObject colorPlane; //Object to display color feed texture on
    public Texture2D depthTexture2D; //Depth Texture
    public Texture2D colorTexture2D; //Color Texture

    private PXCMSenseManager psm; //SenseManager Instance
    private pxcmStatus sts; //Check error status
    private PXCMImage depthImage = null;//PXCMImage for depth
    private PXCMImage colorImage = null;//PXCMImage for color 
    
	/// <summary>
    /// Use this for initialization
    /// Unity function called on the frame when a script is enabled 
    /// just before any of the Update methods is called the first time.
	/// </summary>
	void Start () {

        /* Initialize a PXCMSenseManager instance */
        psm = PXCMSenseManager.CreateInstance();
        if (psm == null){
            Debug.LogError("SenseManager Initialization Failed");
            return;
        }

        /* Enable the depth stream of size 640x480 and color stream of size 640x480 */
	    psm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 640, 480);
	    psm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 640, 480);

        /* Initialize the execution pipeline */
        sts = psm.Init();
        if (sts != pxcmStatus.PXCM_STATUS_NO_ERROR){
            Debug.LogError("PXCMSenseManager.Init Failed");
            OnDisable();
            return;
        }

	}

	/// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update () {

        /* Make sure PXCMSenseManager Instance is Initialized */
        if (psm == null) return;

        /* Wait until any frame data is available true(aligned) false(unaligned) */
        if (psm.AcquireFrame(true) != pxcmStatus.PXCM_STATUS_NO_ERROR) return;

        /* Retrieve a sample from the camera */
        PXCMCapture.Sample sample = psm.QuerySample();
        if (sample != null)
        {
            depthImage = sample.depth;
            if (depthImage != null)
            {       
                if (depthTexture2D == null)
                {
                    /* If not allocated, allocate a Texture2D */
                    depthTexture2D = new Texture2D(depthImage.info.width, depthImage.info.height, TextureFormat.ARGB32, false);

                    /* Associate the Texture2D with a gameObject */
                    depthPlane.renderer.material.mainTexture = depthTexture2D;
                    depthPlane.renderer.material.mainTextureScale = new Vector2(-1f, 1f); /* for a mirror effect */
                }

               /* Retrieve the image data in Texture2D */
               PXCMImage.ImageData depthImageData;
               depthImage.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out depthImageData);
               depthImageData.ToTexture2D(0, depthTexture2D); // converts RSSDK image data to Unity Texture2D
               depthImage.ReleaseAccess(depthImageData);

               /* Apply the texture to the GameObject to display on */
               depthTexture2D.Apply();
            }

            colorImage = sample.color;
            if (colorImage != null)
            {
                if (colorTexture2D == null)
                {
                    /* If not allocated, allocate a Texture2D */
                    colorTexture2D = new Texture2D(colorImage.info.width, colorImage.info.height, TextureFormat.ARGB32, false);

                    /* Associate the Texture2D with a gameObject */
                    colorPlane.renderer.material.mainTexture = colorTexture2D;
					colorPlane.renderer.material.mainTextureScale = new Vector2(-1f, 1f);  /* for a mirror effect */
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
        if (psm == null) return;
        psm.Dispose();
    }
	
}
