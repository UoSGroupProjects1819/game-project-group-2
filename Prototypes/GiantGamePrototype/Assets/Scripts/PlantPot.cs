using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPot : MonoBehaviour {

    public int potID;

    public int potLevel;

    public GameObject treeInPot;

    public Sprite[] PotLevelSprites;

    void Start()
    {
        this.GetComponentInChildren<SpriteRenderer>().sprite = PotLevelSprites[potLevel];
    }

    public void CheckSprite()
    {
        if (this.GetComponentInChildren<SpriteRenderer>().sprite != PotLevelSprites[potLevel])
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = PotLevelSprites[potLevel];
        }
    }

    public void UpgradePot()
    {
        potLevel++;
        this.GetComponentInChildren<SpriteRenderer>().sprite = PotLevelSprites[potLevel];
    }
}
