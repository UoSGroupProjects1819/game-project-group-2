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

    public GameObject HUD;

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

    public void UpdateCraftingObjects(GameObject buttonPanel)
    {
        GameObject[] buttons = new GameObject[2];
        int i = 0;
        foreach (Transform item in buttonPanel.transform)
        {
            buttons[i] = item.gameObject;
            i++;
        }

        Egg egg = eggsInInventory[0];
        buttons[0].GetComponent<InventoryEggButton>().SetUpButton(this, egg);

        Seed seed = seedsInInventory[0];
        buttons[1].GetComponent<InventorySeedButton>().SetUpButton(this, seed);

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

    public void UpdateIngredient(Ingredient ingredient)
    {
        for (int i = 0; i < IngredientsInInventory.Count; i++)
        {
            Ingredient ingredientToEdit = new Ingredient();
            if (IngredientsInInventory[i].name == ingredient.name)
            {
                ingredientToEdit = IngredientsInInventory[i];

                ingredientToEdit.amt -= 1;
                IngredientsInInventory.RemoveAt(i);
                IngredientsInInventory.Insert(i, ingredientToEdit);
                break;
            }
        }
    }

    public void AddSeed(Seed seed)
    {
        bool newSeed = true;
        for (int i = 0; i < seedsInInventory.Count; i++)
        {
            if (seedsInInventory[i].name == seed.name)
            {
                newSeed = false;
                Seed seedToEdit = seedsInInventory[i];
                seedToEdit.amt += 1;
                seedsInInventory[i] = seedToEdit;
                Debug.Log("Added");
                break;
            }
        }

        if (newSeed)
        {
            seedsInInventory.Add(seed);
        }
    }

    public void AddEgg(Egg egg)
    {
        bool newEgg = true;
        for (int i = 0; i < eggsInInventory.Count; i++)
        {
            if (eggsInInventory[i].name == egg.name)
            {
                newEgg = false;
                Egg eggToEdit = eggsInInventory[i];
                eggToEdit.amt += 1;
                eggsInInventory[i] = eggToEdit;  
                Debug.Log("Added");
                break;
            }
        }

        if (newEgg)
        {
            eggsInInventory.Add(egg);
        }
    }

    public void RemoveSeed(Seed seed)
    {
        for (int i = 0; i < seedsInInventory.Count; i++)
        {
            Seed seedToEdit = new Seed();
            if (seedsInInventory[i].name == seed.name)
            {
                seedToEdit = seedsInInventory[i];

                seedToEdit.amt -= 1;
                seedsInInventory.RemoveAt(i);
                seedsInInventory.Insert(i, seedToEdit);
                break;
            }
        }
    }

    public void RemoveEgg(Egg egg)
    {
        for (int i = 0; i < eggsInInventory.Count; i++)
        {
            Egg eggToEdit = new Egg();
            if (eggsInInventory[i].name == egg.name)
            {
                eggToEdit = eggsInInventory[i];

                eggToEdit.amt -= 1;
                eggsInInventory.RemoveAt(i);
                eggsInInventory.Insert(i, eggToEdit);
                break;
            }
        }
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        for (int i = 0; i < IngredientsInInventory.Count; i++)
        {
            Ingredient ingredientToEdit = new Ingredient();
            if (IngredientsInInventory[i].name == ingredient.name)
            {
                Debug.Log("done ingredient");
                ingredientToEdit = IngredientsInInventory[i];

                ingredientToEdit.amt -= 1;
                IngredientsInInventory.RemoveAt(i);
                IngredientsInInventory.Insert(i, ingredientToEdit);
                break;
            }
        }
    }

    public void SetCraftingObject(Egg newEgg)
    {
        craftingObject.egg = newEgg;
        craftingObject.thisObjectType = CraftingObject.ObjectType.egg;
        craftingObject.SetSprite();
        craftingOutcome.GetOutcome();
    }

    public void SetCraftingObject(Seed newSeed)
    {
        craftingObject.seed = newSeed;
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
