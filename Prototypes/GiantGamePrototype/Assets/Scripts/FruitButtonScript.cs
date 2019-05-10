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
        Debug.Log(IM.FindFruit(fruitType));
        GameObject newFruit = Instantiate(IM.FindFruit(fruitType).objectToSpawn, hit.point, Quaternion.identity);
        newFruit.GetComponent<FruitScript>().targetCreature = StatManager.Instance.targetCreature;

        StatManager.Instance.targetCreature.GetComponent<CreatureScript>().targetFruit = newFruit;
        StatManager.Instance.targetCreature.GetComponent<CreatureScript>().targetFruitSpawnPos = (Vector2)newFruit.transform.position;
        TouchManager.Instance.fruitBeingDragged = newFruit;
    }

    public void TapFruit()
    {
        if (IM.FindFruitInInventory(fruitType, WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().islandID).amt <= 0) { return; }
        if (StatManager.Instance.targetCreature.GetComponent<CreatureScript>().eating) { return; }

        GameObject newFruit = Instantiate(IM.FindFruit(fruitType).objectToSpawn, new Vector3(0,0,-10f), Quaternion.identity);
        newFruit.GetComponent<FruitScript>().targetCreature = StatManager.Instance.targetCreature;
        newFruit.GetComponentInChildren<SpriteRenderer>().sortingOrder = StatManager.Instance.targetCreature.GetComponentInChildren<SpriteRenderer>().sortingOrder + 2;
        newFruit.GetComponentInChildren<SpriteRenderer>().sortingLayerID = StatManager.Instance.targetCreature.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
        newFruit.GetComponent<FruitScript>().ReleaseDrag((Vector2)StatManager.Instance.targetCreature.transform.position, StatManager.Instance.targetCreature);
    }
}
