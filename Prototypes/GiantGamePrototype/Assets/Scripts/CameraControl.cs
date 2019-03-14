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
        IslandSelect,
        Dynamic
    }

    public CameraPositions currentCameraPosition;
    public CameraPositions LastCameraPosition;

    public float speed;
    [Space(10)]
    public Vector3 DynamicPosition;
    public float DynamicOrtho;
    [Space(10)]
    public Vector3 IslandViewPos;
    public float IslandViewOrtho;
    [Space(10)]
    public Vector3 IslandSelectPos;
    public float IslandSelectOrtho;

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

    float autoZoomSpeed = 0;
    float autoMoveSpeed = 0;

    bool touchedSinceLock;

    TouchManager TM;

    // Use this for initialization
    void Start ()
    {
        thisCamera = this.GetComponent<Camera>();
        lastFreeCameraOrtho = thisCamera.orthographicSize;
        lastFreeCameraPosition = this.transform.position;
        TM = TouchManager.Instance;
    }

    private void LateUpdate()
    {
        switch (currentCameraPosition)
        {
            case CameraPositions.TouchControl:
                if (!touchedSinceLock && !WorldManager.Instance.selecting)
                {
                    ResetCameraPosition();
                }
                break;

            case CameraPositions.IslandView:
                MoveToIslandView();
                break;

            case CameraPositions.IslandSelect:
                MoveToIslandSelect();
                break;

            case CameraPositions.Dynamic:
                DynamicMoveCamera();
                break;
        }

        LastCameraPosition = currentCameraPosition;
    }

    public void SetWorldOffset( float newOffset)
    {
        worldPositionOffset = newOffset;
        this.transform.position = new Vector3(newOffset, this.transform.position.y, this.transform.position.z);

        lastFreeCameraPosition = this.transform.position;
    }

    public void ScreenPinch(Vector2 dragAmt, float pinchAmt)
    {
        if (InventoryManager.Instance.inventoryPanel.activeSelf) { InventoryManager.Instance.inventoryPanel.SetActive(false); }
        if (WorldManager.Instance.selecting) { return; }

        if (TM.touchesLastFrame == 2)
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
            //Vector2 touch0, touch1;
            //touch0 = Input.GetTouch(0).position;
            //touch1 = Input.GetTouch(1).position;
        }
    }

    public void ScreenDrag(Vector2 touchPos)
    {
        if (currentCameraPosition == CameraPositions.Dynamic || currentCameraPosition == CameraPositions.IslandView) { return; }

        if (InventoryManager.Instance.inventoryPanel.activeSelf) { InventoryManager.Instance.inventoryPanel.SetActive(false); }

        if (TM.touchesLastFrame == 1)
        {
            Vector2 panAmt = (((TM.lastSingleTouchPoint - touchPos) / Screen.width) * 900);

            this.transform.position += new Vector3(panAmt.x, panAmt.y, 0) * panSpeed * thisCamera.orthographicSize;
            if (WorldManager.Instance.selecting)
            {
                this.transform.position = new Vector3(this.transform.position.x, Mathf.Clamp(this.transform.position.y, 1, 1), this.transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, minPanBound.x + worldPositionOffset, maxPanBound.x + worldPositionOffset), Mathf.Clamp(this.transform.position.y, minPanBound.y, maxPanBound.y), this.transform.position.z);
            }
            TM.lastSingleTouchPoint = touchPos;

            touchedSinceLock = true;
            lastFreeCameraOrtho = thisCamera.orthographicSize;
            lastFreeCameraPosition = this.transform.position;
        }
    }

    public void ResetCameraPosition()
    {
        //Debug.Log("Resetting");

        if (currentCameraPosition != LastCameraPosition)
        {
            autoMoveSpeed = Vector3.Distance(this.transform.position, lastFreeCameraPosition);
            autoZoomSpeed = Mathf.Abs(thisCamera.orthographicSize - lastFreeCameraOrtho);
        }

        this.transform.position = Vector3.MoveTowards(transform.position, lastFreeCameraPosition, Time.deltaTime * autoMoveSpeed);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, lastFreeCameraOrtho, Time.deltaTime * autoZoomSpeed);
    }

    public void MoveToIslandView()
    {
        if(currentCameraPosition != LastCameraPosition)
        {
            autoMoveSpeed = Vector3.Distance(this.transform.position, IslandViewPos);
            autoZoomSpeed = Mathf.Abs(thisCamera.orthographicSize - IslandViewOrtho);
        }
        this.transform.position = Vector3.MoveTowards(transform.position, IslandViewPos + WorldManager.Instance.SelectedIsland.transform.position, Time.deltaTime * autoMoveSpeed * 1f);
        thisCamera.orthographicSize = Mathf.MoveTowards(thisCamera.orthographicSize, IslandViewOrtho, Time.deltaTime * autoZoomSpeed * 1f);
        touchedSinceLock = false;
    }

    public void MoveToIslandSelect()
    {
        //this.transform.position = Vector3.MoveTowards(transform.position, IslandSelectPos + WorldManager.Instance.SelectedIsland.transform.position, Time.deltaTime * speed * 1f);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, IslandSelectOrtho, Time.deltaTime * speed);
        touchedSinceLock = false;
    }

    public void DynamicMoveCamera ()
    {
        if (currentCameraPosition != LastCameraPosition)
        {
            autoMoveSpeed = Vector3.Distance(this.transform.position, DynamicPosition);
            autoZoomSpeed = Mathf.Abs(this.GetComponent<Camera>().orthographicSize - DynamicOrtho);
        }
        this.transform.position = Vector3.MoveTowards(transform.position, DynamicPosition, Time.deltaTime * autoMoveSpeed * 1f);
        thisCamera.orthographicSize = Mathf.MoveTowards(thisCamera.orthographicSize, DynamicOrtho, Time.deltaTime * autoZoomSpeed * 1f);
        touchedSinceLock = false;
    }
}
