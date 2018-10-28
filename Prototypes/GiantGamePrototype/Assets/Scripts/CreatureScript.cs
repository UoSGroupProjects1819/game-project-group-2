using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureScript : MonoBehaviour {

    Rigidbody2D RB;
    InventoryScript IS;
    StatManager SM;

    Vector2 targetPoint;

    public float maxWaitTime;
    public float minWaitTime;
    float waitTime = 0;

    public float maxWalkDistance;

    public string preferedFruit;
    public string hatedFruit;

    public float speed;
    public float speedMultiplier = 1;

    public Transform starPanel;
    public GameObject star;

    public GameObject gift;

    public SpriteRenderer face;

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
        public Sprite faceSprite;
        public float speedMultiplier;
        public float amt;
        public bool canBreed;
    }

    public List<HappinessLevel> happinessLevels;
    public HappinessLevel currentHappinessLevel;

    [Space(10)]
    [Header("Stats")]
    int Stars = 1;
    public int Intelligence = 0;
    public int Agility = 0;
    public int Strength = 0;
    public int Style = 0;
    public int Stamina = 0;

    Image IntelligenceImage;
    Image AgilityImage;
    Image StrengthImage;
    Image StyleImage;
    Image StaminaImage;



    void Start ()
    {
        //SortHappniessLevels();
        IS = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryScript>();
        RB = this.GetComponent<Rigidbody2D>();
        SM = GameObject.FindGameObjectWithTag("StatManager").GetComponent<StatManager>();

        IntelligenceImage = SM.IntelligenceImage;
        AgilityImage = SM.AgilityImage;
        StrengthImage = SM.StrengthImage;
        StyleImage = SM.StyleImage;
        StaminaImage = SM.StaminaImage;
        starPanel = SM.StarPanel;

        Intelligence = GetRandomStat(IntelligenceImage);
        Agility = GetRandomStat(AgilityImage);
        Strength = GetRandomStat(StrengthImage);
        Style = GetRandomStat(StyleImage);
        Stamina = GetRandomStat(StaminaImage);

        for (int i = 0; i < Stars; i++)
        {
            Instantiate(star, starPanel);
        }

        targetPoint = new Vector2(Random.Range(this.transform.position.x - maxWalkDistance, this.transform.position.x + maxWalkDistance), this.transform.position.y);

    }

    void TestStatUpdate()
    {
        Intelligence = GetRandomStat(IntelligenceImage);
        Agility = GetRandomStat(AgilityImage);
        Strength = GetRandomStat(StrengthImage);
        Style = GetRandomStat(StyleImage);
        Stamina = GetRandomStat(StaminaImage);
    }

    void Update ()
    {
        ReduceHappiness();
        GetTouch();
        Movement();
        happinessText.rectTransform.localScale = new Vector3(this.transform.localScale.x, 1);
        if (Input.GetKeyDown(KeyCode.A)) { TestStatUpdate(); }
    }

    int GetRandomStat(Image imageToSet)
    {
        int maxChance = 0;
        foreach (var rank in SM.Ranks)
        {
            maxChance += rank.chance;
        }

        int randomInt = Random.Range(0, maxChance);
        int i = 0;

        int lowerRank = 0;
        StatManager.StatRank chosenRank = new StatManager.StatRank();
        foreach (var rank in SM.Ranks)
        {
            i += rank.chance;
            if(i > randomInt)
            {
                chosenRank = rank;
                break;
            }
            else
            {
                lowerRank = rank.amt;
            }
        }
        imageToSet.sprite = chosenRank.sprite;
        return Random.Range(lowerRank, chosenRank.amt);
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
                RB.velocity = new Vector2(speed * speedMultiplier, 0);
                transform.localScale = new Vector2(1, 1);
            }
            else
            if (targetPoint.x < this.transform.position.x)
            {
                RB.velocity = new Vector2(-speed * speedMultiplier, 0);
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

        SM.breedNote.SetActive(currentHappinessLevel.canBreed);
        face.sprite = currentHappinessLevel.faceSprite;
        speedMultiplier = currentHappinessLevel.speedMultiplier;
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
