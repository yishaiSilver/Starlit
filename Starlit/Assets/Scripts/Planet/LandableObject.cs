﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandableObject : MonoBehaviour
{
    public string planetName;
    public float resources;
    public float productionRate;
    public float price;
    public GameObject sprite;

    private void Update()
    {
        resources += productionRate * Time.deltaTime;
    }

    public float Buy(float amount)
    {
        resources -= amount;
        return -(amount * price);
    }

    public float Sell(float amount)
    {
        resources += amount;
        return amount * price;
    }

    public void enableSprite()
    {
        sprite.SetActive(true);
    }

    public void disableSprite()
    {
        sprite.SetActive(false);
    }
}
