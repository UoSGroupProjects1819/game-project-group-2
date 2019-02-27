using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject[] chests;

    void ClickChest(GameObject chest)
    {
        foreach (var item in chests)
        {
            if(item != chest)
            {
                item.SetActive(false);
            }
        }

        chest.GetComponent<Animator>().SetTrigger("Open");
    }
}
