﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour {

    #region variables
    public static GiantScript Instance;

    public Transform seedHolder;
    public GameObject currentHolding;
    bool holding = false;

    public Animator anim;

    public GameObject holdingButton;

    [HideInInspector]
    public Vector2 targetPoint;
    public float speed;


    public GameObject targetObject;

    Rigidbody2D RB;
    StatManager SM;
    InventoryManager IM;
    WorldManager WM;

    public GameObject HUD;

    public LayerMask GiantLayer;
    public LayerMask WorldLayer;

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        targetPoint = this.transform.position;
        RB = this.GetComponent<Rigidbody2D>();
        SM = GameObject.FindGameObjectWithTag("StatManager").GetComponent<StatManager>();
        IM = InventoryManager.Instance;
        WM = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
    }

    void LateUpdate()
    {
        if (holding)
        {
            currentHolding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentHolding.transform.position = seedHolder.position;
            currentHolding.transform.eulerAngles = seedHolder.eulerAngles;

            if (currentHolding.tag == "Egg")
            {
                holdingButton.SetActive(true);
            }

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

    public void GiantTapped()
    {
        if (IM.inventoryPanel.activeSelf)
        {
            IM.inventoryPanel.SetActive(false);
        }
        else
        {
            IM.inventoryPanel.SetActive(true);
            //IM.HUDPanel.SetActive(false);
            //IM.UpdateEggUI(IM.EggUI);
            IM.UpdateSeedButtons();
            targetPoint = this.transform.position;
        }


    }

    public void WorldTapped(Vector2 tapPos)
    {
        if (IM.inventoryPanel.activeSelf) { return; }

        targetPoint = tapPos;

    }

    public void SetCurrentHolding(GameObject newHolding)
    {
        Destroy(currentHolding);
        currentHolding = Instantiate(newHolding, seedHolder.position, seedHolder.rotation, WM.SelectedIsland.transform);

        holding = true;
    }

    public void SetCurrentHolding(GameObject newHolding, string type)
    {

        currentHolding = Instantiate(newHolding, seedHolder.position, seedHolder.rotation, WM.SelectedIsland.transform);

        if (currentHolding.tag == "Seed")
        {
            currentHolding.GetComponent<SeedScript>().seedType = type;
        }
        else
        if (currentHolding.tag == "Egg")
        {
            currentHolding.GetComponent<EggScript>().eggType = type;
        }

        holding = true;
    }

    public void DropHeldItem()
    {
        if (currentHolding == null) { return; }
        if (currentHolding.tag == "Egg")
        {
            IM.RemoveEgg(currentHolding.GetComponent<EggScript>().eggType);
            WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().currentCreaturePopulation++;
        }

        holding = false;
        currentHolding = null;
    }

    public void GoToPot(GameObject pot)
    {
        if (pot.GetComponent<PlantPot>().treeInPot != null || currentHolding == null) { return; }


        if (currentHolding.GetComponent<SeedScript>() == null) { return; }

        targetPoint = pot.transform.position;
        targetObject = pot;
    }

    public void PlaceSeedInPot(GameObject pot)
    {
        if (currentHolding == null) { return; }
        if (currentHolding.tag == "Seed")
        {
            IM.RemoveSeed(currentHolding.GetComponent<SeedScript>().seedType);
            WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().currentTreePopulation++;

            currentHolding.transform.parent = pot.transform;
            currentHolding.transform.localPosition = Vector3.zero;
            currentHolding.transform.eulerAngles = Vector3.zero;
            pot.GetComponent<PlantPot>().treeInPot = currentHolding;
            currentHolding.GetComponent<SeedScript>().readyToSpawn = true;
            currentHolding.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
            targetObject = null;

            holding = false;
            currentHolding = null;
        }
    }

    void CollectFruit(TreeScript tree)
    {
        for (int i = 0; i < tree.currentFruit.Count; i++)
        {
            IM.AddFruit(tree.currentFruit[i].GetComponent<FruitScript>().fruitType, WM.SelectedIsland.GetComponent<IslandScript>().islandID);
            Destroy(tree.currentFruit[i]);
            
        }

        tree.currentFruit.Clear();

        targetObject = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject == targetObject)
        {
            targetPoint = this.transform.position;
            if(collision.tag == "PlantPot")
            {
                PlaceSeedInPot(collision.gameObject);
            }

            if(collision.tag == "Tree")
            {
                CollectFruit(collision.gameObject.GetComponent<TreeScript>());
            }
        }
    }

}
