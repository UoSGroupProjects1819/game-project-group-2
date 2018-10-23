using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingObject : MonoBehaviour {

    public InventoryScript.Seed seed;
    public InventoryScript.Egg egg;

    public enum ObjectType
    {
        egg,
        seed
    }

    public ObjectType thisObjectType;

    public void SetSprite()
    {
        if (thisObjectType == ObjectType.egg)
        {
            this.GetComponent<Image>().sprite = egg.sprite;
        }
        else
        if (thisObjectType == ObjectType.seed)
        {
            this.GetComponent<Image>().sprite = seed.sprite;
        }

        
    }

}
