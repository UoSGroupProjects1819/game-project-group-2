using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitButton : MonoBehaviour
{

    public Image childImage;
    public Text childAmtCounter;

    InventoryScript.Fruit thisFruit;

    InventoryScript IS;

    CreatureScript creatureToFeed;

    public void SetUpButton(InventoryScript newIS, InventoryScript.Fruit newFruit, CreatureScript creature)
    {
        IS = newIS;
        thisFruit = newFruit;
        childImage.sprite = thisFruit.sprite;
        childAmtCounter.text = "x" + thisFruit.amt;
        creatureToFeed = creature;

        if(thisFruit.amt <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void FeedClick()
    {
        creatureToFeed.IncreaseHappiness(10, thisFruit.name);
        IS.RemoveFruit(thisFruit);
        IS.UpdateFruitUI(IS.FruitUI, creatureToFeed);
        IS.fruitPanel.SetActive(false);
        IS.HUDPanel.SetActive(true);
    }

}
