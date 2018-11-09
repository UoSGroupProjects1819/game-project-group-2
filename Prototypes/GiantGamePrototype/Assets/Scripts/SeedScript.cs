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
                Instantiate(treeToSpawn, this.transform.position, Quaternion.identity, this.transform.parent);
                Destroy(this.gameObject);
            }
        }
    }
}
