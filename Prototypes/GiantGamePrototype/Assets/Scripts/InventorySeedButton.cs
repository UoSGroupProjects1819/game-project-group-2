using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySeedButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    public InventoryScript IS;

    public InventoryScript.Seed thisSeed;

    public void SetUpButton(InventoryScript newIS, InventoryScript.Seed newSeed)
    {
        thisSeed = newSeed;
        childImage.sprite = thisSeed.sprite;
        childAmtCounter.text = "x" + thisSeed.amt;
    }

    public void CraftingClick()
    {
        IS.SetCraftingObject(thisSeed);
    }
}
