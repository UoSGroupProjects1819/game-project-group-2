using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour {

    public Transform seedHolder;
    public GameObject currentSeed;
    bool holding;

	// Use this for initialization
	void Start ()
    {
		
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            holding = !holding;
        }
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (holding)
        {
            currentSeed.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentSeed.transform.position = seedHolder.position;
            currentSeed.transform.eulerAngles = seedHolder.eulerAngles;
        }
    }
}
