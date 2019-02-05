using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

    public float minSpawnTime;
    public float maxSpawnTime;

    float spawnTimer;

    public GameObject meteorPrefab;

    public Vector3 offsetPos;

	void Start () {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
	}
	
	void Update () {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            Instantiate(meteorPrefab, this.transform.position, Quaternion.identity);
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        if(this.transform.position != WorldManager.Instance.SelectedIsland.transform.position + offsetPos)
        {
            this.transform.position = WorldManager.Instance.SelectedIsland.transform.position + offsetPos;
        }
	}
}
