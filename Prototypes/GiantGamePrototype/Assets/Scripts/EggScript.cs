using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour {

    public string eggType;

    public GameObject creatureToSpawn;

    public float spawnDelay;
    float spriteChangeTimer = 0;
    int currentsprite = 0;

    public WorldManager WM;

    public Sprite[] eggSprites;

    bool readyToSpawn;

    private void Start()
    {
        WM = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
    }

    void Update ()
    {

        if (readyToSpawn)
        {
            this.transform.eulerAngles = Vector3.zero;
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
                Instantiate(creatureToSpawn, new Vector3(transform.position.x, transform.position.y, CreatureManager.Instance.nextSpawnDepth), Quaternion.identity, WM.SelectedIsland.transform);
                CreatureManager.Instance.nextSpawnDepth += 0.01f;
                if (TutorialManager.Instance.TutorialActive && TutorialManager.Instance.waitingForEgg) { TutorialManager.Instance.tutorialStage++; TutorialManager.Instance.StartTutorialStage(TutorialManager.Instance.tutorialStage); }
                Destroy(this.gameObject);
            }
        }
	}

    public void SpeedUpHatch()
    {
        float speedupAmt = spawnDelay * 0.1f;
        spawnDelay -= speedupAmt;
        spriteChangeTimer -= speedupAmt;
        this.GetComponent<Animator>().SetTrigger("Tapped");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            readyToSpawn = true;
        }
    }
}
