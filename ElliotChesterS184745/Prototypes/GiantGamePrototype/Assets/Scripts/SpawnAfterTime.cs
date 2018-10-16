using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterTime : MonoBehaviour {

    public GameObject objectToSpawn;

    public float spawnAfter;

    public bool StartCountdown;

    float countdownTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (StartCountdown)
        {
            countdownTime += Time.deltaTime;
            if(countdownTime >= spawnAfter)
            {
                Instantiate(objectToSpawn, transform.position, transform.rotation);

                Destroy(this.gameObject);
            }
        }
	}
}
