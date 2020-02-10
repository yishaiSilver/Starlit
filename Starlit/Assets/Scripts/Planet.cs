using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public string planetName;
    public float resources;

    public void test()
    {
        Debug.Log(planetName);
    }

    public Vector2 GetCoordinates()
    {
        return transform.position;
    }
}
