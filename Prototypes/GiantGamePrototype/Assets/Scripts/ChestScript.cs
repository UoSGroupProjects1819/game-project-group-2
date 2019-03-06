using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestScript : MonoBehaviour
{
    public static ChestScript instance;

    public GameObject[] chests;

    public Chest currentChest;

    public GameObject chestInventory;

    enum Stages
    {
        SelectScreen,
        OpenScreen,
        ShowScreen
    }

    Stages currentStage;
        
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void SetUpScreen(Chest newChest)
    {
        currentChest = newChest;
        foreach (var chest in chests)
        {
            chest.GetComponent<Image>().sprite = newChest.sprite;
            chest.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ClickChest(GameObject chest)
    {
        switch (currentStage)
        {
            case Stages.SelectScreen:
                foreach (var item in chests)
                {
                    if (item != chest)
                    {
                        item.SetActive(false);
                    }
                }
                currentStage = Stages.OpenScreen;
                break;
            case Stages.OpenScreen:
                ChestItem newReward = PickRandomItem();
                if(InventoryManager.Instance.FindSeed(newReward.name) != null)
                {
                    for (int i = 0; i < newReward.amount; i++)
                    {
                        InventoryManager.Instance.AddSeed(newReward.name);
                    }
                    chest.GetComponent<Image>().sprite = InventoryManager.Instance.FindSeed(newReward.name).sprite;
                }

                if (InventoryManager.Instance.FindFruit(newReward.name) != null)
                {
                    for (int i = 0; i < newReward.amount; i++)
                    {
                        InventoryManager.Instance.AddFruit(newReward.name, WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().islandID);
                    }
                    chest.GetComponent<Image>().sprite = InventoryManager.Instance.FindFruit(newReward.name).sprite;
                }

                if (newReward.amount > 1)
                {
                    chest.transform.GetChild(0).gameObject.SetActive(true);
                    chest.GetComponentInChildren<TextMeshProUGUI>().text = newReward.amount.ToString();
                }

                InventoryManager.Instance.RemoveChest(currentChest.name);
                currentStage = Stages.ShowScreen;
                break;
            case Stages.ShowScreen:
                foreach (var item in chests)
                {
                    item.SetActive(true);
                }
                currentStage = Stages.SelectScreen;
                chestInventory.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    ChestItem PickRandomItem()
    {
        float totalChance = 0;
        foreach (var item in currentChest.PossibleItems)
        {
            totalChance += item.weight;
        }

        float randomNumber = Random.Range(0, totalChance);

        foreach (var item in currentChest.PossibleItems)
        {
            randomNumber -= item.weight;
            if(randomNumber <= 0)
            {
                return item;
            }
        }

        return null;
    }

}
