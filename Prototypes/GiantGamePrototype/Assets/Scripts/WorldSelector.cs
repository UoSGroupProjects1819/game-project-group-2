using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelector : MonoBehaviour {

    public GameObject[] islands;

    public GameObject SelectedIsland;

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

        SelectedIsland = islands[id];
        
        islands[id].SetActive(true);
    }
}
