using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public Sprite questSprite;
    public RequiredLevel[] minLevels;
    public float timeRequired;
    public string[] rewards;
}

[System.Serializable]
public class ActiveQuest
{
    public string name;
    public float elapsedTime;
}

[System.Serializable]
public class RequiredLevel
{
    public string name;
    public float amt;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Quest[] quests;

    public List<string> availableQuests;
    public List<ActiveQuest> activeQuests;

    public Transform questUIHolder;
    public GameObject questUIPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateQuests();
    }

    private void Update()
    {
        foreach (var quest in activeQuests)
        {
            quest.elapsedTime -= Time.deltaTime;
            if (quest.elapsedTime < 0)
            {
                foreach (var reward in GetQuest(quest.name).rewards)
                {
                    //Debug.Log("Trying to give the gift of a " + reward);
                    if (InventoryManager.Instance.FindChest(reward) != null)
                    {
                        //Debug.Log("Got This far");
                        InventoryManager.Instance.AddChest(reward);
                        InventoryManager.Instance.UpdateChestUI();
                    }

                    if (InventoryManager.Instance.FindSeed(reward) != null)
                    {
                        InventoryManager.Instance.AddSeed(reward);
                    }

                    if (InventoryManager.Instance.FindFruit(reward) != null)
                    {
                        InventoryManager.Instance.AddFruit(reward, WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().islandID);
                    }
                    //Debug.Log("Gift given: " + reward);
                }
                activeQuests.Remove(quest);
                UpdateQuests();
            }
        }
    }

    private void UpdateQuests()
    {
        List<string> names = new List<string>();
        foreach (Transform quest in questUIHolder)
        {
            if (quest.gameObject.GetComponent<QuestScript>().timeLeft <= 1)
            {
                Destroy(quest.gameObject);
            }
            else
            {
                names.Add(quest.GetComponent<QuestScript>().thisQuest.name);
            }
        }

        foreach (var quest in activeQuests)
        {
            if (names.Contains(quest.name)) { continue; }
            Quest currentQuest = GetQuest(quest.name);
            GameObject newQuest = Instantiate(questUIPrefab, questUIHolder);
            newQuest.GetComponent<QuestScript>().SetupButton(currentQuest, quest.elapsedTime);
            newQuest.GetComponent<QuestScript>().DisableButton();
        }

        foreach (var quest in availableQuests)
        {
            foreach (var var in names)
            {
                Debug.Log(var);
            }
            Debug.Log("questname Is " + quest);
            if (names.Contains(quest)) { continue; }
            Quest currentQuest = GetQuest(quest);
            GameObject newQuest = Instantiate(questUIPrefab, questUIHolder);
            newQuest.GetComponent<QuestScript>().SetupButton(currentQuest);
        }
    }

    Quest GetQuest(string name)
    {
        foreach (var quest in quests)
        {
            if (quest.name == name)
            {
                return quest;
            }
        }
        return null;
    }
}