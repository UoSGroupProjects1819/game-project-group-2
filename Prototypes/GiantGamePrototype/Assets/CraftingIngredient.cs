using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngredient : MonoBehaviour {


    public InventoryScript.Ingredient ingredient;


    public void SetSprite()
    {
        this.GetComponent<Image>().sprite = ingredient.sprite;
    }
}
