using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftScript : MonoBehaviour {

    public GameObject giftCanvas;

    public GameObject GiftImage;
    public GameObject ItemPreview;

    InventoryScript IS;

    int AmtGiven;
    int TypeGiven;

    private void Start()
    {
        IS = InventoryScript.Instance;
    }

    public void Touched()
    {
        AmtGiven = Random.Range(2, 4);
        TypeGiven = Random.Range(0, IS.seeds.Length);

        giftCanvas.SetActive(true);
        for (int i = 0; i < AmtGiven; i++)
        {
            IS.AddSeed(IS.seeds[TypeGiven].name);
        }

        IslandScript island = WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentCreaturePopulation < island.maxCreaturePopulation)
        {
            GameObject newEgg = Instantiate(IS.eggs[Random.Range(0, IS.eggs.Length)].objectToSpawn, this.transform.position, Quaternion.identity);
        }

        ItemPreview.GetComponent<Image>().sprite = IS.seeds[TypeGiven].sprite;
        ItemPreview.GetComponentInChildren<Text>().text = "x" + AmtGiven;

        this.GetComponent<Animator>().SetTrigger("Open");
    }

    public void TapGiftScreen()
    {
        GiftImage.SetActive(false);
        ItemPreview.SetActive(true);
    }

    public void TapGiftClose()
    {
        Destroy(this.gameObject);
    }

}
