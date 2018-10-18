using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour {

    public Transform seedHolder;
    public GameObject currentHolding;
    bool holding = false;

	// Use this for initialization
	void Start ()
    {
		
	}

    private void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (holding)
        {
            currentHolding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentHolding.transform.position = seedHolder.position;
            currentHolding.transform.eulerAngles = seedHolder.eulerAngles;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                holding = false;
                currentHolding = null;
            }
        }
    }

    public void SetCurrentHolding(GameObject newHolding)
    {
        Destroy(currentHolding);
        currentHolding = Instantiate(newHolding, seedHolder.position, seedHolder.rotation);
        holding = true;
    }
}
