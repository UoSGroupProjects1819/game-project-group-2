using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour {

    public bool readyToDrop;
    public Fruit thisFruit;

    public string majorStatToIncrease;
    public string minorStatToIncrease;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Creature" && collision.gameObject.GetComponent<CreatureScript>().targetFruit == this.gameObject)
        {
            collision.gameObject.GetComponent<CreatureScript>().EatFruit(10, thisFruit.name, majorStatToIncrease, minorStatToIncrease);
            collision.gameObject.GetComponent<CreatureScript>().targetFruit = null;
            Destroy(this.gameObject);
        }
    }
}
