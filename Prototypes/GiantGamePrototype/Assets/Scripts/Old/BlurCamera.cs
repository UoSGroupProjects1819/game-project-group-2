using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class BlurCamera : MonoBehaviour {

    Camera parentCamera;
    Camera thisCamera;

    public float zeroAt;
    public float maxAt;
    public float maxBlurAmt;

	// Use this for initialization
	void Start () {
        parentCamera = this.transform.parent.GetComponent<Camera>();
        thisCamera = this.GetComponent<Camera>();
	}
	
	// Update IM called once per frame
	void Update () {
        thisCamera.orthographicSize = parentCamera.orthographicSize;

        if(thisCamera.orthographicSize < zeroAt)
        {
            BlurOptimized blur = this.GetComponent<BlurOptimized>();
            blur.enabled = true;
            float maxDifference = maxAt - zeroAt;
            float currentDifference = thisCamera.orthographicSize - zeroAt;
            blur.blurSize = ((1 / maxDifference) * currentDifference) * maxBlurAmt;
        }
        else
        {
            this.GetComponent<BlurOptimized>().enabled = false;
        }
    }
}
