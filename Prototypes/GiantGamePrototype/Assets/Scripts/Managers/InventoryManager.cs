using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject objectToSpawn;
}

[System.Serializable]
public class Chest
{
    public string name;
    public Sprite sprite;
    public ChestItem[] PossibleItems;
}

[System.Serializable]
public class ChestItem
{
    public string name;
    public int amount;
    public float weight;
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
public class FruitInInventory
{
    public string name;
    public int amt;
}

[System.Serializable]
public class ChestInInventory
{
    public string name;
    public int amt;
}

[System.Serializable]
public class IslandStorage
{
    public List<FruitInInventory> fruitInStorage;
}

[System.Serializable]
public class Inventory
{
    public int currency;
    public List<EggInInventory> eggs;
    public List<SeedInInventory> seeds;
    public List<IslandStorage> islands;
    public List<ChestInInventory> chests;
    //public List<Fruit> fruit;
}


[System.Serializable]
public class JsonInventory
{
    public int currencyAmt;

    public string[] eggs;
    public int[] eggAmts;

    public string[] seeds;
    public int[] seedAmts;

    public string[] chests;
    public int[] chestAmts;

    public string[] fruits0;
    public int[] fruitAmts0;

    public string[] fruits1;
    public int[] fruitAmts1;

    public string[] fruits2;
    public int[] fruitAmts2;
}

#endregion

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject HUDPanel;
    public GameObject inventoryPanel;
    public GameObject fruitPanel;

    [Space(10)]

    [SerializeField]
    public Egg[] eggs;

    [SerializeField]
    public Seed[] seeds;

    [SerializeField]
    public Fruit[] fruits;

    [SerializeField]
    public Ingredient[] ingredients;

    [SerializeField]
    public Chest[] chests;

    [Space(10)]

    public Inventory inventory;

    [Space(10)]

    public GameObject inventroryEggButton;
    public GameObject inventrorySeedButton;
    public GameObject ingredientButton;

    public GameObject EggUI;
    public GameObject SeedUI;
    public GameObject[] SeedButtons;
    public GameObject[] FruitButtons;
    public GameObject currencyUI;

