using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositions : MonoBehaviour {

    public float speed;

    public Vector3 CameraOut;
    public float OrthoOut;

    public Vector3 CameraInLeft;
    public float OrthoInLeft;

    public Vector3 CameraInRight;
    public float OrthoInRight;

    public bool useDynamic;
    public Vector3 Dynamic;
    public float DynamicOrtho;

    Vector3 targetPos;
    float targetSize;

    // Use this for initialization
    void Start () {
        targetPos = CameraOut;
        targetSize = OrthoOut;
    }

    private void Update()
    {
        MoveCamera();
    }

    // Update is called once per frame
    public void MoveCamera ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            targetPos = CameraOut;
            targetSize = OrthoOut;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            targetPos = CameraInLeft;
            targetSize = OrthoInLeft;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            targetPos = CameraInRight;
            targetSize = OrthoInRight;
        }

        if (useDynamic)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, Dynamic, Time.deltaTime * speed);
            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(this.GetComponent<Camera>().orthographicSize, DynamicOrtho, Time.deltaTime * speed);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(this.GetComponent<Camera>().orthographicSize, targetSize, Time.deltaTime * speed);
        }

    }
}
