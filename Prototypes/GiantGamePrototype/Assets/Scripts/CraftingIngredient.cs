using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngredient : MonoBehaviour {


    public IngredientInInventory ingredient;
    public Sprite blankSprite;

    public void SetSprite()
    {
        if (ingredient.name != null && ingredient.name != "")
        {
            this.GetComponent<Image>().sprite = InventoryScript.Instance.FindIngredient(ingredient.name).sprite;
        }
        else
        {
            this.GetComponent<Image>().sprite = blankSprite;
        }
    }
}
