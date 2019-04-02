using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySeedButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    InventoryManager IM;

    public SeedInInventory thisSeed;

    private void Start()
    {
        IM = InventoryManager.Instance;

    }

    public void SetUpButton(SeedInInventory newSeed)
    {
        IM = InventoryManager.Instance;
        thisSeed = newSeed;
        childImage.sprite = IM.FindSeed(thisSeed.name).sprite;
        childAmtCounter.text = "x" + thisSeed.amt;

        if (thisSeed.amt <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    /*public void CraftingClick()
    {
        Debug.Log(thisSeed.name);
        IM.SetCraftingObject(thisSeed);
    }*/

    public void StartSeedDrag()
    {
        Debug.Log("SpawningNewSeed");
        IslandScript island = WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>();
        GameObject NewSeed = Instantiate(IM.FindSeed(thisSeed.name).objectToSpawn);
        TouchManager.Instance.seedBeingDragged = NewSeed;
        IM.inventoryPanel.SetActive(false);
    }

    public void SpawnSeed()
    {
        if(TutorialManager.Instance.TutorialActive && TutorialManager.Instance.waitingForSeed) { TutorialManager.Instance.tutorialStage++; }
        GameObject Giant = GameObject.FindGameObjectWithTag("Giant");

        if (Giant.GetComponent<GiantScript>().currentHolding != null)
        {
            Destroy(Giant.GetComponent<GiantScript>().currentHolding);
            Giant.GetComponent<GiantScript>().currentHolding = null;
        }

        IslandScript island = WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>();

        Giant.GetComponent<GiantScript>().SetCurrentHolding(IM.FindSeed(thisSeed.name).objectToSpawn, this.thisSeed.name);
        
        //IM.RemoveSeed(thisSeed);
        //IM.UpdateSeedUI(this.transform.parent.gameObject);
        IM.inventoryPanel.SetActive(false);
        IM.HUDPanel.SetActive(true);
        
    }
}
