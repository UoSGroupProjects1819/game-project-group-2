using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    IngredientInInventory thisIngredient;

    InventoryScript IS;

    public void SetUpButton(IngredientInInventory newIngredient)
    {
        IS = InventoryScript.Instance;
        Debug.Log(IS.name);
        thisIngredient = newIngredient;
        childImage.sprite = IS.FindIngredient(thisIngredient.name).sprite;
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
