using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour {

    public LandableList landableList;
    public string layerName;
    private int layerInt;
    private int numPlayers = 0;
    private ArrayList shipList = new ArrayList();
    public GameObject StarSprite;

    private int starSystemIndex;
    public StarSystemList neighboringStars;
    private StarSystem[] neighboringStarsArr;
    //private StarSystemLink[] starLinks;

    void Start()
    {
        neighboringStarsArr = neighboringStars.systems;
        layerInt = LayerMask.NameToLayer(layerName);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Ship");
        foreach(GameObject obj in objs)
        {
            if (obj.layer == layerInt)
            {
                addShip(obj);
            }
        }

        if(numPlayers != 0)
        {
            showPlanets();
            showStar();
            showShips();
        }
        else
        {
            hidePlanets();
            hideStar();
            hideShips();
        }
    }

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
            showStar();
            showShips();
        }
    }

    public void decrementNumOfPlayersPresent()
    {
        numPlayers--;
        if(numPlayers == 0)
        {
            hidePlanets();
            hideStar();
            hideShips();
        }
    }

    public void addShip(GameObject ship)
    {
        shipList.Add(ship);
        if (ship.GetComponent<Ship>().isPlayer)
        {
            incrementNumOfPlayersPresent();
        }
    }

    public void removeShip(GameObject ship)
    {
        shipList.Remove(ship);
        if (ship.GetComponent<Ship>().isPlayer)
        {
            decrementNumOfPlayersPresent();
        }
    }

    public bool shouldShowShip()
    {
        return numPlayers > 0;
    }

    public void showPlanets() // Am eventually going to have to somehow account for asteroids that cannot be landed on.
    {
        foreach (LandableObject obj in landableList.landables)
        {
            obj.enableSprite();
        }
    }

    public void hidePlanets()
    {
        foreach (LandableObject obj in landableList.landables)
        {
            obj.disableSprite();
        }
    }

    public void showShips()
    {
        foreach(GameObject ship in shipList)
        {
            ship.GetComponent<Ship>().enableSprite();
        }
    }

    public void hideShips()
    {
        foreach (GameObject ship in shipList)
        {
            ship.GetComponent<Ship>().disableSprite();
        }
    }

    public void showStar()
    {
        StarSprite.SetActive(true);
    }

    public void hideStar()
    {
        StarSprite.SetActive(false);
    }

    public void setStarSystemIndex(int n)
    {
        starSystemIndex = n;
    }

    public int getStarSystemIndex()
    {
        return starSystemIndex;
    }

    public StarSystem[] getNeighboringStars()
    {
        return neighboringStarsArr;
    }
}
