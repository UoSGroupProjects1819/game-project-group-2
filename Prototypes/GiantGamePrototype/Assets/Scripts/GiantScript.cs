using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour {

    public Transform seedHolder;
    public GameObject currentHolding;
    bool holding = false;

    public Animator anim;

    public GameObject holdingButton;

    Vector2 targetPoint;
    public float speed;

    Rigidbody2D RB;
    StatManager SM;
    InventoryScript IS;

    public GameObject HUD;

    private void Start()
    {
        targetPoint = this.transform.position;
        RB = this.GetComponent<Rigidbody2D>();
        SM = GameObject.FindGameObjectWithTag("StatManager").GetComponent<StatManager>();
        IS = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryScript>();
    }

    void LateUpdate ()
    {
        if (holding)
        {
            currentHolding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentHolding.transform.position = seedHolder.position;
            currentHolding.transform.eulerAngles = seedHolder.eulerAngles;

            holdingButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropHeldItem();
            }
        }
        else
        {
            holdingButton.SetActive(false);
        }

        Move();

        GetTouch();

        anim.SetFloat("Speed", this.GetComponent<Rigidbody2D>().velocity.magnitude);
    }

    void Move()
    {
        if (targetPoint != (Vector2)this.transform.position)
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
        }
    }

    public void GetTouch()
    {
        if (!HUD.activeSelf || SM.statsPanel.activeSelf) { return; }

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

        if (hit && hit.transform.tag == "TappableArea")
        {
            targetPoint = hit.point;
        }
        if (hit && hit.transform.tag == "Giant")
        {
            IS.inventoryPanel.SetActive(true);
            IS.HUDPanel.SetActive(false);
            IS.UpdateEggUI(IS.EggUI);
            IS.UpdateSeedUI(IS.SeedUI);
        }
    }

    public void SetCurrentHolding(GameObject newHolding)
    {
        Destroy(currentHolding);
        currentHolding = Instantiate(newHolding, seedHolder.position, seedHolder.rotation);
        holding = true;
    }

    public void DropHeldItem()
    {
        holding = false;
        currentHolding = null;
    }

}
