using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float RPM = 10;

	void Update () {
        this.transform.eulerAngles += new Vector3(0, 0, (360 / 60) * RPM * Time.deltaTime);
	}
}
