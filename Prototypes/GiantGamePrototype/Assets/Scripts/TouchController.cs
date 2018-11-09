using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    public static TouchController Instance;

    public LayerMask layersToTouch;

    bool canTap = true;

    public int touchesLastFrame = 0;

    public Vector2 lastSingleTouchPoint;
    public Vector2 lastDoubleTouchPoint;
    public float lastPinchDistance;

    Vector2 startTouchPos = new Vector2(0, 0);

    private void Awake()
    {
        Instance = this;
    }

    void Update ()
    {
        GetTouch();
	}

    void GetTouch()
    { 
        if (Input.touchCount == 0)
        {
            canTap = true;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SingleTap(Input.mousePosition);
            }
        }

        Touch[] Touches = Input.touches;
        
        if (Touches.Length == 1)
        {
            //Debug.Log(Touches[0].phase.ToString());

            if (canTap == true)
            {
                if (Touches[0].phase == TouchPhase.Began)
                {
                    startTouchPos = Touches[0].position;
                }
                else if(Touches[0].phase == TouchPhase.Moved)
                {
                    //Debug.Log("Start touch Pos: " + startTouchPos + "New Touch Pos:" + Touches[0].position + Vector2.Distance(Touches[0].position, startTouchPos));
                    if (Vector2.Distance(startTouchPos, Touches[0].position) > (Screen.height / 50))
                    {
                        canTap = false;
                    }
                }
                else if(Touches[0].phase == TouchPhase.Ended)
                {
                    SingleTap(Touches[0].position);
                }
            }
            else
            {
                if (Touches[0].phase == TouchPhase.Moved)
                {
                    TouchDrag(Touches[0].position);
                }
            }

            lastSingleTouchPoint = Touches[0].position;
        }
        else if(Touches.Length == 2)
        {
            canTap = false;
            Vector2 touch0, touch1;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
            Vector2 newDoupleTouchPoint = new Vector2(touch0.x + touch1.x / 2, touch0.y + touch1.y / 2);
            Vector2 doubleTouchPosDifference = lastDoubleTouchPoint - newDoupleTouchPoint;
            float newPinchDistance = Vector2.Distance(touch0, touch1);
            float pinchDifference = (lastPinchDistance - newPinchDistance) ;

            if (pinchDifference != 0)
            {
                Pinch(doubleTouchPosDifference, pinchDifference);
            }

            lastPinchDistance = newPinchDistance;
            lastDoubleTouchPoint = newDoupleTouchPoint;
        }
        else
        {
            return;
        }

        touchesLastFrame = Input.touchCount;
    }

    void Pinch(Vector2 dragAmt, float pinchAmt)
    {
        Debug.Log("Pinching");

        Camera.main.GetComponent<CameraControl>().ScreenPinch(dragAmt, pinchAmt);
    }

    void TouchDrag(Vector2 touchPos)
    {
        Camera.main.GetComponent<CameraControl>().ScreenDrag(touchPos);
    }

    void SingleTap(Vector2 touchPos)
    {
        Debug.Log("Single Tap");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero);

        if(hit == false) { return; }

        if (hit.transform.tag == "Giant")
        {
            hit.transform.gameObject.GetComponent<GiantScript>().GiantTapped();
        }
        else
        if (hit.transform.tag == "Creature")
        {
            hit.transform.GetComponent<CreatureScript>().Touched();
        }
        else
        if (hit.transform.tag == "Tree")
        {
            hit.transform.gameObject.GetComponent<TreeScript>().Touched();
        }
        else
        if (hit.transform.tag == "TappableArea")
        {
            GiantScript.Instance.WorldTapped(hit.point);
        }
        else
        if (hit.transform.tag == "Meteor")
        {
            hit.transform.GetComponent<Meteor>().Tapped();
        }
        else
        if (hit.transform.tag == "Gift")
        {
            hit.transform.GetComponent<GiftScript>().Touched();
        }
        else
        if (hit.transform.tag == "PlantPot")
        {
            GiantScript.Instance.GoToPot(hit.transform.gameObject);
        }
        else
        {

        }
    }
}
