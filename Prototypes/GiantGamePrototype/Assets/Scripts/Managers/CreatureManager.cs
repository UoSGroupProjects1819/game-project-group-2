using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region Creature Classes

[System.Serializable]
public class SavedIntelligence
{
    public string rank;
    public int level;
    public int upgradePoints;
    public int score;
}

[System.Serializable]
public class SavedAgility
{
    public string rank;
    public int level;
    public int upgradePoints;
    public int score;
}

[System.Serializable]
public class SavedStrength
{
    public string rank;
    public int level;
    public int upgradePoints;
    public int score;
}

[System.Serializable]
public class SavedStyle
{
    public string rank;
    public int level;
    public int upgradePoints;
    public int score;
}

[System.Serializable]
public class SavedStamina
{
    public string rank;
    public int level;
    public int upgradePoints;
    public int score;
}

[System.Serializable]
public class Creature
{
    public string name;
    public GameObject CreatureToSpawn;
}

[System.Serializable]
public class BreedingResult
{
    public string creature1;
    public string creature2;
    public GameObject EggToSpawn;
}

public class JsonCreature
{
    public string name;
    public string type;
    public float happiness;
    public float size;
    public SavedIntelligence savedIntellegence;
    public SavedAgility savedAgility;
    public SavedStrength savedStrength;
    public SavedStyle savedStyle;
    public SavedStamina savedStamina;
}

public class JsonCreatures
{
    public string[] names;
    public string[] types;
    public int[] worldIds;
    public float[] happiness;
    public float[] sizes;
    public int[] stars;

    public string[] intelligenceRank;
    public int[] intelligenceLevel;
    public int[] intelligenceUpgradePoints;
    public int[] intelligenceScore;

    public string[] agilityRank;
    public int[] agilityLevel;
    public int[] agilityUpgradePoints;
    public int[] agilityScore;

    public string[] strengthRank;
    public int[] strengthLevel;
    public int[] strengthUpgradePoints;
    public int[] strengthScore;

    public string[] styleRank;
    public int[] styleLevel;
    public int[] styleUpgradePoints;
    public int[] styleScore;

    public string[] staminaRank;
    public int[] staminaLevel;
    public int[] staminaUpgradePoints;
    public int[] staminaScore;
}
#endregion

public class CreatureManager : MonoBehaviour {

    public static CreatureManager Instance;

    [SerializeField]
    public Creature[] creatures;

    [SerializeField]
    public BreedingResult[] breedingResults;

    [Space(10)]

    public List<GameObject> creaturesInWorld;

    public Vector2 spawnBounds;

    string fileName = @"/Creatures.json";

