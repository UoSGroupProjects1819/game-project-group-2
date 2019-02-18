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

    float questTime;

    public void SetupButton(string name, Sprite sprite, float time)
    {
        questSprite.sprite = sprite;
        questText.text = name;
        questTime = time;
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
        clickTrap.SetActive(true);
        this.GetComponent<Button>().interactable = false;
    }

}
