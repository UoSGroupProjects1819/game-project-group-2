using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class parralax : MonoBehaviour
{
    public GameObject mainCamera;

    public float maxOrthoSize;
    public float minOrthoSize;

    public float maxSize;
    public float minSize;

    public float cameraMaxLeft;
    public float cameraMaxRight;

    public Transform parentIsland;

    public float maxBound;
    public float minBound;

    // Use this for initialization
    void Start()
    {

    }

    // Update IM called once per frame
    void Update()
    {
        float orthoRange = maxOrthoSize - minOrthoSize;
        float orthoPercent = (mainCamera.GetComponent<Camera>().orthographicSize - minOrthoSize) / orthoRange;

        float sizeRange = maxSize - minSize;

        float newSize = minSize + (sizeRange * orthoPercent);

        newSize = Mathf.Clamp(newSize, minSize, maxSize);

        this.transform.localScale = new Vector3(newSize, newSize, 1);

        float offset = parentIsland.position.x;

        float newBound = Mathf.MoveTowards(minBound, maxBound, (minBound - maxBound) * orthoPercent);

        float CameraX = mainCamera.transform.position.x - offset;

        float cameraPercent;

        if (CameraX < 0)
        {
            cameraPercent = CameraX / cameraMaxLeft;
            cameraPercent = -cameraPercent;
        }
        else
        {
            cameraPercent = CameraX / cameraMaxRight;
        }

        Vector3 newPos = this.transform.localPosition;

        newPos.x = newBound * cameraPercent;

        this.transform.localPosition = newPos;
    }
}