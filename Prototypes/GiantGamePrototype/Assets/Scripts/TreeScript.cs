using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    public string treeType;

    public GameObject fruitToSpawn;
    public GameObject[] fruitSpawns;

    public float timeToGrow;

    float growTimer;

    public GameObject currentFruit;

    InventoryScript IS;

    public LayerMask thisLayer;

	// Use this for initialization
	void Start ()
    {
        IS = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryScript>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetTouch();
        GrowFruit();
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

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero,100, thisLayer);

        if (hit && hit.transform == this.transform)
        {
            if (currentFruit.GetComponent<FruitScript>().readyToDrop)
            {
                currentFruit.GetComponent<Rigidbody2D>().gravityScale = 1;
                currentFruit.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
    }

    void DropFruit()
    {
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 1;
        currentFruit.GetComponent<CircleCollider2D>().enabled = true;
    }

    void GrowFruit()
    {
        if(currentFruit == null)
        {
            currentFruit = Instantiate(fruitToSpawn, fruitSpawns[Random.Range(0, fruitSpawns.Length)].transform.position, Quaternion.identity, this.transform);
            growTimer = 0;
        }

        if (!currentFruit.GetComponent<FruitScript>().readyToDrop)
        {
            if(growTimer > timeToGrow)
            {
                currentFruit.GetComponent<FruitScript>().readyToDrop = true;
                currentFruit.transform.localScale = new Vector2(1, 1);
                return;
            }

            growTimer += Time.deltaTime;

            currentFruit.transform.localScale = new Vector2(1, 1) * ((1 / timeToGrow) * growTimer);

        }
    }
}
