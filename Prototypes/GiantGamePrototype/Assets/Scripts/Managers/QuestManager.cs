using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public Sprite questSprite;
    public string minLevel;
    public float timeRequired;
    public string[] rewards;
}

[System.Serializable]
public class ActiveQuest
{
    public string name;
    public float elapsedTime;
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
            if(quest.elapsedTime < 0)
            {
                activeQuests.Remove(quest);
                UpdateQuests();
            }
        }
    }

    private void UpdateQuests()
    {
        foreach (Transform quest in questUIHolder)
        {
            Destroy(quest.gameObject);
        }

        foreach (var quest in activeQuests)
        {
            Quest currentQuest = GetQuest(quest.name);
            GameObject newQuest = Instantiate(questUIPrefab, questUIHolder);
            newQuest.GetComponent<QuestScript>().SetupButton(currentQuest.name, currentQuest.questSprite, currentQuest.timeRequired);
            newQuest.GetComponent<QuestScript>().DisableButton();
        }

        foreach (var quest in availableQuests)
        {
            Quest currentQuest = GetQuest(quest);
            GameObject newQuest = Instantiate(questUIPrefab, questUIHolder);
            newQuest.GetComponent<QuestScript>().SetupButton(currentQuest.name, currentQuest.questSprite, currentQuest.timeRequired);
        }
    }

    Quest GetQuest(string name)
    {
        foreach (var quest in quests)
        {
            if(quest.name == name)
            {
                return quest;
            }
        }
        return null;
    }
}
