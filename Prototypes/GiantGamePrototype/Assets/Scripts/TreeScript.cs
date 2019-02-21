using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    public string treeType;

    bool canSpawnFruit;
    public float spawnDelay;
    float spriteChangeTimer = 0;
    int currentsprite = 0;
    public Sprite[] treeSprites;

    public GameObject fruitToSpawn;
    public GameObject[] fruitSpawns;

    public float timeToGrow;

    public float growTimer;

    public List<GameObject> currentFruit = new List<GameObject>();

    InventoryManager IM;

    public LayerMask thisLayer;

    public bool loadedFromSave = false;

	// Use this for initialization
	void Start ()
    {
        IM = InventoryManager.Instance;

        if (!loadedFromSave)
        {
            TreeManager.Instance.TreesInWorld.Add(this.gameObject);
            this.GetComponent<Animator>().SetTrigger("Grow");
            TreeManager.Instance.SaveTrees();
        }
        else
        {
            canSpawnFruit = true;
            spawnDelay = 0;
        }

        if(spawnDelay <= 0)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = treeSprites[treeSprites.Length-1];
            canSpawnFruit = true;
        }
    }
	
	// Update IM called once per frame
	void Update ()
    {
        StartTreeLife();
        GrowFruit();
    }

    void StartTreeLife()
    {
        if (!canSpawnFruit)
        {
            spawnDelay -= Time.deltaTime;

            spriteChangeTimer -= Time.deltaTime;
            if (spriteChangeTimer <= 0 && currentsprite < treeSprites.Length)
            {
                spriteChangeTimer = spawnDelay / treeSprites.Length + 1;
                this.GetComponentInChildren<SpriteRenderer>().sprite = treeSprites[currentsprite];
                currentsprite++;
            }

            if (spawnDelay <= 0)
            {
                canSpawnFruit = true;
            }
        }
    }

    public void Touched()
    {
        if (IM.inventoryPanel.activeSelf) { return; }

        foreach (var fruit in currentFruit)
        {
            if (fruit.GetComponent<FruitScript>().readyToPick)
            {
                GiantScript.Instance.targetPoint = this.transform.position;
                GiantScript.Instance.targetObject = this.gameObject;
            }
        }
    }

    void GrowFruit()
    {
        if (!canSpawnFruit) { return; }

        Debug.Log(currentFruit);

        if (currentFruit.Count == 0)
        {
            for (int i = 0; i < this.GetComponentInParent<PlantPot>().potLevel + 1; i++)
            {
                currentFruit.Add(Instantiate(fruitToSpawn, fruitSpawns[i].transform.position, Quaternion.identity, this.transform));
                growTimer = 0;
            }
        }

        if (!currentFruit[0].GetComponent<FruitScript>().readyToPick)
        {
            if (growTimer > timeToGrow)
            {
                foreach (var fruit in currentFruit)
                {
                    fruit.GetComponent<FruitScript>().readyToPick = true;
                    fruit.transform.localScale = new Vector2(1, 1);
                }
                return;
            }

            growTimer += Time.deltaTime;

            foreach (var fruit in currentFruit)
            {
                fruit.transform.localScale = new Vector2(1, 1) * ((1 / timeToGrow) * growTimer);
            }
        }
    }

    /* Old Code
    void DropFruit()
    {
        GameObject closestCreature = null;
        float closestCreatureDistance = 0;
        foreach (Transform item in this.transform.parent.parent)
        {
            if (item.tag == "Creature")
            {
                if (item.GetComponent<CreatureScript>().targetFruit == null)
                {
                    
                    if (closestCreature == null || Vector2.Distance(this.transform.position, item.position) < closestCreatureDistance)
                    {
                        closestCreature = item.gameObject;
                        closestCreatureDistance = Vector2.Distance(this.transform.position, item.position);
                    }
                }
            }
        }

        if(closestCreature != null)
        {
            closestCreature.GetComponent<CreatureScript>().targetFruit = currentFruit;
            currentFruit.GetComponent<Rigidbody2D>().gravityScale = 1;
            currentFruit.GetComponent<CircleCollider2D>().enabled = true;
        }
    }*/


}
