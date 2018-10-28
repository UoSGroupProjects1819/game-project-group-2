using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureScript : MonoBehaviour {

    Vector2 targetPoint;

    public float maxWaitTime;
    public float minWaitTime;
    float waitTime = 0;

    public float maxWalkDistance;

    public string preferedFruit;
    public string hatedFruit;

    public float speed;

    public GameObject gift;

    Rigidbody2D RB;

    [Space(10)]
    [Header("Happiness")]
    public float happiness = 50;
    public Text happinessText;
    public float happinessLossSpeed;

    [System.Serializable]
    public struct HappinessLevel
    {
        public string name;
        public Sprite sprite;
        public float amt;
    }

    public List<HappinessLevel> happinessLevels;
    public HappinessLevel currentHappinessLevel;

    [Space(10)]
    [Header("Stats")]
    public int Intelligence = 0;
    public int Agility = 0;
    public int Strength = 0;
    public int Style = 0;
    public int Stamina = 0;

    [Space (10)]

    public Text IntelligenceText;
    public Text AgilityText;
    public Text StrengthText;
    public Text StyleText;
    public Text StaminaText;

    [Space(10)]

    public Image IntelligenceImage;
    public Image AgilityImage;
    public Image StrengthImage;
    public Image StyleImage;
    public Image StaminaImage;


    InventoryScript IS;

    void Start ()
    {
        //SortHappniessLevels();
        IS = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryScript>();
        RB = this.GetComponent<Rigidbody2D>();
        targetPoint = new Vector2(Random.Range(this.transform.position.x - maxWalkDistance, this.transform.position.x + maxWalkDistance), this.transform.position.y);
    }

    void Update ()
    {
        ReduceHappiness();
        GetTouch();
        Movement();
        happinessText.rectTransform.localScale = new Vector3(this.transform.localScale.x, 1);
    }

    void UpdateSaturation()
    {
        this.GetComponentInChildren<SpriteRenderer>().material.SetFloat("_SaturationAmt", happiness / 100);
    }

    void ReduceHappiness()
    {
        if(happiness <= 0) { return; } 
        happiness -= Time.deltaTime * (happinessLossSpeed / 10);
        UpdateSaturation();
        SetHappinessLevel();
    }

    void Movement()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        if (targetPoint != (Vector2)this.transform.position)
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

    public void IncreaseHappiness(float amt, string fruit)
    {
        if(fruit == hatedFruit) { return; }

        if (fruit == preferedFruit)
        {
            amt *= 2f;
        }

        happiness += amt;

        if(happiness > 100) { happiness = 100; }

        SetHappinessLevel();
        UpdateSaturation();
    }

    void SetHappinessLevel()
    {
        HappinessLevel highestLevel = happinessLevels[0];
        foreach (var level in happinessLevels)
        {
            if(happiness > level.amt && highestLevel.amt < level.amt)
            {
                highestLevel = level;
            }
        }
        currentHappinessLevel = highestLevel;

        happinessText.text = currentHappinessLevel.name;
    }

    public void GetTouch()
    {
        if (IS.inventoryPanel.activeSelf) { return; }

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

        if (hit && hit.transform.tag == "Creature")
        {
            IS.fruitPanel.SetActive(true);
            IS.HUDPanel.SetActive(false);
            IS.UpdateFruitUI(IS.FruitUI, this);
        }
    }

    /*void SortHappniessLevels()
    {
        List<HappinessLevel> unsortedLevels = happinessLevels;
        List<HappinessLevel> sortedLevels = new List<HappinessLevel>();
        HappinessLevel lowest = new HappinessLevel();
        for (int i = 0; i < happinessLevels.Count; i++)
        {
            foreach (var item in unsortedLevels)
            {
                    Debug.Log(lowest.name);
                if (lowest.name == null || lowest.amt > item.amt)
                {
                    lowest = item;
                }
            }
            sortedLevels.Add(lowest);
            unsortedLevels.Remove(lowest);
        }

        happinessLevels = sortedLevels;
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "wall")
        {
            targetPoint = this.transform.position;
        }
    }
}
