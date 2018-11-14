using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour {

    public GameObject giftCanvas;

    InventoryScript IS;

    private void Start()
    {
        IS = InventoryScript.Instance;
    }

    public void Touched()
    {
        giftCanvas.SetActive(true);
        for (int i = 0; i < Random.Range(2,4); i++)
        {
            IS.AddSeed(IS.seeds[Random.Range(0, IS.seeds.Length)].name);
        }

        IslandScript island = WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentCreaturePopulation < island.maxCreaturePopulation)
        {
            GameObject newEgg = Instantiate(IS.eggs[Random.Range(0, IS.eggs.Length)].objectToSpawn, this.transform.position, Quaternion.identity);
        }
    }

    public void TapGiftScreen()
    {
        Destroy(this.gameObject);
    }

}
