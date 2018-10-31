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

    Transform starPanel;
    public GameObject star;

    public SpriteRenderer face;

    [Space(10)]
    [Header("Happiness")]
    public float happiness = 50;
    public float happinessLossSpeed;
    Image happinessImage;

    [System.Serializable]
    public struct HappinessLevel
    {
        public string name;
        public Sprite sprite;
        public Sprite faceSprite;
        public float speedMultiplier;
        public int amt;
        public bool canBreed;
    }

    [System.Serializable]
    public struct Stat
    {
        public int amt;
        public int level;
        public StatManager.StatRank rank;
        public int upgradePoints;
    }

    public List<HappinessLevel> happinessLevels;
    HappinessLevel currentHappinessLevel;

    [Space(10)]
    [Header("Stats")]
    public Stat Intelligence;
    public Stat Agility;
    public Stat Strength;
    public Stat Style;
    public Stat Stamina;
    int Stars = 1;

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
        happinessImage = SM.happinessImage;

        Intelligence = GetRandomStat(IntelligenceImage);
        Agility = GetRandomStat(AgilityImage);
        Strength = GetRandomStat(StrengthImage);
        Style = GetRandomStat(StyleImage);
        Stamina = GetRandomStat(StaminaImage);
        UpdateStatUI();

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

        if (SM.statsPanel.activeSelf && SM.targetCreature == this.gameObject)
        {
            Camera.main.gameObject.GetComponent<CameraPositions>().DynamicOrtho = 0.25f;
            Camera.main.gameObject.GetComponent<CameraPositions>().Dynamic = this.transform.position + new Vector3(0.2f, 0, -10);
        }

        if (Input.GetKeyDown(KeyCode.A)) { TestStatUpdate(); }
    }

    Stat GetRandomStat(Image imageToSet)
    {
        Stat newStat = new Stat();
        int maxChance = 0;
        foreach (var rank in SM.Ranks)
        {
            maxChance += rank.chance;
        }

        int randomInt = Random.Range(0, maxChance);
        int i = 0;

        StatManager.StatRank chosenRank = new StatManager.StatRank();
        foreach (var rank in SM.Ranks)
        {
            i += rank.chance;
            if(i > randomInt)
            {
                chosenRank = rank;
                break;
            }
        }
        imageToSet.sprite = chosenRank.sprite;

        newStat.amt = chosenRank.amt;
        newStat.level = 1;
        newStat.rank = chosenRank;
        newStat.upgradePoints = 0;

        return newStat;
    }

    int UpgradeStat()
    {



        return 0;
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

    public void IncreaseHappiness(float amt, string fruit, string statToIncrease)
    {
        if(fruit == hatedFruit) { return; }

        if (fruit == preferedFruit)
        {
            amt *= 2f;
        }

        happiness += amt;

        if(happiness > 100) { happiness = 100; }

        switch (statToIncrease)
        {
            case "Intelligence":
                Intelligence.upgradePoints++;
                if(Intelligence.upgradePoints > 2)
                {
                    Intelligence.upgradePoints = 0;
                    Intelligence.level++;
                }
                break;

            case "Agility":
                Agility.upgradePoints++;
                if (Agility.upgradePoints > 2)
                {
                    Agility.upgradePoints = 0;
                    Agility.level++;
                }
                break;

            case "Strength":
                Strength.upgradePoints++;
                if (Strength.upgradePoints > 2)
                {
                    Strength.upgradePoints = 0;
                    Strength.level++;
                }
                break;

            case "Style":
                Style.upgradePoints++;
                if (Style.upgradePoints > 2)
                {
                    Style.upgradePoints = 0;
                    Style.level++;
                }
                break;

            case "Stamina":
                Stamina.upgradePoints++;
                if (Stamina.upgradePoints > 2)
                {
                    Stamina.upgradePoints = 0;
                    Stamina.level++;
                }
                break;

            default:
                break;
        }

        UpdateStatUI();
        SetHappinessLevel();
        UpdateSaturation();
    }

    private void UpdateStatUI()
    {
        int i = 1;
        foreach (Transform item in SM.intelligenceUpgradeMeter)
        {
            if (Intelligence.upgradePoints >= i)
            {
                item.GetComponent<Image>().color = Color.red;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
        SM.intelligenceLevel.text = "Lvl " + Intelligence.level;

        i = 1;
        foreach (Transform item in SM.agilityUpgradeMeter)
        {
            if (Agility.upgradePoints >= i)
            {
                item.GetComponent<Image>().color = Color.red;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
        SM.agilityLevel.text = "Lvl " + Agility.level;

        i = 1;
        foreach (Transform item in SM.strengthUpgradeMeter)
        {
            if (Strength.upgradePoints >= i)
            {
                item.GetComponent<Image>().color = Color.red;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
        SM.strengthLevel.text = "Lvl " + Strength.level;

        i = 1;
        foreach (Transform item in SM.styleUpgradeMeter)
        {
            if (Style.upgradePoints >= i)
            {
                item.GetComponent<Image>().color = Color.red;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
        SM.styleLevel.text = "Lvl " + Style.level;

        i = 1;
        foreach (Transform item in SM.staminaUpgradeMeter)
        {
            if (Stamina.upgradePoints >= i)
            {
                item.GetComponent<Image>().color = Color.red;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
        SM.staminaLevel.text = "Lvl " + Stamina.level;

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

        happinessImage.sprite = currentHappinessLevel.sprite;
        SM.breedNote.SetActive(currentHappinessLevel.canBreed);
        face.sprite = currentHappinessLevel.faceSprite;
        speedMultiplier = currentHappinessLevel.speedMultiplier;
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

        if (hit && hit.transform == this.transform)
        {
            SM.statsPanel.SetActive(!SM.statsPanel.activeSelf);
            UpdateStatUI();
            Camera.main.gameObject.GetComponent<CameraPositions>().useDynamic = SM.statsPanel.activeSelf;
            SM.targetCreature = this.gameObject;
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
