using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public GameObject TutorialUIPanel;
    public GameObject InGameUIPanel;
    public TextMeshProUGUI TutorialText;

    public bool TutorialActive = false;

    public int tutorialStage = 0;

    public LayerMask[] layersForStages;

    [Header ("Stage 1")]
    public float timeBeforeFreezeMeteor;
    GameObject meteorFollowing;

    [Header("Stage 2")]
    public bool waitingForEgg;

    [Header("Stage 4")]
    public bool waitingForSeed;

    [Header("Stage 6")]
    public bool waitingForTree;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("PlayTutorial") == 1) { TutorialActive = false; return; }
        StartTutorialStage(0);
    }

    void Update()
    {
        if (!TutorialActive) { return; }
        UpdateTutorialStage();
    }

    public void StartTutorialStage(int stageInt)
    {
        tutorialStage = stageInt;
        switch (stageInt)
        {
            case 0:
                //Meteor spawn
                MeteorSpawner.instance.canSpawn = false;
                TutorialUIPanel.SetActive(true);
                InGameUIPanel.SetActive(false);
                Debug.Log("Start Tutorial Stage 1");
                MeteorSpawner.instance.SpawnMeteor(out meteorFollowing);
                TutorialText.text = "Hi I'm Gaia\nStart by tapping the meteor";
                break;

            case 1:
                Debug.Log("Start Tutorial Stage 2");
                TutorialText.text = "Tap the present to recieve some items";
                break;

            case 2:
                Debug.Log("Start Tutorial Stage 3");
                TutorialText.text = "Wait for the egg to hatch\nYou can tap it to speed it up";
                waitingForEgg = true;
                break;

            case 3:
                Debug.Log("Start Tutorial Stage 4");
                TutorialText.text = "Now tap me!";
                waitingForEgg = false;
                break;

            case 4:
                Debug.Log("Start Tutorial Stage 5");
                TutorialText.text = "You got a seed in your gift\nTap it now";
                waitingForSeed = true;

                break;

            case 5:
                Debug.Log("Start Tutorial Stage 6");
                TutorialText.text = "You can plant the seed by tapping one of the pots";
                Camera.main.gameObject.GetComponent<CameraControl>().currentCameraPosition = CameraControl.CameraPositions.IslandView;
                waitingForSeed = false;
                break;

            case 6:
                Debug.Log("Start Tutorial Stage 7");
                TutorialText.text = "Now wait for the tree to grow";
                waitingForTree = true;
                break;

            case 7:
                Debug.Log("Start Tutorial Stage 8");
                TutorialText.text = "You can collect fruit by tapping the tree";
                waitingForTree = false;
                break;

            case 8:
                Debug.Log("Start Tutorial Stage 9");
                TutorialText.text = "You finished the tutorial\nYou can carry on collecting creatures and planting trees now";
                MeteorSpawner.instance.canSpawn = true;
                break;

            default:

                TutorialUIPanel.SetActive(false);
                InGameUIPanel.SetActive(true);
                TutorialActive = false;
                PlayerPrefs.SetInt("PlayTutorial", 1);
                break;
        }

    }

    public void UpdateTutorialStage()
    {
        switch (tutorialStage)
        {
            case 0:
                //Meteor spawn
                Debug.Log("Tutorial Stage 1");
                if(timeBeforeFreezeMeteor <= 0)
                {
                    meteorFollowing.GetComponent<Meteor>().speed = Mathf.MoveTowards(meteorFollowing.GetComponent<Meteor>().speed, 0, Time.deltaTime * 1f);
                }
                else
                {
                    timeBeforeFreezeMeteor -= Time.deltaTime;
                }
                break;

            case 1:
                Debug.Log("Tutorial Stage 2");
                break;

            case 2:
                Debug.Log("Tutorial Stage 3");
                break;

            case 3:
                Debug.Log("Tutorial Stage 4");
                break;

            case 4:
                Debug.Log("Tutorial Stage 5");
                break;

            case 5:
                Debug.Log("Tutorial Stage 6");
                break;

            case 6:
                Debug.Log("Tutorial Stage 7");
                break;

            case 7:
                Debug.Log("Tutorial Stage 8");
                break;
        }
    }

    public void CurrentTutorialItemTapped(Vector2 touchPos)
    {
        RaycastHit2D hit;
        switch (tutorialStage)
        {
            case 0:
                //TapMeteor
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[tutorialStage]);
                if(hit != false)
                {
                    hit.transform.GetComponent<Meteor>().Tapped();
                    tutorialStage++;
                }
                break;

            case 1:
                //tap gift
                
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[tutorialStage]);
                if(hit != false)
                {
                    hit.transform.GetComponent<GiftScript>().Tapped();
                    tutorialStage++;
                }

                break;

            case 2:
                //tap egg
                
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[tutorialStage]);
                if(hit != false)
                {
                    hit.transform.GetComponent<EggScript>().SpeedUpHatch();
                }
                break;

            case 3:
                //tap giant
                
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[tutorialStage]);
                if(hit != false)
                {
                    hit.transform.gameObject.GetComponent<GiantScript>().GiantTapped();
                    tutorialStage++;
                }
                break;

            case 4:
                //tap seed
                
                break;

            case 5:
                //tap pot

                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[tutorialStage]);
                if (hit != false)
                {
                    GiantScript.Instance.GoToPot(hit.transform.gameObject);
                    tutorialStage++;
                }
                break;

            case 7:
                //tap tree
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[tutorialStage]);
                if (hit != false)
                {
                    hit.transform.gameObject.GetComponent<TreeScript>().Tapped();
                    tutorialStage++;
                }
                break;

            case 8:
                //do anything
                tutorialStage++;
                break;
        }

        StartTutorialStage(tutorialStage);
    }

    public void CurrentTutorialItemDragReleased(Vector2 touchPos)
    {
        switch (tutorialStage)
        {

            case 8:
                //do anything
                tutorialStage++;
                break;

        }
    }

    public void CurrentTutorialItemDrag(Vector2 touchPos)
    {
        switch (tutorialStage)
        {
            case 8:
                //do anything
                tutorialStage++;
                break;
        }
    }
}
