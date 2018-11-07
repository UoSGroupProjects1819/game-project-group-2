using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngredient : MonoBehaviour {


    public IngredientInInventory ingredient;


    public void SetSprite()
    {
        this.GetComponent<Image>().sprite = InventoryScript.Instance.FindIngredient(ingredient.name).sprite;
    }
}
