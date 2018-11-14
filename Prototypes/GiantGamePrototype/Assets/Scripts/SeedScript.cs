using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour {

    public string seedType;

    public GameObject treeToSpawn;

    public float spawnDelay;

    public WorldSelector WS;

    public bool readyToSpawn;

    private void Start()
    {
        WS = GameObject.FindGameObjectWithTag("World").GetComponent<WorldSelector>();
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
        if (PotToSpawnIn.GetComponent<PlantPot>() != null && PotToSpawnIn.GetComponent<PlantPot>().treeInPot == null)
        {
            Debug.Log("Dropped Seed In Pot");
            InventoryScript.Instance.RemoveSeed(seedType);
            WorldSelector.Instance.SelectedIsland.GetComponent<IslandScript>().currentTreePopulation++;
            InventoryScript.Instance.UpdateSeedButtons();
            this.transform.parent = PotToSpawnIn.transform;
            this.transform.localPosition = Vector2.zero;
            this.GetComponentInChildren<SpriteRenderer>().sortingOrder = PotToSpawnIn.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;
            readyToSpawn = true;
            InventoryScript.Instance.inventoryPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Dropped seed but no pot");
            InventoryScript.Instance.inventoryPanel.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
