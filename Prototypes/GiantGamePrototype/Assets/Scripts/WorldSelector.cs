using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelector : MonoBehaviour {

    public static WorldSelector Instance;

    public GameObject[] islands;

    public GameObject SelectedIsland;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < islands.Length; i++)
        {
            islands[i].GetComponent<IslandScript>().islandID = i;
        }
    }

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
