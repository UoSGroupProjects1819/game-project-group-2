using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestButton : MonoBehaviour
{
    public Image chestSprite;

    public TextMeshProUGUI counter;

    string chestName;

    Chest thisChest;

    public void SetUpChest(Chest chest, int amt)
    {
        chestSprite.sprite = chest.sprite;
        counter.text = amt.ToString();
        chestName = chest.name;
        thisChest = chest;
    }

    public void ButtonPress()
    {
        GameObject chestOpenScreen = ChestScript.instance.gameObject;
        chestOpenScreen.SetActive(true);

        GameObject chestInventoryScreen = GameObject.FindGameObjectWithTag("ChestInventoryScreen");
        chestInventoryScreen.SetActive(false);

        chestOpenScreen.GetComponent<ChestScript>().SetUpScreen(thisChest);
    }
}
