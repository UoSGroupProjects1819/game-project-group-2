using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#region InventoryClasses

[System.Serializable]
public class Egg
{
    public string name;
    public Sprite sprite;
    public GameObject objectToSpawn;
}

[System.Serializable]
public class Seed
{
    public string name;
    public Sprite sprite;
    public GameObject objectToSpawn;
}

[System.Serializable]
public class Ingredient
{
    public string name;
    public Sprite sprite;
    public string eggToMake;
    public string seedToMake;
}

[System.Serializable]
public class Fruit
{
    public string name;
    public Sprite sprite;
    public int amt;
}


[System.Serializable]
public class EggInInventory
{
    public string name;
    public int amt;
}

[System.Serializable]
public class SeedInInventory
{
    public string name;
    public int amt;
}

[System.Serializable]
public class IngredientInInventory
{
    public string name;
    public int amt;
}


[System.Serializable]
public class Inventory
{
    public List<EggInInventory> eggs;
    public List<SeedInInventory> seeds;
    public List<IngredientInInventory> ingredients;
    //public List<Fruit> fruit;
}


[System.Serializable]
public class JsonInventory
{
    public string[] eggs;
    public int[] eggAmts;

    public string[] seeds;
    public int[] seedAmts;

    public string[] ingredients;
    public int[] ingredientAmts;
}

