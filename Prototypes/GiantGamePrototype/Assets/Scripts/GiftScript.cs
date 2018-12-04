using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftScript : MonoBehaviour {

    public GameObject giftCanvas;

    public GameObject GiftImage;
    public GameObject ItemPreview;

    InventoryManager IM;

    int AmtGiven;
    int TypeGiven;

    private void Start()
    {
        IM = InventoryManager.Instance;
    }

    public void Touched()
    {
        AmtGiven = Random.Range(2, 4);
        TypeGiven = Random.Range(0, IM.seeds.Length);

        giftCanvas.SetActive(true);
        for (int i = 0; i < AmtGiven; i++)
        {
            IM.AddSeed(IM.seeds[TypeGiven].name);
        }

        IslandScript island = WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>();
        if (island.currentCreaturePopulation < island.maxCreaturePopulation)
        {
            GameObject newEgg = Instantiate(IM.eggs[Random.Range(0, IM.eggs.Length)].objectToSpawn, this.transform.position, Quaternion.identity);
        }

        ItemPreview.GetComponent<Image>().sprite = IM.seeds[TypeGiven].sprite;
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
