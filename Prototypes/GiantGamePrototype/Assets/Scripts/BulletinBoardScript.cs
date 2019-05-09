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

    public bool pickingSlime = false;

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

    public Image InstructionPanel;
    public void ZoomForCreature()
    {
        pickingSlime = true;
        ActivateButton.SetActive(false);
        Camera.main.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.IslandView;
        InstructionPanel.gameObject.SetActive(true);
        InstructionPanel.GetComponentInChildren<Text>().text = "Before you can start a quest, you have to pick a slime";
    }

    public void SetSlime(GameObject slime)
    {
        if (slime != null)
        {
            CurrentSlime = slime;
            CurrentSlime.transform.position = (Vector2)SlimeHolder.position;
            CurrentSlime.GetComponent<CreatureScript>().WaitingForQuest = true;
        }
        InstructionPanel.gameObject.SetActive(false);
        ActivateButton.SetActive(false);
        inUse = true;
        Camera.main.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.Dynamic;
        Camera.main.GetComponent<CameraControl>().DynamicOrtho = 0.6f;
        Camera.main.GetComponent<CameraControl>().DynamicPosition = this.transform.position + (Vector3.back * 10) + (Vector3.up * 0.175f);
        pickingSlime = false;
    }
}
