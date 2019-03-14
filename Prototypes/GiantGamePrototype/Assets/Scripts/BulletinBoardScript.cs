using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBoardScript : MonoBehaviour
{
    public static BulletinBoardScript Instance;

    public bool inUse = false;

    public GameObject ActivateButton;

    public Transform SlimeHolder;
    public GameObject CurrentSlime;

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
        if (!inUse)
        {
            ActivateButton.SetActive(true);
        }
    }

    public void Pressed()
    {
        ActivateButton.SetActive(false);
        inUse = true;
        Camera.main.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.Dynamic;
        Camera.main.GetComponent<CameraControl>().DynamicOrtho = 0.6f;
        Camera.main.GetComponent<CameraControl>().DynamicPosition = this.transform.position + (Vector3.back * 10) + (Vector3.up * 0.175f);
    }
}
