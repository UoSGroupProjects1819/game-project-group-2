using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEggButton : MonoBehaviour {

    public Image childImage;
    public Text childAmtCounter;

    public InventoryScript IS;

    public EggInInventory thisEgg;

    void Start()
    {
        
    }

    public void SetUpButton(EggInInventory newEgg)
    {
        IS = InventoryScript.Instance;
        thisEgg = newEgg;
        childImage.sprite = IS.FindEgg(thisEgg.name).sprite;
        childAmtCounter.text = "x" + thisEgg.amt;
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
    
        IslandScript island = WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentCreaturePopulation >= island.maxCreaturePopulation) { return; }
        Debug.Log("SpawningEgg");

        if (Giant.GetComponent<GiantScript>().currentHolding != null)
        {
            Destroy(Giant.GetComponent<GiantScript>().currentHolding);
            Giant.GetComponent<GiantScript>().currentHolding = null;
        }

        Giant.GetComponent<GiantScript>().SetCurrentHolding(IS.FindEgg(thisEgg.name).objectToSpawn, thisEgg.name);
        //IS.RemoveEgg(thisEgg);
        //IS.UpdateEggUI(this.transform.parent.gameObject);
        IS.inventoryPanel.SetActive(false);
        IS.HUDPanel.SetActive(true);
    }
}
