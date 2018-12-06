using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update IM called once per frame
	void Update () {
        this.transform.eulerAngles += new Vector3(0,0, (360 / -speed) * Time.deltaTime);
	}
}
