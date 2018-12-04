using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOutcome : MonoBehaviour {

    /*public InventoryScript IM;

    public CraftingObject craftingObject;
    public CraftingIngredient craftingIngredient;

    public Sprite blankSprite;

    public void GetOutcome()
    {
        if (craftingObject.thisObjectType == CraftingObject.ObjectType.egg)
        {
            if (craftingIngredient.ingredient.name == "" || craftingObject.egg.name == "") { return; }
            this.GetComponent<Button>().interactable = true;
            this.GetComponent<Image>().sprite = IM.FindEgg(IM.FindIngredient(craftingIngredient.ingredient.name).eggToMake).sprite;
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            if (craftingIngredient.ingredient.name == "" || craftingObject.seed.name == "") { return; }
            this.GetComponent<Button>().interactable = true;
            this.GetComponent<Image>().sprite = IM.FindSeed(IM.FindIngredient(craftingIngredient.ingredient.name).seedToMake).sprite;
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
            IM.AddEgg(IM.FindIngredient(craftingIngredient.ingredient.name).eggToMake);
            IM.RemoveEgg(IM.inventory.eggs[0]);
        }

        if (craftingObject.thisObjectType == CraftingObject.ObjectType.seed)
        {
            IM.AddSeed(IM.FindIngredient(craftingIngredient.ingredient.name).seedToMake);
            IM.RemoveSeed(IM.inventory.seeds[0]);
        }

        IM.RemoveIngredient(craftingIngredient.ingredient);

        ResetCraftingTable();
    }
    */
}
