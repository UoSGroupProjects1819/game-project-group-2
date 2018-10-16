using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour {

    public GameObject creatureToSpawn;

    public float spawnDelay;

    public IslandScript IS;

    bool readyToSpawn;

	void Update () {
        if (readyToSpawn)
        {
            spawnDelay -= Time.deltaTime;
            if(spawnDelay <= 0)
            {
                GameObject newCreature = Instantiate(creatureToSpawn, this.transform.position, Quaternion.identity);
                IS.AddCreature(newCreature);
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
