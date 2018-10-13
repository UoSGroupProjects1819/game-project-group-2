using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        this.transform.parent = this.transform.parent.parent;
	}
}
