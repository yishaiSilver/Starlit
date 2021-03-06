﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

    public PlayerController playerController;

    public GameObject hud;
    public GameObject worldMenu;
    public GameObject codeMenu;
    public GameObject map;
    private bool mapOpen = false;

    private float lastToggleTime = 0;
    private float toggleTimeLamp = 0.25f;

    private void Start()
    {
        worldMenu.GetComponent<LandableMenu>().Start();
    }

    public void Reset()
    {
        closeAll();
        hud.SetActive(true);
    }

    public void Land(GameObject ship, LandableObject target)
    {
        closeAll();
        worldMenu.SetActive(true);
        worldMenu.GetComponent<LandableMenu>().Enable(ship, target);
    }

    public void OpenCodeMenu(Ship ship, GameObject parentMenu)
    {
        closeAll();
        codeMenu.SetActive(true);
        codeMenu.GetComponent<CodeMenu>().Enable(ship, parentMenu);
    }

    public void closeSubMenu(GameObject toClose, GameObject toOpen)
    {
        toClose.SetActive(false);
        toOpen.SetActive(true);
    }

    public void toggleMap(Ship ship)
    {
        if (Time.time - lastToggleTime > toggleTimeLamp) {
            if (mapOpen)
            {
                Reset();
            }
            else
            {
                closeAll();
                map.SetActive(true);
            }
            mapOpen = !mapOpen;
            lastToggleTime = Time.time;
        }
    }

    public void closeAll()
    {
        hud.SetActive(false);
        worldMenu.SetActive(false);
        codeMenu.SetActive(false);
        map.SetActive(false);
    }
}
