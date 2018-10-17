using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureScript : MonoBehaviour {

    Vector2 targetPoint;

    public float maxWaitTime;
    public float minWaitTime;
    float waitTime = 0;

    public float maxWalkDistance;

    public string preferedTree;

    public float speed;

    public GameObject gift;

    Rigidbody2D RB;

    bool giftGiven = false;

    public float happiness = 0;

    // Use this for initialization
    void Start ()
    {
        RB = this.GetComponent<Rigidbody2D>();
        targetPoint = new Vector2(Random.Range(this.transform.position.x - maxWalkDistance, this.transform.position.x + maxWalkDistance), this.transform.position.y);
    }

    // Update is called once per frame
    void Update ()
    {

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        //Debug.Log(this.transform.name + " is getting here");

        if (targetPoint != (Vector2)this.transform.position && targetPoint != null)
        {
            //Debug.Log(this.transform.name + " is moving to " + targetPoint);
            if (targetPoint.x > this.transform.position.x)
            {
                RB.velocity = new Vector2(speed, 0);
                transform.localScale = new Vector2(1, 1);
            }
            else
            if (targetPoint.x < this.transform.position.x)
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

            if (distanceToTarget < 0.01f)
            {
                this.transform.position = new Vector2(targetPoint.x, this.transform.position.y);
                RB.velocity = Vector2.zero;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
                targetPoint = new Vector2(Random.Range(this.transform.position.x - maxWalkDistance, this.transform.position.x + maxWalkDistance), this.transform.position.y);
            }
        }
    }

    internal void UpdateHappniess(List<GameObject> plantsOnIsland)
    {
        happiness = 0;
        foreach (var plant in plantsOnIsland)
        {
            if (plant.GetComponent<TreeScript>().treeType == preferedTree)
            {
                happiness += 10;
            }
        }

        if (!giftGiven && happiness >= 10)
        {
            GameObject newGift = Instantiate(gift, this.transform.position, Quaternion.identity);
            giftGiven = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "wall")
        {
            targetPoint = this.transform.position;
        }
    }
}
