using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour {

    public LandableList landableList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public LandableObject[] GetLandables()
    {
        return landableList.landables;
    }

    public int NumOfLAndables()
    {
        return landableList.landables.Length;
    }
}
