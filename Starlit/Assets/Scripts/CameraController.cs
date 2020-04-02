using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public Camera minimapCamera;

    private Vector3 offset;
    
	// Use this for initialization
	void Start () {
        //player = sceneInformation.systemInformation.ship;
        //transform.position.Set(player.transform.position.x, player.transform.position.y, 0);
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position + offset;
        //transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    public bool turnTo(Quaternion angle, float time)
    {
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, angle, time * Time.time);
        return (Quaternion.Angle(transform.rotation, angle)) < 1f;
    }

    public void setCameraLayer(int layer)
    {
        GetComponent<Camera>().cullingMask = (1 << layer) | (1 << 0) | (1 << 5);
        minimapCamera.cullingMask = (1 << layer) | (1 << 0);
    }
}
