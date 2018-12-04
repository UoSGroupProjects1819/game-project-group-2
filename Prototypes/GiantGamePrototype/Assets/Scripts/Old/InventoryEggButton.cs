using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEggButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    public InventoryManager IM;

    public EggInInventory thisEgg;

    void Start()
    {
        
    }

    public void SetUpButton(EggInInventory newEgg)
    {
        IM = InventoryManager.Instance;
        thisEgg = newEgg;
        childImage.sprite = IM.FindEgg(thisEgg.name).sprite;
        childAmtCounter.text = "x" + thisEgg.amt;
        if (thisEgg.amt <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    /*public void CraftingClick()
    {
        IM.SetCraftingObject(thisEgg);
    }*/

    public void SpawnEgg()
    {
        GameObject Giant = GameObject.FindGameObjectWithTag("Giant");
    
        IslandScript island = WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentCreaturePopulation >= island.maxCreaturePopulation) { return; }
        Debug.Log("SpawningEgg");

        if (Giant.GetComponent<GiantScript>().currentHolding != null)
        {
            Destroy(Giant.GetComponent<GiantScript>().currentHolding);
            Giant.GetComponent<GiantScript>().currentHolding = null;
        }

        Giant.GetComponent<GiantScript>().SetCurrentHolding(IM.FindEgg(thisEgg.name).objectToSpawn, thisEgg.name);
        //IM.RemoveEgg(thisEgg);
        //IM.UpdateEggUI(this.transform.parent.gameObject);
        IM.inventoryPanel.SetActive(false);
        IM.HUDPanel.SetActive(true);
    }
}
