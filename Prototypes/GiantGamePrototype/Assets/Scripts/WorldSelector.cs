using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelector : MonoBehaviour {

    public GameObject[] islands;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectIsland(0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SelectIsland(1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectIsland(2);
        }
    }

    public void SelectIsland(int id)
    {
        if(islands.Length < id + 1) { return; }

        foreach (var item in islands)
        {
            item.SetActive(false);
        }

        islands[id].SetActive(true);
    }
}
