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
            this.GetComponent<Image>().sprite = IS.FindEgg(IS.FindIngredient(craftingIngredient.ingredient.name).eggToMake).sprite;
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            this.GetComponent<Image>().sprite = IS.FindSeed(IS.FindIngredient(craftingIngredient.ingredient.name).seedToMake).sprite;
        }
    }

    void ResetCraftingTable()
    {
        craftingIngredient.ingredient = new IngredientInInventory();
        craftingIngredient.SetSprite();

        craftingObject.seed = new SeedInInventory();
        craftingObject.egg = new EggInInventory();
        craftingObject.thisObjectType = CraftingObject.ObjectType.egg;
        craftingObject.SetSprite();

        GetOutcome();
        this.GetComponent<Button>().interactable = false;
    }

    public void CompleteCraft()
    {
        if(craftingObject.thisObjectType == CraftingObject.ObjectType.egg)
        {
            IS.AddEgg(IS.FindIngredient(craftingIngredient.ingredient.name).eggToMake);
            IS.RemoveEgg(IS.inventory.eggs[0]);
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            IS.AddSeed(IS.FindIngredient(craftingIngredient.ingredient.name).seedToMake);
            IS.RemoveSeed(IS.inventory.seeds[0]);
        }

        IS.RemoveIngredient(craftingIngredient.ingredient);

        ResetCraftingTable();
    }
}
