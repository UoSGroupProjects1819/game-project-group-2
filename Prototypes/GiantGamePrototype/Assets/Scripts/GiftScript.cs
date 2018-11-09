using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour {

    public GameObject giftCanvas;

    public void Touched()
    {
        giftCanvas.SetActive(true);
    }

    public void TapGiftScreen()
    {
        Destroy(this.gameObject);
    }

}
