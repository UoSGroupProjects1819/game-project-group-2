using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour {

    public bool readyToDrop;
    InventoryScript.Fruit thisFruit;

    public string statToIncrease;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Creature")
        {
            collision.gameObject.GetComponent<CreatureScript>().IncreaseHappiness(10, thisFruit.name, statToIncrease);
            Destroy(this.gameObject);
        }
    }
}
