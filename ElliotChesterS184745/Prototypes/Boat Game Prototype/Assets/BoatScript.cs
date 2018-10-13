using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour {

    Rigidbody RB;

    
    public float speed;
    public float turnPower;
    public float turnSpeed;

    float TargetRot;

    public bool returnToZeroDegrees;

	// Use this for initialization
	void Start ()
    {
        RB = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RB.velocity = transform.rotation * Vector3.forward * speed;

        while(TargetRot < 0)
        {
            TargetRot += 360;
        }

        this.transform.eulerAngles = new Vector3(0, Mathf.MoveTowardsAngle(this.transform.eulerAngles.y, TargetRot, turnSpeed * Time.deltaTime), 0);

        if (returnToZeroDegrees)
        {
            if (TargetRot - this.transform.eulerAngles.y < 1 && TargetRot - this.transform.eulerAngles.y > -1)
            {
                TargetRot = 0;
            }
        }
	}

    Vector3 LerpEulerAngle(Vector3 old, Vector3 target, float value)
    {
        float newX = Mathf.LerpAngle(old.x, target.x, value);
        float newY = Mathf.LerpAngle(old.y, target.y, value);
        float newZ = Mathf.LerpAngle(old.z, target.z, value);

        return new Vector3(newX, newY, newZ);
    }

    public void TurnLeft()
    {
        TargetRot = this.transform.eulerAngles.y + turnPower;
    }

    public void TurnRight()
    {
        TargetRot = this.transform.eulerAngles.y - turnPower;
    }
}
