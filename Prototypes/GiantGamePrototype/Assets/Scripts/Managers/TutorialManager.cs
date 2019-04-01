using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public bool TutorialActive = false;

    public int tutorialStage = 0;

    public LayerMask[] layersForStages;

    [Header ("Stage1")]
    public float timeBeforeFreezeMeteor;
    GameObject meteorFollowing;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
                Debug.Log("Start Tutorial Stage 1");
                MeteorSpawner.instance.SpawnMeteor(out meteorFollowing);
                MeteorSpawner.instance.canSpawn = false;
                break;

            case 1:
                Debug.Log("Start Tutorial Stage 2");
                break;

            case 2:
                Debug.Log("Start Tutorial Stage 3");
                break;

            default:
                TutorialActive = false;
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

            default:
                TutorialActive = false;
                break;
        }
    }

    public void CurrentTutorialItemTapped(Vector2 touchPos)
    {
        switch (tutorialStage)
        {
            case 0:
                //TapGiant
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero, 100, layersForStages[0]);
                if(hit != false)
                {
                    hit.transform.gameObject.GetComponent<GiantScript>().GiantTapped();
                    tutorialStage++;
                }
                break;

            case 2:
                //Tap Meteor

                tutorialStage++;
                break;

        }
    }

    public void CurrentTutorialItemDragReleased(Vector2 touchPos)
    {
        switch (tutorialStage)
        {

            case 1:
                //release seed on pot

                tutorialStage++;
                break;

        }
    }

    public void CurrentTutorialItemDrag(Vector2 touchPos)
    {
        switch (tutorialStage)
        {
            case 1:
                //drag seed to pot

                break;

            case 2:

                tutorialStage++;
                break;
        }
    }
}
