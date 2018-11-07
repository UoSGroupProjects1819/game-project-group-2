using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    public Vector2 StartPoint;
    public Vector2 EndPoint;
    public float speed;
    float randomRotation;

    public GameObject GiftToSpawn;

    public bool canTap;

    void Start () {
        this.transform.position = StartPoint;
        randomRotation = Random.Range(-90, 90);
        this.GetComponent<Rigidbody2D>().velocity = (EndPoint - (Vector2)this.transform.position).normalized * speed;
        this.GetComponent<Rigidbody2D>().angularVelocity = randomRotation;

    }

    void Update ()
    {

        if ((Vector2)this.transform.position == EndPoint)
        {
            Destroy(this.gameObject);
            //this.transform.position = StartPoint;
        }
	}

    public void Tapped()
    {
        if(!canTap) { return; }
        this.GetComponent<TrailRenderer>().enabled = false;
        Instantiate(GiftToSpawn, this.transform.position, Quaternion.identity);
        this.GetComponent<Rigidbody2D>().angularVelocity = 0;

        foreach (Transform piece in this.transform)
        {
            piece.GetComponent<Rigidbody2D>().simulated = true;
            piece.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity + new Vector2(Random.Range(-1,1f), Random.Range(-1, 1));
            piece.GetComponent<Rigidbody2D>().drag = Random.Range(0.2f, 0.5f);
            piece.GetComponent<TrailRenderer>().enabled = true;
            piece.GetComponent<Animator>().SetTrigger("Break");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if(collision.gameObject.tag == "TappableArea")
        {
            canTap = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TappableArea")
        {
            canTap = false;
        }
    }
}
