using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour {

    public GameObject treeToSpawn;

    public float spawnDelay;

    public WorldSelector WS;

    bool readyToSpawn;

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
                Instantiate(treeToSpawn, this.transform.position, Quaternion.identity, WS.SelectedIsland.transform);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            readyToSpawn = true;
        }
    }
}
