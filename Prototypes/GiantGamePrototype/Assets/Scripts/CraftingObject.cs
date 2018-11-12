using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingObject : MonoBehaviour {

    public SeedInInventory seed;
    public EggInInventory egg;

    public Sprite blankSprite;

    public enum ObjectType
    {
        egg,
        seed
    }

    public ObjectType thisObjectType;

    public void SetSprite()
    {

        if (egg.name == "" && seed.name == "")
        {
            this.GetComponent<Image>().sprite = blankSprite;
            return;
        }

        Debug.Log("Seed is " + seed.name);

        if (thisObjectType == ObjectType.egg)
        {
                this.GetComponent<Image>().sprite = InventoryScript.Instance.FindEgg(egg.name).sprite;
        }
        else
        if (thisObjectType == ObjectType.seed)
        {
                this.GetComponent<Image>().sprite = InventoryScript.Instance.FindSeed(seed.name).sprite;
        }
    }
}
