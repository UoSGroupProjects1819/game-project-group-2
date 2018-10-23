using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOutcome : MonoBehaviour {

    public InventoryScript IS;

    public CraftingObject craftingObject;
    public CraftingIngredient craftingIngredient;

    public void GetOutcome()
    {
        this.GetComponent<Button>().interactable = true;

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.egg)
        {
            this.GetComponent<Image>().sprite = craftingIngredient.ingredient.eggToMake.sprite;
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            this.GetComponent<Image>().sprite = craftingIngredient.ingredient.seedToMake.sprite;
        }
    }

    void ResetCraftingTable()
    {
        craftingIngredient.ingredient = new InventoryScript.Ingredient();
        craftingIngredient.SetSprite();

        craftingObject.seed = new InventoryScript.Seed();
        craftingObject.egg = new InventoryScript.Egg();
        craftingObject.thisObjectType = CraftingObject.ObjectType.egg;
        craftingObject.SetSprite();

        GetOutcome();
        this.GetComponent<Button>().interactable = false;
    }

    public void CompleteCraft()
    {
        if(craftingObject.thisObjectType == CraftingObject.ObjectType.egg)
        {
            IS.AddEgg(craftingIngredient.ingredient.eggToMake);
            IS.RemoveEgg(IS.eggsInInventory[0]);
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            IS.AddSeed(craftingIngredient.ingredient.seedToMake);
            IS.RemoveSeed(IS.seedsInInventory[0]);
        }

        IS.RemoveIngredient(craftingIngredient.ingredient);

        ResetCraftingTable();
    }
}
