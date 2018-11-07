using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour {

    public GameObject creatureToSpawn;

    public float spawnDelay;
    float spriteChangeTimer = 0;
    int currentsprite = 0;

    public WorldSelector WS;

    public Sprite[] eggSprites;

    bool readyToSpawn;

    private void Start()
    {
        WS = GameObject.FindGameObjectWithTag("World").GetComponent<WorldSelector>();
    }

    void Update () {
        if (readyToSpawn)
        {
            spawnDelay -= Time.deltaTime;

            spriteChangeTimer -= Time.deltaTime;
            if(spriteChangeTimer <= 0 && currentsprite < eggSprites.Length)
            {
                spriteChangeTimer = spawnDelay / eggSprites.Length+1;
                this.GetComponentInChildren<SpriteRenderer>().sprite = eggSprites[currentsprite];
                currentsprite++;
            }

            if (spawnDelay <= 0)
            {
                Instantiate(creatureToSpawn, this.transform.position, Quaternion.identity, WS.SelectedIsland.transform);
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
