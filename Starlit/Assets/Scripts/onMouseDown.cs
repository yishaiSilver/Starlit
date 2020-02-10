using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onMouseDown : MonoBehaviour {

    public SceneInformation sceneInformation;
    private PlayerController playerController;
    public Transform target;

    private void Start()
    {
        playerController = sceneInformation.systemInformation.ship.GetComponent<PlayerController>();
    }

    private void OnMouseDown()
    {

        Debug.Log("onmousedown");
        MouseInformation.onObject = true;
        //playerController.landOnTarget(target);
    }

    private void OnMouseUp()
    {
        MouseInformation.onObject = false;
    }
}
