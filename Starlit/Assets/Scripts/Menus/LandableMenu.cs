using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandableMenu : MonoBehaviour {

    public HUDManager hudManager;

    private string stringCargo = "Cargo: ";
    private string stringPlanet = "Planet: ";

    private bool isShowing = false;
    public GameObject worldMenu;
    public Ship theShip; //------------
    public LandableObject theWorld; //Change from public to private (testing rn)

    private Text title;
    private Text panelCargo;
    private Text panelPlanetResources;

    public void Start()
    {
        title = transform.Find("Panel").Find("PanelName").Find("Text").GetComponent<Text>();
        panelCargo = transform.Find("Panel").Find("PanelCargo").Find("Text").GetComponent<Text>();
        panelPlanetResources = transform.Find("Panel").Find("PanelPlanetResources").Find("Text").GetComponent<Text>();
    }

    public void BuyItem()
    {
        if(theShip.currentCargo < theShip.cargoSize && theWorld.resources > 0)
        {
            theShip.currentCargo++;
            theWorld.Buy(1);

            Debug.Log("Bought item.");
        }

        panelCargo.text = stringCargo + theShip.currentCargo + "/" + theShip.cargoSize;
        panelPlanetResources.text = stringPlanet + theWorld.resources;

    }

    public void SellItem()
    {
        if (theShip.currentCargo > 0)
        {
            theShip.currentCargo--;
            theWorld.Sell(1);
        }

        panelCargo.text = stringCargo + theShip.currentCargo + "/" + theShip.cargoSize;
        panelPlanetResources.text = stringPlanet + theWorld.resources;
    }

    public void Enable(GameObject ship, GameObject world)
    {
        theShip = ship.GetComponent<Ship>();
        theWorld = world.GetComponent<LandableObject>();
        
        title.text = world.name.ToUpper();

        panelCargo.text = stringCargo + theShip.currentCargo + "/" + theShip.cargoSize;
        panelPlanetResources.text = stringPlanet + theWorld.resources;
    }

    public void LaunchCodeMenu()
    {
        hudManager.OpenCodeMenu(theShip, gameObject);
    }
}
