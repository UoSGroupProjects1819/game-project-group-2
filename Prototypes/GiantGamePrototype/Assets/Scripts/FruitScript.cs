using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour {

    public bool readyToDrop;
    public string fruitType;

    public string majorStatToIncrease;
    public string minorStatToIncrease;

    public GameObject targetCreature;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Creature" && collision.gameObject.GetComponent<CreatureScript>().targetFruit == this.gameObject)
        {
            collision.gameObject.GetComponent<CreatureScript>().EatFruit(10, thisFruit.name, majorStatToIncrease, minorStatToIncrease);
            collision.gameObject.GetComponent<CreatureScript>().targetFruit = null;
            Destroy(this.gameObject);
        }
    }*/

    public void Dragging(Vector2 dragPos)
    {
        this.transform.position = new Vector3(dragPos.x, dragPos.y, 0);
        this.GetComponentInChildren<SpriteRenderer>().sortingOrder = targetCreature.GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;
        this.GetComponentInChildren<SpriteRenderer>().sortingLayerID = targetCreature.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
    }

    public void ReleaseDrag(Vector3 dragPos, GameObject creatureToFeed)
    {
        if (creatureToFeed != null && creatureToFeed == targetCreature)
        {
            InventoryScript.Instance.RemoveFruit(fruitType, targetCreature.GetComponentInParent<IslandScript>().islandID);
            targetCreature.GetComponent<CreatureScript>().EatFruit(10, fruitType, majorStatToIncrease, minorStatToIncrease);
            this.transform.parent = targetCreature.transform;
            InventoryScript.Instance.UpdateFruitButtons(WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>().islandID);
            Debug.Log("Fed creature");
            //this.transform.localPosition = Vector2.zero;
        }
        else
        {
            Debug.Log("Dropped fruit but no creature");
            InventoryScript.Instance.inventoryPanel.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
