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
        if (islands[id] == SelectedIsland) { return; }

        Camera.main.GetComponent<CameraControl>().SetWorldOffset(islands[id].transform.position.x);
        GiantScript.Instance.gameObject.transform.position += islands[id].transform.position - SelectedIsland.transform.position;
        GiantScript.Instance.targetPoint = GiantScript.Instance.transform.position;

        SelectedIsland = islands[id];
       
    }
}
