using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToPoint : MonoBehaviour
{

    Vector2 targetPoint;
    public float speed;

    GameObject target;

    Rigidbody2D RB;

    // Use this for initialization
    void Start()
    {
        targetPoint = this.transform.position;
        RB = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targetPoint != (Vector2)this.transform.position && targetPoint != null)
        {

            if (targetPoint.x > this.transform.position.x && targetPoint.x - this.transform.position.x > 0.1f)
            {
                RB.velocity = new Vector2(speed, 0);
                transform.localScale = new Vector2(1, 1);
            }
            else
            if (targetPoint.x < this.transform.position.x && targetPoint.x - this.transform.position.x < -0.1f)
            {
                RB.velocity = new Vector2(-speed, 0);
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                RB.velocity = Vector2.zero;
            }

            float distanceToTarget = targetPoint.x - this.transform.position.x;
            if (distanceToTarget < 0)
            {
                distanceToTarget = -distanceToTarget;
            }

            if (distanceToTarget < 0.1f)
            {
                this.transform.position = new Vector2(targetPoint.x, this.transform.position.y);
                RB.velocity = Vector2.zero;
            }

            //Camera.main.GetComponent<CameraPositions>().Dynamic =   this.transform.position + new Vector3(0,1,-10);
            //Camera.main.GetComponent<CameraPositions>().DynamicOrtho = 3;
            //Camera.main.GetComponent<CameraPositions>().useDynamic = true;
            //Camera.main.GetComponent<CameraPositions>().MoveCamera();
        }
        else
        {
            //Camera.main.GetComponent<CameraPositions>().useDynamic = false;
        }

        MoveToPoint();
    }

    public void MoveToPoint()
    {
        Touch[] Touches = Input.touches;

        Vector2 touchPos = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchPos = Input.mousePosition;
        }
        else
        if (Touches.Length > 0)
        {
            touchPos = Touches[0].position;
        }
        else
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero);

        if (hit)
        {
            targetPoint = hit.point;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && collision.gameObject == target)
        {
            targetPoint = this.transform.position;
        }
    }
}