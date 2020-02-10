/*
 * Class used to rotate the 2001 spacestation
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour {
    
    public float rotSpeed;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
	}
}
