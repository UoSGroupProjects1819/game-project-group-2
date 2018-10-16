using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour {

    public List<GameObject> PlantsOnIsland;

    public List<GameObject> CreaturesOnIsland;

    public void UpdateCreatureHappniess()
    {
        foreach (var creature in CreaturesOnIsland)
        {
            creature.GetComponent<CreatureScript>().UpdateHappniess(PlantsOnIsland);
        }
    }

    public void AddPlant(GameObject plant)
    {
        PlantsOnIsland.Add(plant);
        UpdateCreatureHappniess();
    }

    public void AddCreature(GameObject creature)
    {
        CreaturesOnIsland.Add(creature);
        creature.GetComponent<CreatureScript>().UpdateHappniess(PlantsOnIsland);
    }
}
