using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitButtonScript : MonoBehaviour
{
    public Image childImage;
    public Text childAmtCounter;

    InventoryScript IS;

    public string fruitType;

    private void Start()
    {
        IS = InventoryScript.Instance;
    }

    public void StartFruitDrag()
    {
        if(IS.FindFruitInInventory(fruitType, WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>().islandID).amt <= 0) { return; }
        Debug.Log("SpawningNewFruit");
        GameObject newFruit = Instantiate(IS.FindFruit(fruitType).objectToSpawn);
        newFruit.GetComponent<FruitScript>().targetCreature = StatManager.Instance.targetCreature;
        TouchController.Instance.fruitBeingDragged = newFruit;
    }
}
