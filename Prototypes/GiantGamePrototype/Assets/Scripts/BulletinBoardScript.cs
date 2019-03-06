using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBoardScript : MonoBehaviour
{
    public static BulletinBoardScript Instance;

    public bool inUse = false;
    bool resetCamera = false;

    public GameObject ActivateButton;

    private void Awake()
    {
        Instance = this;
    }

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
            Camera.main.GetComponent<CameraControl>().DynamicOrtho = 0.4f;
            Camera.main.GetComponent<CameraControl>().DynamicPosition = this.transform.position + (Vector3.back * 10);
            resetCamera = false;
        }
        else
        {
            if (!resetCamera)
            {
                Camera.main.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.IslandView;
                resetCamera = true;
            }
        }
    }

    public void Pressed()
    {
        ActivateButton.SetActive(false);
        inUse = true;
    }
}