    public float nextSpawnDepth;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        LoadCreatures();
        //StartCoroutine(AutoSaveCreatures());
	}

    void Update () {
        if(Input.GetKeyDown(KeyCode.K)){
            SaveCreatures();
        }
	}

    public GameObject GetBreedingResult(string firstCreature, string secondCreature)
    {
        foreach (var result in breedingResults)
        {
            if(result.creature1 == firstCreature && result.creature2 == secondCreature)
            {
                return result.EggToSpawn;
            }
            else
            if (result.creature1 == secondCreature && result.creature2 == firstCreature)
            {
                return result.EggToSpawn;
            }
        }

        Debug.Log("Couldn't Find Breeding Result");
        return null;
    }

    GameObject FindCreature(string nameToFind)
    {
        foreach (var creature in creatures)
        {
            if (creature.name == nameToFind)
            {
                return creature.CreatureToSpawn;
            }
        }
        return null;
    }

    void LoadCreatures()
    {
        //Debug.Log("LOADING CREATURES");
        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JsonCreatures jsonCreatures = JsonUtility.FromJson<JsonCreatures>(dataAsJson);

            //Debug.Log(jsonCreatures.types[0]);

            for (int i = 0; i < jsonCreatures.names.Length; i++)
            {
                GameObject newCreature = Instantiate(
                    FindCreature(jsonCreatures.types[i]), 
                    new Vector3(Random.Range(spawnBounds.x, spawnBounds.y), 0.5f, nextSpawnDepth) + WorldManager.Instance.islands[jsonCreatures.worldIds[i]].transform.position, 
                    Quaternion.identity, WorldManager.Instance.islands[jsonCreatures.worldIds[i]].transform
                );

                nextSpawnDepth -= 0.05f;

                creaturesInWorld.Add(newCreature);
                WorldManager.Instance.islands[jsonCreatures.worldIds[i]].GetComponent<IslandScript>().currentCreaturePopulation++;
                SavedIntelligence intelligence = new SavedIntelligence()
                {
                    rank = jsonCreatures.intelligenceRank[i],
                    level = jsonCreatures.intelligenceLevel[i],
                    upgradePoints = jsonCreatures.intelligenceUpgradePoints[i],
                    score = jsonCreatures.intelligenceScore[i]
                };

                SavedAgility agility = new SavedAgility()
                {
                    rank = jsonCreatures.agilityRank[i],
                    level = jsonCreatures.agilityLevel[i],
                    upgradePoints = jsonCreatures.agilityUpgradePoints[i],
                    score = jsonCreatures.agilityScore[i]
                };

                SavedStrength strength = new SavedStrength()
                {
                    rank = jsonCreatures.strengthRank[i],
                    level = jsonCreatures.strengthLevel[i],
                    upgradePoints = jsonCreatures.strengthUpgradePoints[i],
                    score = jsonCreatures.strengthScore[i]
                };

                SavedStyle style = new SavedStyle()
                {
                    rank = jsonCreatures.styleRank[i],
                    level = jsonCreatures.styleLevel[i],
                    upgradePoints = jsonCreatures.styleUpgradePoints[i],
                    score = jsonCreatures.styleScore[i]
                };

                SavedStamina stamina = new SavedStamina()
                {
                    rank = jsonCreatures.staminaRank[i],
                    level = jsonCreatures.staminaLevel[i],
                    upgradePoints = jsonCreatures.staminaUpgradePoints[i],
                    score = jsonCreatures.staminaScore[i]
                };

                newCreature.GetComponent<CreatureScript>().SetUpCreature(
                    jsonCreatures.names[i], 
                    jsonCreatures.happiness[i], 
                    jsonCreatures.sizes[i], 
                    jsonCreatures.stars[i],
                    intelligence, 
                    agility,
                    strength,
                    style,
                    stamina
                    );
                
            }

            //Debug.Log("Loaded creatures");
        }
        else
        {
            SaveCreatures();
        }
    }

    public void SaveCreatures()
    {
        //Debug.Log("SAVING CREATURES");

        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        //Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            JsonCreatures jsonCreatures = new JsonCreatures()
            {
                names = new string[creaturesInWorld.Count],
                types = new string[creaturesInWorld.Count],
                worldIds = new int[creaturesInWorld.Count],
                happiness = new float[creaturesInWorld.Count],
                sizes = new float[creaturesInWorld.Count],
                stars = new int[creaturesInWorld.Count],
                intelligenceRank = new string[creaturesInWorld.Count],
                intelligenceLevel = new int[creaturesInWorld.Count],
                intelligenceUpgradePoints = new int[creaturesInWorld.Count],
                intelligenceScore = new int[creaturesInWorld.Count],

                agilityRank = new string[creaturesInWorld.Count],
                agilityLevel = new int[creaturesInWorld.Count],
                agilityUpgradePoints = new int[creaturesInWorld.Count],
                agilityScore = new int[creaturesInWorld.Count],

                strengthRank = new string[creaturesInWorld.Count],
                strengthLevel = new int[creaturesInWorld.Count],
                strengthUpgradePoints = new int[creaturesInWorld.Count],
                strengthScore = new int[creaturesInWorld.Count],

                styleRank = new string[creaturesInWorld.Count],
                styleLevel = new int[creaturesInWorld.Count],
                styleUpgradePoints = new int[creaturesInWorld.Count],
                styleScore = new int[creaturesInWorld.Count],

                staminaRank = new string[creaturesInWorld.Count],
                staminaLevel = new int[creaturesInWorld.Count],
                staminaUpgradePoints = new int[creaturesInWorld.Count],
                staminaScore = new int[creaturesInWorld.Count],
            };
            int i = 0;
            foreach (var creature in creaturesInWorld)
            {
                CreatureScript CS = creature.GetComponent<CreatureScript>();

                jsonCreatures.names[i] = CS.name;
                jsonCreatures.happiness[i] = CS.happiness;
                jsonCreatures.sizes[i] = CS.size;
                jsonCreatures.types[i] = CS.type;
                jsonCreatures.worldIds[i] = creature.transform.parent.GetComponent<IslandScript>().islandID;
                jsonCreatures.stars[i] = CS.Stars;

                jsonCreatures.intelligenceRank[i] = CS.Intelligence.rank.name;
                jsonCreatures.intelligenceLevel[i] = CS.Intelligence.level;
                jsonCreatures.intelligenceUpgradePoints[i] = CS.Intelligence.upgradePoints;
                jsonCreatures.intelligenceScore[i] = CS.Intelligence.amt;

                jsonCreatures.agilityRank[i] = CS.Agility.rank.name;
                jsonCreatures.agilityLevel[i] = CS.Agility.level;
                jsonCreatures.agilityUpgradePoints[i] = CS.Agility.upgradePoints;
                jsonCreatures.agilityScore[i] = CS.Agility.amt;

                jsonCreatures.strengthRank[i] = CS.Strength.rank.name;
                jsonCreatures.strengthLevel[i] = CS.Strength.level;
                jsonCreatures.strengthUpgradePoints[i] = CS.Strength.upgradePoints;
                jsonCreatures.strengthScore[i] = CS.Strength.amt;

                jsonCreatures.styleRank[i] = CS.Style.rank.name;
                jsonCreatures.styleLevel[i] = CS.Style.level;
                jsonCreatures.styleUpgradePoints[i] = CS.Style.upgradePoints;
                jsonCreatures.styleScore[i] = CS.Style.amt;

                jsonCreatures.staminaRank[i] = CS.Stamina.rank.name;
                jsonCreatures.staminaLevel[i] = CS.Stamina.level;
                jsonCreatures.staminaUpgradePoints[i] = CS.Stamina.upgradePoints;
                jsonCreatures.staminaScore[i] = CS.Stamina.amt;
                i++;
            }

            string dataAsJson = JsonUtility.ToJson(jsonCreatures);

            File.WriteAllText(filePath, dataAsJson);

            //Debug.Log("Saved creatures");
        }
        else
        {
            File.Create(filePath).Dispose();
            SaveCreatures();
        }
    }

    public void ResetSave()
    {
        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    IEnumerator AutoSaveCreatures()
    {
        while (true)
        {
            SaveCreatures();
            yield return new WaitForSeconds(10);
        }
    }
}
