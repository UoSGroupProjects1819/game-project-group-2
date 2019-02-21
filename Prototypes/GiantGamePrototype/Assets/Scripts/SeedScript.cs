using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour {

    public string seedType;

    public GameObject treeToSpawn;

    public float spawnDelay;

    public WorldManager WM;

    public bool readyToSpawn;

    private void Start()
    {
        WM = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
    }

    void Update()
    {
        if (readyToSpawn)
        {
            spawnDelay -= Time.deltaTime;
            if (spawnDelay <= 0)
            {
                GameObject newTree = Instantiate(treeToSpawn, this.transform.position, Quaternion.identity, this.transform.parent);
                this.transform.parent.GetComponent<PlantPot>().treeInPot = newTree;
                Destroy(this.gameObject);
            }
        }

    }

    public void Dragging(Vector2 dragPos)
    {
        this.transform.position = new Vector3(dragPos.x, dragPos.y, 0);
    }

    public void ReleaseDrag(Vector3 dragPos, GameObject PotToSpawnIn)
    {
        if (PotToSpawnIn.GetComponent<PlantPot>() != null)
        {
            if (PotToSpawnIn.GetComponent<PlantPot>().treeInPot == null)
            {
                Debug.Log("Dropped Seed In Pot");
                InventoryManager.Instance.RemoveSeed(seedType);
                WorldManager.Instance.SelectedIsland.GetComponent<IslandScript>().currentTreePopulation++;
                InventoryManager.Instance.UpdateSeedButtons();
                PotToSpawnIn.GetComponent<PlantPot>().treeInPot = this.gameObject;
                this.transform.parent = PotToSpawnIn.transform;
                this.transform.localPosition = Vector2.zero;
                this.GetComponentInChildren<SpriteRenderer>().sortingOrder = PotToSpawnIn.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;
                readyToSpawn = true;
                InventoryManager.Instance.inventoryPanel.SetActive(true);
            }
            else
            {
                if (PotToSpawnIn.GetComponent<PlantPot>().potLevel < 2)
                {
                    Debug.Log("Dropped Seed In Pot for upgrade");
                    InventoryManager.Instance.RemoveSeed(seedType);
                    InventoryManager.Instance.UpdateSeedButtons();
                    PotToSpawnIn.GetComponent<PlantPot>().UpgradePot();
                    InventoryManager.Instance.inventoryPanel.SetActive(true);
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("Dropped seed but no pot");
                    InventoryManager.Instance.inventoryPanel.SetActive(true);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
