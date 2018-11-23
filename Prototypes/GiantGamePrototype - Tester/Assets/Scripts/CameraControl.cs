using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    Camera thisCamera;

    [HideInInspector]
    public float worldPositionOffset;

    public enum CameraPositions
    {
        TouchControl,
        IslandView,
        Dynamic
    }

    public CameraPositions currentCameraPosition;

    public float speed;
    [Space(10)]
    public Vector3 Dynamic;
    public float DynamicOrtho;
    [Space(10)]
    public Vector3 IslandViewPos;
    public float IslandViewOrtho;

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
        switch (currentCameraPosition)
        {
            case CameraPositions.TouchControl:
                if (!touchedSinceLock)
                {
                    ResetCameraPosition();
                }
                break;

            case CameraPositions.IslandView:
                MoveToIslandView();
                break;

            case CameraPositions.Dynamic:
                DynamicMoveCamera();
                break;
        }
    }

    public void SetWorldOffset( float newOffset)
    {
        worldPositionOffset = newOffset;
        this.transform.position = new Vector3(newOffset, this.transform.position.y, this.transform.position.z);

        lastFreeCameraPosition = this.transform.position;
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
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, minPanBound.x + worldPositionOffset, maxPanBound.x + worldPositionOffset), Mathf.Clamp(this.transform.position.y, minPanBound.y, maxPanBound.y), this.transform.position.z);
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
        if (currentCameraPosition == CameraPositions.Dynamic || currentCameraPosition == CameraPositions.IslandView) { return; }

        if (InventoryScript.Instance.inventoryPanel.activeSelf) { return; }

        if (TC.touchesLastFrame == 1)
        {
            Vector2 panAmt = TC.lastSingleTouchPoint - touchPos;

            this.transform.position += new Vector3(panAmt.x, panAmt.y, 0) * panSpeed * thisCamera.orthographicSize;
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, minPanBound.x + worldPositionOffset, maxPanBound.x + worldPositionOffset), Mathf.Clamp(this.transform.position.y, minPanBound.y, maxPanBound.y), this.transform.position.z);

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

    public void MoveToIslandView()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, IslandViewPos + WorldSelector.Instance.SelectedIsland.transform.position, Time.deltaTime * speed * 1f);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, IslandViewOrtho, Time.deltaTime * speed);
        touchedSinceLock = false;
    }

    public void DynamicMoveCamera ()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, Dynamic, Time.deltaTime * speed * 1f);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, DynamicOrtho, Time.deltaTime * speed);
        touchedSinceLock = false;
    }
}
