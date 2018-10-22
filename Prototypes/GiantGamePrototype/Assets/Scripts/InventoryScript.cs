using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour {

    [System.Serializable]
    public struct Egg
    {
        public string name;
        public Sprite sprite;
        public int amt;
        public GameObject objectToSpawn;
    }

    [System.Serializable]
    public struct Seed
    {
        public string name;
        public Sprite sprite;
        public int amt;
        public GameObject objectToSpawn;
    }

    [System.Serializable]
    public struct Ingredient
    {
        public string name;
        public Sprite sprite;
        public int amt;
        public Egg eggToMake;
        public Seed seedToMake;
    }

    [SerializeField]
    public List<Egg> eggsInInventory;

    [Space(10)]

    [SerializeField]
    public List<Seed> seedsInInventory;

    [Space(10)]

    [SerializeField]
    public List<Ingredient> IngredientsInInventory;

    [Space(20)]

    public GameObject inventroryEggButton;
    public GameObject inventrorySeedButton;
    public GameObject ingredientButton;

    public void UpdateEggUI(GameObject eggButtonPanel)
    {
        foreach (Transform item in eggButtonPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (Egg egg in eggsInInventory)
        {
            GameObject newButton = Instantiate(inventroryEggButton, eggButtonPanel.transform);
            newButton.GetComponent<InventoryEggButton>().SetUpButton(this, egg);
        }
    }

    public void UpdateSeedUI(GameObject seedButtonPanel)
    {
        foreach (Transform item in seedButtonPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (Seed seed in seedsInInventory)
        {
            GameObject newButton = Instantiate(inventrorySeedButton, seedButtonPanel.transform);
            newButton.GetComponent<InventorySeedButton>().SetUpButton(this, seed);
        }
    }

    public void UpdateIngredientsUI(GameObject ingredientButtonPanel)
    {
        foreach (Transform item in ingredientButtonPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (Ingredient ingredient in IngredientsInInventory)
        {
            GameObject newButton = Instantiate(ingredientButton, ingredientButtonPanel.transform);
            newButton.GetComponent<IngredientButton>().SetUpButton(this, ingredient);
        }
    }

    public CraftingObject craftingObject;
    public CraftingIngredient craftingIngredient;
    public CraftingOutcome craftingOutcome;

    public void SetCraftingObject(Egg newEgg)
    {
        craftingObject.thisEgg = newEgg;
        craftingObject.thisObjectType = CraftingObject.ObjectType.egg;
        craftingObject.SetSprite();
        craftingOutcome.GetOutcome();
    }

    public void SetCraftingObject(Seed newSeed)
    {
        craftingObject.thisSeed = newSeed;
        craftingObject.thisObjectType = CraftingObject.ObjectType.seed;
        craftingObject.SetSprite();
        craftingOutcome.GetOutcome();
    }

    public void SetCraftingIngredient(Ingredient newIngredient)
    {
        craftingIngredient.ingredient = newIngredient;
        craftingIngredient.SetSprite();
        craftingOutcome.GetOutcome();
    }

}
