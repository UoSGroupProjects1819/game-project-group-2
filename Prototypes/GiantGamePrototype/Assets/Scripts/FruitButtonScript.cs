using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitButtonScript : MonoBehaviour
{
    public Image childImage;
    public Text childAmtCounter;

    InventoryManager IM;

    public string fruitType;

    private void Start()
    {
        IM = InventoryManager.Instance;
    }

    public void StartFruitDrag()
    {
        if(IM.FindFruitInInventory(fruitType, WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().islandID).amt <= 0) { return; }
        Debug.Log("SpawningNewFruit");


        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

        GameObject newFruit = Instantiate(IM.FindFruit(fruitType).objectToSpawn, hit.point, Quaternion.identity);
        newFruit.GetComponent<FruitScript>().targetCreature = StatManager.Instance.targetCreature;

        StatManager.Instance.targetCreature.GetComponent<CreatureScript>().targetFruit = newFruit;
        StatManager.Instance.targetCreature.GetComponent<CreatureScript>().targetFruitSpawnPos = (Vector2)newFruit.transform.position;
        TouchController.Instance.fruitBeingDragged = newFruit;
    }
}
