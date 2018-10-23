using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour {

    public Transform seedHolder;
    public GameObject currentHolding;
    bool holding = false;

    public Animator anim;

    public GameObject holdingButton;

    void LateUpdate ()
    {
        if (holding)
        {
            currentHolding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentHolding.transform.position = seedHolder.position;
            currentHolding.transform.eulerAngles = seedHolder.eulerAngles;

            holdingButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropHeldItem();
            }
        }
        else
        {
            holdingButton.SetActive(false);
        }

        anim.SetFloat("Speed", this.GetComponent<Rigidbody2D>().velocity.magnitude);
    }

    public void SetCurrentHolding(GameObject newHolding)
    {
        Destroy(currentHolding);
        currentHolding = Instantiate(newHolding, seedHolder.position, seedHolder.rotation);
        holding = true;
    }

    public void DropHeldItem()
    {
        holding = false;
        currentHolding = null;
    }

}
