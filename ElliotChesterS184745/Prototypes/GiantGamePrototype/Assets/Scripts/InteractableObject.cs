using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour {

    GameObject giant;

	// Use this for initialization
	void Start ()
    {
        giant = GameObject.FindGameObjectWithTag("Giant");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("click");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == this.GetComponent<BoxCollider2D>() || hit.collider == this.GetComponent<CircleCollider2D>() || hit.collider == this.GetComponent<PolygonCollider2D>())
            {
                giant.GetComponent<WalkToPoint>().MoveToPoint(hit.collider.gameObject);
            }
        }
    }
}
