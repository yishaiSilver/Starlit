using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour {

    public LandableList landableList;
    public string layerName;
    private int numPlayers = 0;
    private ArrayList shipList = new ArrayList();

    public LandableObject[] GetLandables()
    {
        return landableList.landables;
    }

    public int NumOfLAndables()
    {
        return landableList.landables.Length;
    }

    public int getLayerInt()
    {
        return LayerMask.NameToLayer(layerName);
    }

    public void incrementNumOfPlayersPresent()
    {
        numPlayers++;
        if (numPlayers == 1)
        {
            showPlanets();
            showShips();
        }
    }

    public void decrementNumOfPlayersPresent()
    {
        numPlayers--;
        if(numPlayers == 0)
        {
            hidePlanets();
            hideShips();
        }
    }

    public void addShip(GameObject ship)
    {
        shipList.Add(ship);
    }

    public void removeShip(GameObject ship)
    {
        shipList.Remove(ship);
    }

    public bool shouldShowShip()
    {
        return numPlayers > 0;
    }
}
