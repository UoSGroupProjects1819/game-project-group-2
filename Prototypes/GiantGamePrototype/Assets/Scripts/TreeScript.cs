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

    public GameObject currentFruit;

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
        
        if (currentFruit.GetComponent<FruitScript>().readyToPick)
        {
            GiantScript.Instance.targetPoint = this.transform.position;
            GiantScript.Instance.targetObject = this.gameObject;
        }
    }

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
    }

    void GrowFruit()
    {
        if (!canSpawnFruit) { return; }

        if(currentFruit == null)
        {
            currentFruit = Instantiate(fruitToSpawn, fruitSpawns[Random.Range(0, fruitSpawns.Length)].transform.position, Quaternion.identity, this.transform);
            growTimer = 0;
        }

        if (!currentFruit.GetComponent<FruitScript>().readyToPick)
        {
            if(growTimer > timeToGrow)
            {
                currentFruit.GetComponent<FruitScript>().readyToPick = true;
                currentFruit.transform.localScale = new Vector2(1, 1);
                return;
            }

            growTimer += Time.deltaTime;

            currentFruit.transform.localScale = new Vector2(1, 1) * ((1 / timeToGrow) * growTimer);

        }
    }
}
