using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoryController {

    public static bool hasTalked;

    public static bool landedOnStation;
    public static bool landedOnMoon;
    public static bool landedOnMars;
    public static bool landedOnStationOne;
    public static bool landedOnStationTwo;
    public static bool landedOnJupiter;
    public static bool landedOnSaturn;

    public static bool engaged = true;

    public static TargetIndicator hiddenTarget;

    public static bool canJump = false;

    public static float numOfEnemies;
    public static float numKilled;

    public static void update(string landedOn)
    {
        switch (landedOn)
        {
            case "Station":
                landedOnStation = true;
                break;
            case "Moon":
                if (landedOnStation)
                {
                    landedOnMoon = true;
                    canJump = true;
                }
                break;
            case "Mars":
                landedOnMars = true;
                canJump = true;
                break;
            case "Station 1":
                landedOnStationOne = true;
                hiddenTarget.setHidden(false);
                break;
            case "Station 2":
                landedOnStationTwo = true;
                canJump = true;
                break;
            case "Jupiter":
                landedOnJupiter = true;
                hiddenTarget.setHidden(false);
                break;
            case "Saturn":
                landedOnSaturn = true;
                hiddenTarget.setHidden(false);
                break;
        }
        //Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", landedOnStation, landedOnMoon, landedOnMars, landedOnStationOne, landedOnStationTwo, landedOnJupiter, landedOnSaturn));
        //Debug.Log(canJump);
    }

    public static string nextScene(string current)
    {
        if (current == "1EarthScene")
        {
            return "2MarsScene";
        }
        else if (current == "2MarsScene")
        {
            return "3BeltScene";
        }
        else if (current == "3BeltScene")
        {
            return "4JupiterScene";
        }
        else if (current == "4JupiterScene")
        {
            return "5SaturnScene";
        }
        else if (current == "5SaturnScene")
        {
            return "6EndgameScene";
        }
        return null;
    }

    public static void rollCAll()
    {
        numOfEnemies++;
    }

    public static void wasKilled()
    {
        numKilled++;
        if(numKilled == numOfEnemies)
        {
            canJump = true;
        }
        Debug.Log(canJump + " " + numKilled + " " + numOfEnemies);
    }
}
