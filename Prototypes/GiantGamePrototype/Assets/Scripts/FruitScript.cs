using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour {

    public bool readyToPick;
    public string fruitType;

    public string majorStatToIncrease;
    public string minorStatToIncrease;

    public GameObject targetCreature;
    public bool beingEaten;
    public float timeToEat;
    private float spriteChangeTimer;
    private int currentsprite;
    public Sprite[] eggSprites;

    private void Update()
    {
        if (beingEaten)
        {
            this.transform.eulerAngles = Vector3.zero;
            timeToEat -= Time.deltaTime;

            spriteChangeTimer -= Time.deltaTime;
            if (spriteChangeTimer <= 0 && currentsprite < eggSprites.Length)
            {
                spriteChangeTimer = timeToEat / eggSprites.Length + 1;
                this.GetComponentInChildren<SpriteRenderer>().sprite = eggSprites[currentsprite];
                currentsprite++;
            }

            if (timeToEat <= 0)
            {
                targetCreature.GetComponent<CreatureScript>().eating = false;
                Destroy(this.gameObject);
            }
        }
    }

    public void Dragging(Vector2 dragPos)
    {
        this.transform.position = new Vector3(dragPos.x, dragPos.y, 0);
        this.GetComponentInChildren<SpriteRenderer>().sortingOrder = targetCreature.GetComponentInChildren<SpriteRenderer>().sortingOrder + 2;
        this.GetComponentInChildren<SpriteRenderer>().sortingLayerID = targetCreature.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
    }

    public void ReleaseDrag(Vector3 dragPos, GameObject creatureToFeed)
    {
        if (creatureToFeed != null && creatureToFeed == targetCreature)
        {
            InventoryManager.Instance.RemoveFruit(fruitType, targetCreature.GetComponentInParent<IslandScript>().islandID);
            targetCreature.GetComponent<CreatureScript>().EatFruit(10, fruitType, majorStatToIncrease, minorStatToIncrease);
            targetCreature.GetComponent<CreatureScript>().eating = true;
            this.transform.parent = targetCreature.transform;
            this.transform.localPosition = new Vector2(0.05f, 0f);
            creatureToFeed.GetComponent<CreatureScript>().targetFruit = null;
            InventoryManager.Instance.UpdateFruitButtons(WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().islandID);
            beingEaten = true;
            Debug.Log("Fed creature");
            //this.transform.localPosition = Vector2.zero;
        }
        else
        {
            Debug.Log("Dropped fruit but no creature");
            InventoryManager.Instance.inventoryPanel.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
