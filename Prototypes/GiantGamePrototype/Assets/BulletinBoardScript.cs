using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBoardScript : MonoBehaviour
{
    public bool inUse = false;
    bool resetCamera = false;

    public GameObject ActivateButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inUse)
        {
            Camera.main.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.Dynamic;
            Camera.main.GetComponent<CameraControl>().DynamicOrtho = 0.5f;
            Camera.main.GetComponent<CameraControl>().DynamicPosition = this.transform.position + (Vector3.back * 10);
        }
        else
        {
            if (!resetCamera)
            {
                Camera.main.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.IslandView;
            }
        }
    }

    public void Pressed()
    {
        ActivateButton.SetActive(false);
        inUse = true;
    }
}
