using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Tree
{
    public string name;
    public GameObject treeToSpawn;
}

public class JsonTrees
{
    public string[] types;
    public float[] growTimers;
    public float[] timeToGrows;
    public int[] worldIds;
    public int[] potIds;
}

public class TreeManager : MonoBehaviour {

    public static TreeManager Instance; 

    [SerializeField]
    List<Tree> Trees;

    public List<GameObject> TreesInWorld;

    public Vector2 spawnBounds;

    string fileName = @"/Trees.json";

    private void Start()
    {
        Instance = this;
        LoadTrees();
        //StartCoroutine(AutoSaveTrees());
    }

    void LoadTrees()
    {
        //Debug.Log("LOADING TREES");
        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JsonTrees jsonTrees = JsonUtility.FromJson<JsonTrees>(dataAsJson);

            //Debug.Log(jsonTrees.types[0]);

            for (int i = 0; i < jsonTrees.types.Length; i++)
            {
                GameObject newTree = Instantiate(
                    FindTree(jsonTrees.types[i]),
                    WorldSelector.Instance.islands[jsonTrees.worldIds[i]].GetComponent<IslandScript>().plantPots[jsonTrees.potIds[i]].transform.position,
                    Quaternion.identity, 
                    WorldSelector.Instance.islands[jsonTrees.worldIds[i]].GetComponent<IslandScript>().plantPots[jsonTrees.potIds[i]].transform
                    );
                TreesInWorld.Add(newTree);
                WorldSelector.Instance.islands[jsonTrees.worldIds[i]].GetComponent<IslandScript>().currentTreePopulation++;
                TreeScript TS = newTree.GetComponent<TreeScript>();
                TS.growTimer = jsonTrees.growTimers[i];
                TS.timeToGrow = jsonTrees.timeToGrows[i];
                TS.loadedFromSave = true;
            }

            //Debug.Log("Loaded trees");
        }
        else
        {
           SaveTrees();
        }
    }

    public void SaveTrees()
    {
        //Debug.Log("SAVING TREES");

        string filePath = Application.persistentDataPath + fileName;
        //string filePath = fileName;

        //Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            JsonTrees jsonTrees = new JsonTrees()
            {
                types = new string[TreesInWorld.Count],
                growTimers = new float[TreesInWorld.Count],
                timeToGrows = new float[TreesInWorld.Count],
                worldIds = new int[TreesInWorld.Count],
                potIds = new int[TreesInWorld.Count]
            };

            int i = 0;
            foreach (var tree in TreesInWorld)
            {
                TreeScript TS = tree.GetComponent<TreeScript>();
                jsonTrees.types[i] = TS.treeType;
                jsonTrees.growTimers[i] = TS.growTimer;
                jsonTrees.timeToGrows[i] = TS.timeToGrow;
                jsonTrees.worldIds[i] = tree.transform.parent.GetComponentInParent<IslandScript>().islandID;
                jsonTrees.potIds[i] = tree.transform.GetComponentInParent<PlantPot>().potID;
                i++;
            }

            string dataAsJson = JsonUtility.ToJson(jsonTrees);

            File.WriteAllText(filePath, dataAsJson);

            //Debug.Log("saved trees");
        }
        else
        {
            File.Create(filePath).Dispose();
            SaveTrees();
        }
    }

    GameObject FindTree(string nameToFind)
    {
        foreach (var tree in Trees)
        {
            if (tree.name == nameToFind)
            {
                return tree.treeToSpawn;
            }
        }
        return null;
    }

    IEnumerator AutoSaveTrees()
    {
        while (true)
        {
            SaveTrees();
            yield return new WaitForSeconds(10);
        }
    }
}
