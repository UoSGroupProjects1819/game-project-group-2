using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToPoint : MonoBehaviour {

    Vector3 targetPoint;
    public float speed;

    GameObject target;

	// Use this for initialization
	void Start () {
        targetPoint = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetPoint != this.transform.position && targetPoint != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPoint, speed * Time.deltaTime);

            Camera.main.GetComponent<CameraPositions>().Dynamic =   this.transform.position + new Vector3(0,1,-10);
            Camera.main.GetComponent<CameraPositions>().DynamicOrtho = 3;
            Camera.main.GetComponent<CameraPositions>().useDynamic = true;
        }
        else
        {
            Camera.main.GetComponent<CameraPositions>().useDynamic = false;
        }
    }

    public void MoveToPoint(GameObject point)
    {
        targetPoint = point.transform.position;
        target = point;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable" && collision.gameObject == target)
        {
            targetPoint = this.transform.position;
        }
    }
}
