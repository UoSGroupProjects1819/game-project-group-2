using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositions : MonoBehaviour {

    Camera thisCamera;

    public float speed;
    [Space(10)]
    public bool LockCamera;
    public Vector3 Dynamic;
    public float DynamicOrtho;
    Vector3 targetPos;
    float targetSize;

    Vector3 lastFreeCameraPosition;
    float lastFreeCameraOrtho;

    [Space(10)]
    public float zoomSpeed;
    public float minCameraOrtho;
    public float maxCameraOrtho;
    //public float zoomPanAmt;

    [Space(5)]
    public float panSpeed;
    public Vector2 minPanBound;
    public Vector2 maxPanBound;

    bool touchedSinceLock;

    TouchController TC;

    // Use this for initialization
    void Start ()
    {
        thisCamera = this.GetComponent<Camera>();
        lastFreeCameraOrtho = thisCamera.orthographicSize;
        lastFreeCameraPosition = this.transform.position;
        TC = TouchController.Instance;
    }

    private void LateUpdate()
    {
        if (LockCamera)
        {
            ControlledMoveCamera();
        }
        else
        {
            if (!touchedSinceLock)
            {
                ResetCameraPosition();
            }
        }
    }

    public void ScreenPinch(Vector2 dragAmt, float pinchAmt)
    {
        if (InventoryScript.Instance.inventoryPanel.activeSelf) { return; }

        if (TC.touchesLastFrame == 2)
        {
            float zoomAmt = pinchAmt / thisCamera.fieldOfView;

            thisCamera.orthographicSize += zoomAmt * (1 / zoomSpeed);

            thisCamera.orthographicSize = Mathf.Clamp(thisCamera.orthographicSize, minCameraOrtho, maxCameraOrtho);
            this.transform.position += new Vector3(dragAmt.x, dragAmt.y, 0) * panSpeed * thisCamera.orthographicSize;
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, minPanBound.x, maxPanBound.x), Mathf.Clamp(this.transform.position.y, minPanBound.y, maxPanBound.y), this.transform.position.z);
        }
        else
        {
            touchedSinceLock = true;
            Vector2 touch0, touch1;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
        }
    }

    public void ScreenDrag(Vector2 touchPos)
    {
        if (LockCamera) { return; }

        if (InventoryScript.Instance.inventoryPanel.activeSelf) { return; }

        if (TC.touchesLastFrame == 1)
        {
            Vector2 panAmt = TC.lastSingleTouchPoint - touchPos;

            this.transform.position += new Vector3(panAmt.x, panAmt.y, 0) * panSpeed * thisCamera.orthographicSize;
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, minPanBound.x, maxPanBound.x), Mathf.Clamp(this.transform.position.y, minPanBound.y, maxPanBound.y), this.transform.position.z);

            TC.lastSingleTouchPoint = touchPos;

            touchedSinceLock = true;
            lastFreeCameraOrtho = thisCamera.orthographicSize;
            lastFreeCameraPosition = this.transform.position;
        }
    }

    public void ResetCameraPosition()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, lastFreeCameraPosition, Time.deltaTime * speed);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, lastFreeCameraOrtho, Time.deltaTime * speed);
    }

    public void ControlledMoveCamera ()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, Dynamic, Time.deltaTime * speed * 2.5f);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, DynamicOrtho, Time.deltaTime * speed);
        touchedSinceLock = false;
    }
}
