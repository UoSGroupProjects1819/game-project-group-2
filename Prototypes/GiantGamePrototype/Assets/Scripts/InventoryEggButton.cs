using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEggButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    public InventoryScript IS;

    public InventoryScript.Egg thisEgg;

    public void SetUpButton(InventoryScript newIS, InventoryScript.Egg newEgg)
    {
        thisEgg = newEgg;
        childImage.sprite = thisEgg.sprite;
        childAmtCounter.text = "x" + thisEgg.amt;
    }

    public void CraftingClick()
    {
        IS.SetCraftingObject(thisEgg);
    }
}
