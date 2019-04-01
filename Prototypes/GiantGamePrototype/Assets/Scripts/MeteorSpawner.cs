using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

    public static MeteorSpawner instance;

    public float minSpawnTime;
    public float maxSpawnTime;

    public bool canSpawn = false;

    float spawnTimer;

    public GameObject meteorPrefab;

    public Vector3 offsetPos;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
	}
	
	void Update () {
        if (!canSpawn) { return; }
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            SpawnMeteor();
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        if(this.transform.position != WorldManager.Instance.SelectedIsland.transform.position + offsetPos)
        {
            this.transform.position = WorldManager.Instance.SelectedIsland.transform.position + offsetPos;
        }
	}

    public void SpawnMeteor()
    {
        Instantiate(meteorPrefab, this.transform.position, Quaternion.identity);
    }

    public void SpawnMeteor(out GameObject MeteorSpawned)
    {
        MeteorSpawned = Instantiate(meteorPrefab, this.transform.position, Quaternion.identity);
    }
}
