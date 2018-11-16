using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitButtonScript : MonoBehaviour
{

    

    public void StartFruitDrag()
    {
        Debug.Log("SpawningNewSeed");
        IslandScript island = WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentTreePopulation >= island.maxTreePopulation) { return; }
        //GameObject NewSeed = Instantiate(IS.FindSeed(thisSeed.name).objectToSpawn);
        TouchController.Instance.seedBeingDragged = NewSeed;
    }
}
