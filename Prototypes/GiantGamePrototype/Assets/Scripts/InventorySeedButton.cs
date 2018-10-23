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

        IS = newIS;

        if (thisSeed.amt <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void CraftingClick()
    {
        IS.SetCraftingObject(thisSeed);
    }

    public void SpawnSeed()
    {
        GameObject Giant = GameObject.FindGameObjectWithTag("Giant");
        if (Giant.GetComponent<GiantScript>().currentHolding == null)
        {
            Giant.GetComponent<GiantScript>().SetCurrentHolding(thisSeed.objectToSpawn);
            thisSeed.amt -= 1;

            IS.RemoveSeed(thisSeed);
            IS.UpdateSeedUI(this.transform.parent.gameObject);
            IS.gameObject.SetActive(false);
            IS.HUD.SetActive(true);
        }
    }
}
