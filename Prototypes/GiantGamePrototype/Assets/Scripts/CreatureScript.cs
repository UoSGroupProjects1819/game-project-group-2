using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CreatureScript : MonoBehaviour {

    #region Variables


    public string creatureName;
    public string type;

    Rigidbody2D RB;
    InventoryScript IS;
    StatManager SM;
    CreatureManager CM;

    Vector2 targetPoint;

    public float maxWaitTime;
    public float minWaitTime;
    float waitTime = 0;

    public float maxWalkDistance;

    public string preferedFruit;
    public string hatedFruit;

    public float speed;
    public float speedMultiplier = 1;
    public Vector2 movementBoundary;

    public GameObject targetFruit;

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
    public int Stars = 1;

    Image IntelligenceImage;
    Image AgilityImage;
    Image StrengthImage;
    Image StyleImage;
    Image StaminaImage;

    bool generateStats = true;

    #endregion

    void Start ()
    {
        //SortHappniessLevels();
        IS = InventoryScript.Instance;
        RB = this.GetComponent<Rigidbody2D>();
        SM = GameObject.FindGameObjectWithTag("StatManager").GetComponent<StatManager>();
        CM = CreatureManager.Instance;


        IntelligenceImage = SM.IntelligenceImage;
        AgilityImage = SM.AgilityImage;
        StrengthImage = SM.StrengthImage;
        StyleImage = SM.StyleImage;
        StaminaImage = SM.StaminaImage;
        starPanel = SM.StarPanel;
        happinessImage = SM.happinessImage;

        if (generateStats)
        { 
            CM.creaturesInWorld.Add(this.gameObject);
            Intelligence = GetRandomStat(IntelligenceImage);
            Agility = GetRandomStat(AgilityImage);
            Strength = GetRandomStat(StrengthImage);
            Style = GetRandomStat(StyleImage);
            Stamina = GetRandomStat(StaminaImage);
            CreatureManager.Instance.SaveCreatures();
        }

        SetHappinessLevel();
        UpdateSaturation();
        UpdateStatUI();

        for (int i = 0; i < Stars; i++)
        {
            Instantiate(star, starPanel);
        }


        targetPoint = new Vector2(Random.Range(this.transform.position.x - maxWalkDistance, this.transform.position.x + maxWalkDistance), this.transform.position.y);
    }

    public void SetUpCreature(string newName, float newHappiness, int newStars, SavedIntelligence intelligence, SavedAgility agility, SavedStrength strength, SavedStyle style, SavedStamina stamina)
    {
        generateStats = false;
        SM = GameObject.FindGameObjectWithTag("StatManager").GetComponent<StatManager>();

        name = newName;
        happiness = newHappiness;
        Stars = newStars;
        Intelligence.amt = intelligence.score;
        Intelligence.level = intelligence.level;
        Intelligence.rank = SM.FindRank(intelligence.rank);
        Intelligence.upgradePoints = intelligence.upgradePoints;

        Agility.amt = agility.score;
        Agility.level = agility.level;
        Agility.rank = SM.FindRank(agility.rank);
        Agility.upgradePoints = agility.upgradePoints;

        Strength.amt = strength.score;
        Strength.level = strength.level;
        Strength.rank = SM.FindRank(strength.rank);
        Strength.upgradePoints = strength.upgradePoints;

        Style.amt = style.score;
        Style.level = style.level;
        Style.rank = SM.FindRank(style.rank);
        Style.upgradePoints = style.upgradePoints;

        Stamina.amt = stamina.score;
        Stamina.level = stamina.level;
        Stamina.rank = SM.FindRank(stamina.rank);
        Stamina.upgradePoints = stamina.upgradePoints;
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
        Movement();

        if (SM.statsPanel.activeSelf && SM.targetCreature == this.gameObject)
        {
            Camera.main.gameObject.GetComponent<CameraControl>().DynamicOrtho = 0.25f;
            Camera.main.gameObject.GetComponent<CameraControl>().Dynamic = this.transform.position + new Vector3(0.2f, 0, -10);
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
        if(targetFruit != null)
        {
            targetPoint = targetFruit.transform.position;
            waitTime = 0;
        }

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        if ((Vector2)this.transform.position != targetPoint)
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

            if (distanceToTarget < 0.05f)
            {
                this.transform.position = new Vector2(targetPoint.x, this.transform.position.y);
                RB.velocity = Vector2.zero;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
                targetPoint = new Vector2(Random.Range(this.transform.position.x - maxWalkDistance, this.transform.position.x + maxWalkDistance), this.transform.position.y);
                if ((targetPoint.x - this.transform.parent.position.x) < movementBoundary.x || (targetPoint.x - this.transform.parent.position.x) > movementBoundary.y)
                {
                    Debug.Log("would have hit wall so redirected " + (targetPoint.x - this.transform.parent.position.x));
                    targetPoint = (Vector2)this.transform.position + ((Vector2)this.transform.position - targetPoint);
                }
            }
        }
    }

    public void EatFruit(float amt, string fruit, string majorStatToIncrease, string minorStatToIncrease)
    {
        if(fruit == hatedFruit) { return; }

        if (fruit == preferedFruit)
        {
            amt *= 2f;
        }

        happiness += amt;

        if(happiness > 100) { happiness = 100; }

        switch (majorStatToIncrease)
        {
            case "Intelligence":
                Intelligence.upgradePoints += 2;
                if(Intelligence.upgradePoints > 2)
                {
                    Intelligence.upgradePoints -= 3;
                    Intelligence.level++;
                }
                break;

            case "Agility":
                Agility.upgradePoints += 2;
                if (Agility.upgradePoints > 2)
                {
                    Agility.upgradePoints -= 3;
                    Agility.level++;
                }
                break;

            case "Strength":
                Strength.upgradePoints += 2;
                if (Strength.upgradePoints > 2)
                {
                    Strength.upgradePoints -= 3;
                    Strength.level++;
                }
                break;

            case "Style":
                Style.upgradePoints += 2;
                if (Style.upgradePoints > 2)
                {
                    Style.upgradePoints -= 3;
                    Style.level++;
                }
                break;

            case "Stamina":
                Stamina.upgradePoints += 2;
                if (Stamina.upgradePoints > 2)
                {
                    Stamina.upgradePoints = 0;
                    Stamina.level++;
                }
                break;

            default:
                break;
        }

        switch (minorStatToIncrease)
        {
            case "Intelligence":
                Intelligence.upgradePoints++;
                if (Intelligence.upgradePoints > 2)
                {
                    Intelligence.upgradePoints -= 3;
                    Intelligence.level++;
                }
                break;

            case "Agility":
                Agility.upgradePoints++;
                if (Agility.upgradePoints > 2)
                {
                    Agility.upgradePoints -= 3;
                    Agility.level++;
                }
                break;

            case "Strength":
                Strength.upgradePoints++;
                if (Strength.upgradePoints > 2)
                {
                    Strength.upgradePoints -= 3;
                    Strength.level++;
                }
                break;

            case "Style":
                Style.upgradePoints++;
                if (Style.upgradePoints > 2)
                {
                    Style.upgradePoints -= 3;
                    Style.level++;
                }
                break;

            case "Stamina":
                Stamina.upgradePoints++;
                if (Stamina.upgradePoints > 2)
                {
                    Stamina.upgradePoints -= 3;
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
        if(SM.targetCreature == null || SM.targetCreature != this.gameObject) { return; }

        SM.nameText.text = this.creatureName;

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

        foreach (Transform item in starPanel)
        {
            Destroy(item.gameObject);
        }

        for (i = 0; i < Stars; i++)
        {
            Instantiate(star, starPanel);
        }
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

    public void Touched()
    {
        if (IS.inventoryPanel.activeSelf) { return; }

        SM.statsPanel.SetActive(!SM.statsPanel.activeSelf);
        Camera.main.gameObject.GetComponent<CameraControl>().LockCamera = SM.statsPanel.activeSelf;
        SM.targetCreature = this.gameObject;
        UpdateStatUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "wall")
        {
            Debug.Log("Touched Wall");
            targetPoint = (Vector2)this.transform.position + ((Vector2)this.transform.position - targetPoint);
        }
    }
}
