/*
	All scripts must contain a Main() method.

	Accessible commands are:
		- TurnRight()
		- TurnLeft()
		- GetRadar()
*/

using UnityEngine;
using System.Collections;

class Auto1 : Auto
{
    private const int waitLowerBound = 1;
    private const int waitUpperBound = 15;

    public Ship ship;
    public StarSystem starSystem;

    private int lastLanded = -1;
    private int targetInSystem = -1;

    private bool shouldWait = false;

    void Start()
    {
        ship = GetComponent<Ship>();
        ship.UpdateStarSystem();
        starSystem = ship.starSystem;
    }

    // Will execute once per frame if enabled (U)
    public override void main()
    {
        if (shouldWait)
            return;
        
        while(lastLanded == targetInSystem)
        {
            targetInSystem = Random.Range(0, starSystem.NumOfLAndables());
        }

        ship.targetObject = starSystem.GetLandables()[targetInSystem];

        if (!ship.Land(ship.targetObject))
        {
            ship.Thrust();
        }
        else
        {
            lastLanded = targetInSystem;

            shouldWait = true;

            int seconds = Random.Range(waitLowerBound, waitUpperBound);
            StartCoroutine(waitForSeconds(seconds));
        }
    }

    IEnumerator waitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ship.TakeOff();
        shouldWait = false;
    }

    // Will execute once the user has chosen to disable (I)
    public void Disable()
    {

    }
}