using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public bool TutorialActive = true;

    public int tutorialStage = 0;

    public LayerMask[] layersForStages;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!TutorialActive) { return; }
        switch (tutorialStage)
        {
            case 0:
                //wait for giant tap
                Debug.Log("Tutorial Stage 1");
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
