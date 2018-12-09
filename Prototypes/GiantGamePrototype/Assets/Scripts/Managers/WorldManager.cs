using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public static WorldManager Instance;

    public bool selecting;

    public GameObject[] islands;

    public GameObject SelectedIsland;

    CameraControl CC;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < islands.Length; i++)
        {
            islands[i].GetComponent<IslandScript>().islandID = i;
        }
    }

    private void Start()
    {
        CC = Camera.main.GetComponent<CameraControl>();
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
        //if (islands[id] == SelectedIsland) { return; }

        Camera.main.GetComponent<CameraControl>().SetWorldOffset(islands[id].transform.position.x);
        GiantScript.Instance.gameObject.transform.position += islands[id].transform.position - SelectedIsland.transform.position;
        GiantScript.Instance.targetPoint = GiantScript.Instance.transform.position;

        SelectedIsland = islands[id];
        //Camera.main.orthographicSize = CC.maxCameraOrtho;
        CC.currentCameraPosition = CameraControl.CameraPositions.TouchControl;
        selecting = false;

    }

    public void ResetCurrentIsland()
    {
        List<GameObject> creaturesToRemove = new List<GameObject>();

        foreach (var item in CreatureManager.Instance.creaturesInWorld)
        {
            if(item.GetComponentInParent<IslandScript>() == SelectedIsland.GetComponent<IslandScript>())
            {
                creaturesToRemove.Add(item);
            }
        }

        foreach (var item in creaturesToRemove)
        {
            CreatureManager.Instance.creaturesInWorld.Remove(item);
            Destroy(item);
        }

        CreatureManager.Instance.SaveCreatures();


        List<GameObject> treesToRemove = new List<GameObject>();

        foreach (var item in TreeManager.Instance.TreesInWorld)
        {
            if (item.GetComponentInParent<IslandScript>() == SelectedIsland.GetComponent<IslandScript>())
            {
                treesToRemove.Add(item);
            }
        }

        foreach (var item in treesToRemove)
        {
            Destroy(item);
            TreeManager.Instance.TreesInWorld.Remove(item);
        }

        TreeManager.Instance.SaveTrees();
    }

    public void CheckZoom()
    {
        if (Mathf.Abs(Camera.main.orthographicSize - CC.maxCameraOrtho) < 0.1f)
        {
            ZoomToWorlds();
        }
    }

    public void ZoomToWorlds()
    {
        Debug.Log("Zoom Out To Worlds");
        CC.currentCameraPosition = CameraControl.CameraPositions.IslandSelect;
        selecting = true;
    }
}
