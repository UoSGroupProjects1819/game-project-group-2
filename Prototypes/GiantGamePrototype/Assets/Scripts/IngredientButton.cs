using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    InventoryScript.Ingredient thisIngredient;

    InventoryScript IS;

    public void SetUpButton(InventoryScript newIS, InventoryScript.Ingredient newIngredient)
    {
        IS = newIS;
        thisIngredient = newIngredient;
        childImage.sprite = thisIngredient.sprite;
        childAmtCounter.text = "x" + thisIngredient.amt;

        if(thisIngredient.amt <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void CraftingClick()
    {
        IS.SetCraftingIngredient(thisIngredient);
    }

}
