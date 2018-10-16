using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour {

    public GameObject treeToSpawn;

    public float spawnDelay;

    public IslandScript IS;

    bool readyToSpawn;

    void Update()
    {
        if (readyToSpawn)
        {
            spawnDelay -= Time.deltaTime;
            if (spawnDelay <= 0)
            {
                GameObject newTree = Instantiate(treeToSpawn, this.transform.position, Quaternion.identity);
                IS.AddPlant(newTree);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            readyToSpawn = true;
            IS = collision.gameObject.GetComponentInParent<IslandScript>();
        }
    }
}
