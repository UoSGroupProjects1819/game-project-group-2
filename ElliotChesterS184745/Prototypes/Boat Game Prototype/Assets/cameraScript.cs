using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    public GameObject focusPoint;

    public float cameraRotVertOffset;

    public float height;
    public float distance;

	// Use this for initialization
	void Start ()
    {
        this.transform.position = new Vector3(0, height, focusPoint.transform.position.z + distance);
        this.transform.LookAt(focusPoint.transform);
        this.transform.eulerAngles += new Vector3(cameraRotVertOffset, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3(0, height, focusPoint.transform.position.z + distance);
	}
}
