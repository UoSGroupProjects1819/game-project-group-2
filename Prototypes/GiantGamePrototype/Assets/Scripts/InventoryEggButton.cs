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
        IS = newIS;
        if (thisEgg.amt <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void CraftingClick()
    {
        IS.SetCraftingObject(thisEgg);
    }

    public void SpawnEgg()
    {
        GameObject Giant = GameObject.FindGameObjectWithTag("Giant");

        if (Giant.GetComponent<GiantScript>().currentHolding == null)
        {
            Giant.GetComponent<GiantScript>().SetCurrentHolding(thisEgg.objectToSpawn);
            thisEgg.amt -= 1;
            IS.RemoveEgg(thisEgg);
            IS.UpdateEggUI(this.transform.parent.gameObject);
            IS.gameObject.SetActive(false);
            IS.HUD.SetActive(true);
        }
    }
}