#endregion

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript Instance;

    public GameObject HUDPanel;
    public GameObject inventoryPanel;
    public GameObject fruitPanel;

    [Space(10)]

    [SerializeField]
    public Egg[] eggs;

    [SerializeField]
    public Seed[] seeds;

    [SerializeField]
    public Ingredient[] ingredients;

    [Space(10)]

    public Inventory inventory;

    [Space(10)]

    public GameObject inventroryEggButton;
    public GameObject inventrorySeedButton;
    public GameObject ingredientButton;

    public GameObject EggUI;
    public GameObject SeedUI;

    string fileName = @"/Inventory.json";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadInventory();
        //StartCoroutine(AutoSaveInventory());

        if(PlayerPrefs.GetInt("FirstOpen") == 0)
        {
            FirstOpen();
        }

    }

    void FirstOpen()
    {
        Instantiate(eggs[0].objectToSpawn, new Vector2(Random.Range(-3, 3), 0.5f), Quaternion.identity, WorldSelector.Instance.SelectedIsland.transform);
        Instantiate(eggs[1].objectToSpawn, new Vector2(Random.Range(-3, 3), 0.5f), Quaternion.identity, WorldSelector.Instance.SelectedIsland.transform);
        CreatureManager.Instance.SaveCreatures();
        PlayerPrefs.SetInt("FirstOpen", 1); 
    }

    IEnumerator AutoSaveInventory()
    {
        while (true)
        {
            SaveInventory();
            yield return new WaitForSeconds(10);
        }
    }

    public void SaveInventory()
    {
        //Debug.Log("SAVING INVENTORY");

        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        if (File.Exists(filePath))
        {
            JsonInventory jsonInventory = new JsonInventory()
            {
                eggs = new string[inventory.eggs.Count],
                eggAmts = new int[inventory.eggs.Count],
                seeds = new string[inventory.seeds.Count],
                seedAmts = new int[inventory.seeds.Count],
                ingredients = new string[inventory.ingredients.Count],
                ingredientAmts = new int[inventory.ingredients.Count],
            };

            for (int i = 0; i < jsonInventory.eggs.Length; i++)
            {
                jsonInventory.eggs[i] = inventory.eggs[i].name;
                jsonInventory.eggAmts[i] = inventory.eggs[i].amt;
            }

            for (int i = 0; i < jsonInventory.seeds.Length; i++)
            {
                jsonInventory.seeds[i] = inventory.seeds[i].name;
                jsonInventory.seedAmts[i] = inventory.seeds[i].amt;
            }

            for (int i = 0; i < jsonInventory.ingredients.Length; i++)
            {
                jsonInventory.ingredients[i] = inventory.ingredients[i].name;
                jsonInventory.ingredientAmts[i] = inventory.ingredients[i].amt;
            }

            string dataAsJson = JsonUtility.ToJson(jsonInventory);

            File.WriteAllText(filePath, dataAsJson);
            //Debug.Log("Saved inventory");

        }
        else
        {
            File.Create(filePath).Dispose();
            SaveInventory();
        }
    }

    void LoadInventory()
    {
        //Debug.Log("LOADING INVENTORY");
        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JsonInventory jsonInventory = JsonUtility.FromJson<JsonInventory>(dataAsJson);

            Inventory newInventory = new Inventory() {
                eggs = new List<EggInInventory>(),
                seeds = new List<SeedInInventory>(),
                ingredients = new List<IngredientInInventory>()
            };

            for (int i = 0; i < jsonInventory.eggs.Length; i++)
            {
                EggInInventory newEgg = new EggInInventory() {
                    name = jsonInventory.eggs[i],
                    amt = jsonInventory.eggAmts[i]
                };
                newInventory.eggs.Add(newEgg);
            }

            for (int i = 0; i < jsonInventory.seeds.Length; i++)
            {
                SeedInInventory newSeed = new SeedInInventory()
                {
                    name = jsonInventory.seeds[i],
                    amt = jsonInventory.seedAmts[i]
                };
                newInventory.seeds.Add(newSeed);
            }

            for (int i = 0; i < jsonInventory.ingredients.Length; i++)
            {
                IngredientInInventory newIngredient = new IngredientInInventory()
                {
                    name = jsonInventory.ingredients[i],
                    amt = jsonInventory.ingredientAmts[i]
                };
                newInventory.ingredients.Add(newIngredient);
            }

            inventory = newInventory;

            //Debug.Log("LOADED INVENTORY");
        }
        else
        {
            SaveInventory();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadInventory();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveInventory();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("FirstOpen", 0);
        }
    }

    public Egg FindEgg(string eggToFind)
    {
        foreach (Egg egg in eggs)
        {
            if(egg.name == eggToFind)
            {
                return egg;
            }
        }

        Debug.Log("Couldnt find egg");

        return null;
    }

    public Seed FindSeed(string seedToFind)
    {
        foreach (Seed seed in seeds)
        {
            if (seed.name == seedToFind)
            {
                return seed;
            }
        }


        Debug.Log("Couldnt find seed");

        return null;
    }

    public Ingredient FindIngredient(string ingredientToFind)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.name == ingredientToFind)
            {
                return ingredient;
            }
        }

        Debug.Log("Couldnt find Ingredient");

        return null;
    }

    public void UpdateEggUI(GameObject eggButtonPanel)
    {
        foreach (Transform item in eggButtonPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (EggInInventory egg in inventory.eggs)
        {
            GameObject newButton = Instantiate(inventroryEggButton, eggButtonPanel.transform);
            newButton.GetComponent<InventoryEggButton>().SetUpButton(egg);
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

        EggInInventory egg = inventory.eggs[0];
        buttons[0].GetComponent<InventoryEggButton>().SetUpButton(egg);

        SeedInInventory seed = inventory.seeds[0];
        buttons[1].GetComponent<InventorySeedButton>().SetUpButton(seed);
    }

    public void UpdateSeedUI(GameObject seedButtonPanel)
    {
        foreach (Transform item in seedButtonPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (SeedInInventory seed in inventory.seeds)
        {
            GameObject newButton = Instantiate(inventrorySeedButton, seedButtonPanel.transform);
            newButton.GetComponent<InventorySeedButton>().SetUpButton(seed);
        }
    }

    public void UpdateIngredientsUI(GameObject ingredientButtonPanel)
    {
        foreach (Transform item in ingredientButtonPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (IngredientInInventory ingredient in inventory.ingredients)
        {
            GameObject newButton = Instantiate(ingredientButton, ingredientButtonPanel.transform);
            newButton.GetComponent<IngredientButton>().SetUpButton(ingredient);
        }
    }

    public CraftingObject craftingObject;
    public CraftingIngredient craftingIngredient;
    public CraftingOutcome craftingOutcome;

    public void UpdateIngredient(Ingredient ingredient)
    {
        for (int i = 0; i < inventory.ingredients.Count; i++)
        {
            IngredientInInventory ingredientToEdit = new IngredientInInventory();
            if (inventory.ingredients[i].name == ingredient.name)
            {
                ingredientToEdit = inventory.ingredients[i];

                ingredientToEdit.amt -= 1;
                inventory.ingredients.RemoveAt(i);
                inventory.ingredients.Insert(i, ingredientToEdit);
                break;
            }
        }
    }

    public void AddSeed(SeedInInventory seed)
    {
        bool newSeed = true;
        for (int i = 0; i < inventory.seeds.Count; i++)
        {
            if (inventory.seeds[i].name == seed.name)
            {
                newSeed = false;
                SeedInInventory seedToEdit = inventory.seeds[i];
                seedToEdit.amt += 1;
                inventory.seeds[i] = seedToEdit;
                Debug.Log("Added");
                break;
            }
        }

        if (newSeed)
        {
            inventory.seeds.Add(seed);
        }

        SaveInventory();
    }

    public void AddSeed(string seed)
    {
        bool isNewSeed = true;
        for (int i = 0; i < inventory.seeds.Count; i++)
        {
            if (inventory.seeds[i].name == seed)
            {
                isNewSeed = false;
                SeedInInventory seedToEdit = inventory.seeds[i];
                seedToEdit.amt += 1;
                inventory.seeds[i] = seedToEdit;
                Debug.Log("Added");
                break;
            }
        }

        if (isNewSeed)
        {
            SeedInInventory newSeed = new SeedInInventory();
            newSeed.amt = 1;
            newSeed.name = seed;
            inventory.seeds.Add(newSeed);
        }
        SaveInventory();
    }

    public void AddEgg(EggInInventory egg)
    {
        bool newEgg = true;
        for (int i = 0; i < inventory.eggs.Count; i++)
        {
            if (inventory.eggs[i].name == egg.name)
            {
                newEgg = false;
                EggInInventory eggToEdit = inventory.eggs[i];
                eggToEdit.amt += 1;
                inventory.eggs[i] = eggToEdit;  
                Debug.Log("Added");
                break;
            }
        }

        if (newEgg)
        {
            inventory.eggs.Add(egg);
        }

        SaveInventory();
    }

    public void AddEgg(string egg)
    {
        bool isNewEgg = true;
        for (int i = 0; i < inventory.eggs.Count; i++)
        {
            if (inventory.eggs[i].name == egg)
            {
                isNewEgg = false;
                EggInInventory eggToEdit = inventory.eggs[i];
                eggToEdit.amt += 1;
                inventory.eggs[i] = eggToEdit;
                Debug.Log("Added");
                break;
            }
        }

        if (isNewEgg)
        {
            EggInInventory newEgg = new EggInInventory();
            newEgg.name = egg;
            newEgg.amt = 1;
            inventory.eggs.Add(newEgg);
        }

        SaveInventory();
    }

    /*public void AddFruit(Fruit fruit)
    {
        bool newFruit = true;
        for (int i = 0; i < inventory.fruit.Count; i++)
        {
            if (inventory.fruit[i].name == fruit.name)
            {
                newFruit = false;
                Fruit fruitToEdit = inventory.fruit[i];
                fruitToEdit.amt += 1;
                inventory.fruit[i] = fruitToEdit;
                Debug.Log("Added");
                break;
            }
        }

        if (newFruit)
        {
            inventory.fruit.Add(fruit);
        }
    }*/

    public void RemoveSeed(SeedInInventory seed)
    {
        for (int i = 0; i < inventory.seeds.Count; i++)
        {
            SeedInInventory seedToEdit = new SeedInInventory();
            if (inventory.seeds[i].name == seed.name)
            {
                seedToEdit = inventory.seeds[i];

                seedToEdit.amt -= 1;
                inventory.seeds.RemoveAt(i);
                inventory.seeds.Insert(i, seedToEdit);
                break;
            }
        }

        SaveInventory();
    }

    public void RemoveSeed(string seedName)
    {
        for (int i = 0; i < inventory.seeds.Count; i++)
        {
            SeedInInventory seedToEdit = new SeedInInventory();
            if (inventory.seeds[i].name == seedName)
            {
                seedToEdit = inventory.seeds[i];

                seedToEdit.amt -= 1;
                inventory.seeds.RemoveAt(i);
                inventory.seeds.Insert(i, seedToEdit);
                break;
            }
        }

        SaveInventory();
    }

    public void RemoveEgg(EggInInventory egg)
    {
        for (int i = 0; i < inventory.eggs.Count; i++)
        {
            EggInInventory eggToEdit = new EggInInventory();
            if (inventory.eggs[i].name == egg.name)
            {
                eggToEdit = inventory.eggs[i];

                eggToEdit.amt -= 1;
                inventory.eggs.RemoveAt(i);
                inventory.eggs.Insert(i, eggToEdit);
                break;
            }
        }

        SaveInventory();
    }

    public void RemoveEgg(string eggName)
    {
        for (int i = 0; i < inventory.eggs.Count; i++)
        {
            EggInInventory eggToEdit = new EggInInventory();
            if (inventory.eggs[i].name == eggName)
            {
                eggToEdit = inventory.eggs[i];

                eggToEdit.amt -= 1;
                inventory.eggs.RemoveAt(i);
                inventory.eggs.Insert(i, eggToEdit);
                break;
            }
        }

        SaveInventory();
    }

    /*public void RemoveFruit(Fruit fruit)
    {
        for (int i = 0; i < inventory.fruit.Count; i++)
        {
            Fruit fruitToEdit = new Fruit();
            if (inventory.fruit[i].name == fruit.name)
            {
                fruitToEdit = inventory.fruit[i];

                fruitToEdit.amt -= 1;
                inventory.fruit.RemoveAt(i);
                inventory.fruit.Insert(i, fruitToEdit);
                break;
            }
        }
    }*/

    public void RemoveIngredient(IngredientInInventory ingredient)
    {
        for (int i = 0; i < inventory.ingredients.Count; i++)
        {
            IngredientInInventory ingredientToEdit = new IngredientInInventory();
            if (inventory.ingredients[i].name == ingredient.name)
            {
                ingredientToEdit = inventory.ingredients[i];

                ingredientToEdit.amt -= 1;
                inventory.ingredients.RemoveAt(i);
                inventory.ingredients.Insert(i, ingredientToEdit);
                break;
            }
        }
        SaveInventory();
    }

    public void SetCraftingObject(EggInInventory newEgg)
    {
        craftingObject.egg = newEgg;
        craftingObject.thisObjectType = CraftingObject.ObjectType.egg;
        craftingObject.SetSprite();
        craftingOutcome.GetOutcome();
    }

    public void SetCraftingObject(SeedInInventory newSeed)
    {
        craftingObject.seed = newSeed;
        craftingObject.thisObjectType = CraftingObject.ObjectType.seed;
        craftingObject.SetSprite();
        craftingOutcome.GetOutcome();
    }

    public void SetCraftingIngredient(IngredientInInventory newIngredient)
    {
        craftingIngredient.ingredient = newIngredient;
        craftingIngredient.SetSprite();
        craftingOutcome.GetOutcome();
    }
}
