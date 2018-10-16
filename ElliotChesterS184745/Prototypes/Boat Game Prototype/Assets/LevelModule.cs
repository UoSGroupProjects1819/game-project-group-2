using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModule : MonoBehaviour {

    GameManager gameManager;
    public Transform spawnPoint;
    GameObject waterPrefab;

    public GameObject[] oneSpawners;
    public GameObject[] twoSpawners;

    public GameObject[] Obstacles;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        waterPrefab = gameManager.waterPrefab;

        Debug.Log("bang");
        int firstRandomInt = Random.Range(0, oneSpawners.Length * 1);
        int secondRandomInt = Random.Range(0, twoSpawners.Length * 10);
        int obstacleRandomInttwo = Random.Range(0, Obstacles.Length-1);
        int obstacleRandomIntone = Random.Range(0, Obstacles.Length - 1);
        Instantiate(Obstacles[obstacleRandomInttwo], oneSpawners[firstRandomInt].transform.position, oneSpawners[firstRandomInt].transform.rotation, oneSpawners[firstRandomInt].transform);
        Instantiate(Obstacles[obstacleRandomIntone], twoSpawners[secondRandomInt].transform.position, twoSpawners[secondRandomInt].transform.rotation, twoSpawners[secondRandomInt].transform);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boat")
        {
            Instantiate(waterPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boat")
        {
            Destroy(this.gameObject);
        }
    }
}
