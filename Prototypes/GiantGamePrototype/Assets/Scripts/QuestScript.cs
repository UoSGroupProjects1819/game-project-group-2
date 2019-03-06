using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestScript : MonoBehaviour
{

    public Image questSprite;
    public TextMeshProUGUI questText;
    public GameObject clickTrap;
    public GameObject timer;

    public TextMeshProUGUI questLevel1;
    public TextMeshProUGUI questLevel2;
    public TextMeshProUGUI questLevel3;

    Quest thisQuest;

    float questTime;
    float timeLeft;

    bool active;

    public void Update()
    {
        if (active)
        {
            timer.GetComponent<TextMeshProUGUI>().text = GetTimeFromSeconds(timeLeft);
            timeLeft -= Time.deltaTime;
        }   
    }

    string GetTimeFromSeconds(float seconds)
    {
        int h = 0;
        int m = 0;
        int s = 0;

        while (seconds > 3600)
        {
            seconds -= 3600;
            h++;
        }

        while (seconds > 60)
        {
            seconds -= 3600;
            h++;
        }

        s = (int)seconds;

        if (h != 0)
        {
            return h.ToString() + ":" + m.ToString() + "." + s.ToString();
        }

        if (m != 0)
        {
            return m.ToString() + "." + s.ToString();
        }

        return s.ToString() + "s";
    }

    public void SetupButton(Quest quest)
    {
        questSprite.sprite = quest.questSprite;
        questText.text = quest.name;
        questTime = quest.timeRequired;
        timeLeft = quest.timeRequired;
        if (quest.minLevels.Length > 0)
        {
            questLevel1.text = quest.minLevels[0].name + ": " + quest.minLevels[0].amt;

            if (quest.minLevels.Length > 1)
            {
                questLevel2.text = quest.minLevels[1].name + ": " + quest.minLevels[1].amt;

                if (quest.minLevels.Length > 2)
                {
                    questLevel3.text = quest.minLevels[2].name + ": " + quest.minLevels[2].amt;
                }
            }
        }
    }

    public void SetupButton(Quest quest, float elapsed)
    {
        questSprite.sprite = quest.questSprite;
        questText.text = quest.name;
        questTime = quest.timeRequired;
        timeLeft = elapsed;
        if (quest.minLevels.Length > 0)
        {
            questLevel1.text = quest.minLevels[0].name + ": " + quest.minLevels[0].amt;

            if (quest.minLevels.Length > 1)
            {
                questLevel2.text = quest.minLevels[1].name + ": " + quest.minLevels[1].amt;

                if (quest.minLevels.Length > 2)
                {
                    questLevel3.text = quest.minLevels[2].name + ": " + quest.minLevels[2].amt;
                }
            }
        }
    }

    public void Pressed()
    {
        DisableButton();
        ActiveQuest newQuest = new ActiveQuest { name = questText.text, elapsedTime = questTime };
        QuestManager.instance.activeQuests.Add(newQuest);
        QuestManager.instance.availableQuests.Remove(questText.text);
    }

    public void DisableButton()
    {
        active = true;
        clickTrap.SetActive(true);
        timer.SetActive(true);
        this.GetComponent<Button>().interactable = false;
    }

}
