using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour {

    public Transform seedHolder;
    public GameObject currentHolding;
    bool holding = false;

    public Animator anim;

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

        anim.SetFloat("Speed", this.GetComponent<Rigidbody2D>().velocity.magnitude);
    }

    public void SetCurrentHolding(GameObject newHolding)
    {
        Destroy(currentHolding);
        currentHolding = Instantiate(newHolding, seedHolder.position, seedHolder.rotation);
        holding = true;
    }

}
