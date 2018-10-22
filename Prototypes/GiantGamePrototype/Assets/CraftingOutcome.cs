using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOutcome : MonoBehaviour {

    public CraftingObject craftingObject;
    public CraftingIngredient craftingIngedient;

    public void GetOutcome()
    {
        if (craftingObject.thisObjectType == CraftingObject.ObjectType.egg)
        {
            this.GetComponent<Image>().sprite = craftingIngedient.ingredient.eggToMake.sprite;
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            this.GetComponent<Image>().sprite = craftingIngedient.ingredient.seedToMake.sprite;
        }
    }
}
