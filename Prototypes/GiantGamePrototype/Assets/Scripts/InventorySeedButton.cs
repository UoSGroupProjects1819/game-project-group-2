using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySeedButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    InventoryScript IS;

    public SeedInInventory thisSeed;

    private void Start()
    {

    }

    public void SetUpButton(SeedInInventory newSeed)
    {
        IS = InventoryScript.Instance;

        thisSeed = newSeed;
        childImage.sprite = IS.FindSeed(thisSeed.name).sprite;
        childAmtCounter.text = "x" + thisSeed.amt;

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

        if (Giant.GetComponent<GiantScript>().currentHolding != null)
        {
            Destroy(Giant.GetComponent<GiantScript>().currentHolding);
            Giant.GetComponent<GiantScript>().currentHolding = null;
        }

        IslandScript island = WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentTreePopulation >= island.maxTreePopulation) { return; }

        Giant.GetComponent<GiantScript>().SetCurrentHolding(IS.FindSeed(thisSeed.name).objectToSpawn, this.thisSeed.name);
        
        //IS.RemoveSeed(thisSeed);
        //IS.UpdateSeedUI(this.transform.parent.gameObject);
        IS.inventoryPanel.SetActive(false);
        IS.HUDPanel.SetActive(true);
        
    }
}
