using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOutcome : MonoBehaviour {

    public InventoryScript IS;

    public CraftingObject craftingObject;
    public CraftingIngredient craftingIngredient;

    public Sprite blankSprite;

    public void GetOutcome()
    {
        if (craftingObject.thisObjectType == CraftingObject.ObjectType.egg)
        {
            if (craftingIngredient.ingredient.name == "" || craftingObject.egg.name == "") { return; }
            this.GetComponent<Button>().interactable = true;
            this.GetComponent<Image>().sprite = IS.FindEgg(IS.FindIngredient(craftingIngredient.ingredient.name).eggToMake).sprite;
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            if (craftingIngredient.ingredient.name == "" || craftingObject.seed.name == "") { return; }
            this.GetComponent<Button>().interactable = true;
            this.GetComponent<Image>().sprite = IS.FindSeed(IS.FindIngredient(craftingIngredient.ingredient.name).seedToMake).sprite;
        }

        if (this.GetComponent<Image>().sprite == null)
        {
            this.GetComponent<Image>().sprite = blankSprite;
        }
    }

    void ResetCraftingTable()
    {
        craftingIngredient.ingredient = new IngredientInInventory() { name = "", amt = 0};
        craftingIngredient.SetSprite();

        craftingObject.seed = new SeedInInventory()
        {
            name = "",
            amt = 0
        };

        craftingObject.egg = new EggInInventory() {
            name = "",
            amt = 0
        };
        craftingObject.thisObjectType = CraftingObject.ObjectType.egg;
        craftingObject.SetSprite();

        //GetOutcome();
        this.GetComponent<Button>().interactable = false;
        this.GetComponent<Image>().sprite = blankSprite;
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
