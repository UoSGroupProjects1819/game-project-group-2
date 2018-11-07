using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour {

    public bool readyToDrop;
    public Fruit thisFruit;

    public string statToIncrease;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Creature" && collision.gameObject.GetComponent<CreatureScript>().targetFruit == this.gameObject)
        {
            collision.gameObject.GetComponent<CreatureScript>().IncreaseHappiness(10, thisFruit.name, statToIncrease);
            collision.gameObject.GetComponent<CreatureScript>().targetFruit = null;
            Destroy(this.gameObject);
        }
    }
}