    string fileName = @"/Inventory.json";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadInventory();
        StartCoroutine(AutoSaveInventory());
        UpdateChestUI();
        if(PlayerPrefs.GetInt("FirstOpen") == 0)
        {
            FirstOpen();
        }
    }

    void FirstOpen()
    {
        Instantiate(eggs[0].objectToSpawn, new Vector2(Random.Range(-3, 3), 0.5f), Quaternion.identity, WorldManager.Instance.SelectedIsland.transform);
        Instantiate(eggs[1].objectToSpawn, new Vector2(Random.Range(-3, 3), 0.5f), Quaternion.identity, WorldManager.Instance.SelectedIsland.transform);
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
                chests = new string[inventory.chests.Count],
                chestAmts = new int[inventory.chests.Count],
                fruits0 = new string[inventory.islands[0].fruitInStorage.Count],
                fruitAmts0 = new int[inventory.islands[0].fruitInStorage.Count],
                fruits1 = new string[inventory.islands[1].fruitInStorage.Count],
                fruitAmts1 = new int[inventory.islands[1].fruitInStorage.Count],
                fruits2 = new string[inventory.islands[2].fruitInStorage.Count],
                fruitAmts2 = new int[inventory.islands[2].fruitInStorage.Count]
            };

            jsonInventory.currencyAmt = inventory.currency;

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

            for (int i = 0; i < jsonInventory.chests.Length; i++)
            {
                jsonInventory.chests[i] = inventory.chests[i].name;
                jsonInventory.chestAmts[i] = inventory.chests[i].amt;
            }

            for (int i = 0; i < inventory.islands[0].fruitInStorage.Count; i++)
            {
                jsonInventory.fruits0[i] = inventory.islands[0].fruitInStorage[i].name;
                jsonInventory.fruitAmts0[i] = inventory.islands[0].fruitInStorage[i].amt;
            }

            for (int i = 0; i < inventory.islands[1].fruitInStorage.Count; i++)
            {
                jsonInventory.fruits1[i] = inventory.islands[1].fruitInStorage[i].name;
                jsonInventory.fruitAmts1[i] = inventory.islands[1].fruitInStorage[i].amt;
            }

            for (int i = 0; i < inventory.islands[2].fruitInStorage.Count; i++)
            {
                jsonInventory.fruits2[i] = inventory.islands[2].fruitInStorage[i].name;
                jsonInventory.fruitAmts2[i] = inventory.islands[2].fruitInStorage[i].amt;
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
                islands = new List<IslandStorage>(),
                chests = new List<ChestInInventory>()
            };

            newInventory.currency = jsonInventory.currencyAmt;
            currencyUI.GetComponent<Text>().text = newInventory.currency.ToString();

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

            for (int i = 0; i < jsonInventory.chests.Length; i++)
            {
                ChestInInventory newChest = new ChestInInventory()
                {
                    name = jsonInventory.chests[i],
                    amt = jsonInventory.chestAmts[i]
                };
                newInventory.chests.Add(newChest);
            }

            IslandStorage newStorage = new IslandStorage() {
                fruitInStorage = new List<FruitInInventory>()
            };
            for (int i = 0; i < jsonInventory.fruits0.Length; i++)
            {
                FruitInInventory newFruitInInventory = new FruitInInventory();
                newFruitInInventory.name = jsonInventory.fruits0[i];
                newFruitInInventory.amt = jsonInventory.fruitAmts0[i];
                newStorage.fruitInStorage.Add(newFruitInInventory);
                //Debug.Log("Added fruit");
            }
            newInventory.islands.Add(newStorage);

            newStorage = new IslandStorage()
            {
                fruitInStorage = new List<FruitInInventory>()
            };
            for (int i = 0; i < jsonInventory.fruits1.Length; i++)
            {
                FruitInInventory newFruitInInventory = new FruitInInventory();
                newFruitInInventory.name = jsonInventory.fruits1[i];
                newFruitInInventory.amt = jsonInventory.fruitAmts1[i];
                newStorage.fruitInStorage.Add(newFruitInInventory);
                //Debug.Log("Added fruit");
            }
            newInventory.islands.Add(newStorage);

            newStorage = new IslandStorage()
            {
                fruitInStorage = new List<FruitInInventory>()
            };
            for (int i = 0; i < jsonInventory.fruits0.Length; i++)
            {
                FruitInInventory newFruitInInventory = new FruitInInventory();
                newFruitInInventory.name = jsonInventory.fruits2[i];
                newFruitInInventory.amt = jsonInventory.fruitAmts2[i];
                newStorage.fruitInStorage.Add(newFruitInInventory);
                //Debug.Log("Added fruit");
            }
            newInventory.islands.Add(newStorage);

            inventory = newInventory;

            //Debug.Log("LOADED INVENTORY");
        }
        else
        {
            SaveInventory();
        }
    }

    public void ResetSave()
    {
        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            TreeManager.Instance.ResetSave();
            CreatureManager.Instance.ResetSave();
            PlayerPrefs.SetInt("PlayTutorial", 0);
            SceneManager.LoadScene(0);
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
            ResetSave();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            AddCurrency(10);
        }
    }

    public void AddCurrency(int amt)
    {
        inventory.currency += amt;
        currencyUI.GetComponent<Text>().text = inventory.currency.ToString();
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


        //Debug.Log("Couldnt find seed");

        return null;
    }

    public Fruit FindFruit(string fruitToFind)
    {
        foreach (Fruit fruit in fruits)
        {
            if (fruit.name == fruitToFind)
            {
                //Debug.Log("Found fruit: " + fruit.name);
                return fruit;
            }
        }


        //Debug.Log("Couldnt find fruit");

        return null;
    }

    public Chest FindChest(string chestToFind)
    {
        foreach (Chest chest in chests)
        {
            if (chest.name == chestToFind)
            {
                return chest;
            }
        }

        //Debug.Log("Couldnt find egg");

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

    public void UpdateFruitButtons(int islandID)
    {
        foreach (GameObject item in FruitButtons)
        {
            item.GetComponent<FruitButtonScript>().childAmtCounter.text = "x" + FindFruitInInventory(item.GetComponent<FruitButtonScript>().fruitType, islandID).amt.ToString();
        }
    }

    public void UpdateSeedButtons()
    {
        foreach (GameObject item in SeedButtons)
        {
            item.GetComponent<InventorySeedButton>().childAmtCounter.text = "x" + FindSeedInInventory(item.GetComponent<InventorySeedButton>().thisSeed.name).amt.ToString();
        }
    }

    public SeedInInventory FindSeedInInventory(string seedName)
    {
        foreach (var item in inventory.seeds)
        {
            if(item.name == seedName)
            {
                return item;
            }
        }
        return null;
    }

    public FruitInInventory FindFruitInInventory(string fruitType, int islandID)
    {
        foreach (var item in inventory.islands[islandID].fruitInStorage)
        {
            if (item.name == fruitType)
            {
                return item;
            }
        }
        Debug.Log("Couldnt find fruit " + fruitType);
        return null;
    }

    public ChestInInventory FindChestInInventory(string chestName)
    {
        foreach (var item in inventory.chests)
        {
            if (item.name == chestName)
            {
                return item;
            }
        }
        Debug.Log("Couldnt find Chest " + chestName);
        return null;
    }

    [Header("Chests")]
    public GameObject ChestCounter;
    public GameObject ChestSlider;
    public GameObject NochestText;
    public GameObject ChestPrefab;

    public void UpdateChestUI()
    {
        if(inventory.chests.Count > 0)
        {
            

            int chestAmt = 0;
            foreach (var item in inventory.chests)
            {
                chestAmt += item.amt;
            }
            ChestCounter.GetComponentInChildren<TextMeshProUGUI>().text = chestAmt.ToString();
            ChestCounter.SetActive(true);


            NochestText.SetActive(false);
            ChestSlider.SetActive(true);

            for (int i = 0; i < ChestSlider.transform.childCount; i++)
            {
                Destroy(ChestSlider.transform.GetChild(i).gameObject);
            }

            foreach (var chest in inventory.chests)
            {
                GameObject newChest = Instantiate(ChestPrefab, ChestSlider.transform);
                newChest.GetComponent<ChestButton>().SetUpChest(FindChest(chest.name), chest.amt);
            }
        }
        else
        {
            ChestCounter.SetActive(false);
            NochestText.SetActive(true);
            ChestSlider.SetActive(false);
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
                //Debug.Log("Added");
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

    public void AddFruit(string fruitName, int islandID)
    {
        for (int i = 0; i < inventory.islands.Count; i++)
        {
            bool isNewFruit = true;
            FruitInInventory fruitToEdit = new FruitInInventory();
            for (int j = 0; j < inventory.islands[i].fruitInStorage.Count; j++)
            {
                if (inventory.islands[i].fruitInStorage[j].name == fruitName)
                {
                    isNewFruit = false;
                    fruitToEdit = inventory.islands[i].fruitInStorage[j];

                    fruitToEdit.amt += 1;
                    inventory.islands[i].fruitInStorage.RemoveAt(j);
                    inventory.islands[i].fruitInStorage.Insert(j, fruitToEdit);
                    break;
                }
            }

            if (isNewFruit)
            {
                fruitToEdit.name = fruitName;
                fruitToEdit.amt = 1;
                inventory.islands[islandID].fruitInStorage.Add(fruitToEdit);
            }
        }
        SaveInventory();
    }

    public void AddChest(string chest)
    {
        bool isNewChest = true;
        for (int i = 0; i < inventory.chests.Count; i++)
        {
            if (inventory.chests[i].name == chest)
            {
                isNewChest = false;
                ChestInInventory chestToEdit = inventory.chests[i];
                chestToEdit.amt += 1;
                inventory.chests[i] = chestToEdit;
                //Debug.Log("Added");
                break;
            }
        }

        if (isNewChest)
        {
            ChestInInventory newChest = new ChestInInventory();
            newChest.amt = 1;
            newChest.name = chest;
            inventory.chests.Add(newChest);
            Debug.Log("Added");
        }
        UpdateChestUI();
        SaveInventory();
    }

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

    public void RemoveFruit(FruitInInventory fruit)
    {
        for (int i = 0; i < inventory.islands.Count; i++)
        {
            for (int j = 0; j < inventory.islands[i].fruitInStorage.Count; j++)
            {
                FruitInInventory fruitToEdit = new FruitInInventory();
                if (inventory.islands[i].fruitInStorage[j].name == fruit.name)
                {
                    fruitToEdit = inventory.islands[i].fruitInStorage[j];

                    fruitToEdit.amt -= 1;
                    inventory.islands[i].fruitInStorage.RemoveAt(i);
                    inventory.islands[i].fruitInStorage.Insert(i, fruitToEdit);
                    break;
                }
            }
        }
    }

    public void RemoveFruit(string fruitName, int islandID)
    {
        for (int i = 0; i < inventory.islands.Count; i++)
        {
            FruitInInventory fruitToEdit = new FruitInInventory();
            for (int j = 0; j < inventory.islands[i].fruitInStorage.Count; j++)
            {
                if (inventory.islands[i].fruitInStorage[j].name == fruitName)
                {
                    fruitToEdit = inventory.islands[i].fruitInStorage[j];

                    fruitToEdit.amt -= 1;
                    inventory.islands[i].fruitInStorage.RemoveAt(j);
                    inventory.islands[i].fruitInStorage.Insert(j, fruitToEdit);
                    break;
                }

            }
        }

        SaveInventory();
    }

    public void RemoveChest(string chest)
    {
        for (int i = 0; i < inventory.seeds.Count; i++)
        {
            ChestInInventory chestToEdit = new ChestInInventory();
            if (inventory.chests[i].name == chest)
            {
                chestToEdit = inventory.chests[i];

                chestToEdit.amt -= 1;
                inventory.chests.RemoveAt(i);
                if (chestToEdit.amt > 0)
                {
                    inventory.chests.Insert(i, chestToEdit);
                }
                Debug.Log("Removed chest: " + chest);
                UpdateChestUI();
                SaveInventory();
                return;
            }
        }
        Debug.Log("Couldn't remove chest: " + chest);
        UpdateChestUI();
        SaveInventory();
    }

    /* Old crafting code, will stay for a while incase crafting returns
    public Ingredient FindIngredient(string ingredientToFind)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.name == ingredientToFind)
            {
                return ingredient;
            }
            //Debug.Log(ingredientToFind + " IM not " + ingredient.name);
        }

        Debug.Log("Couldnt find Ingredient");

        return null;
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
    }*/
}
