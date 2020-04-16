using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour {

    public SpriteRenderer renderer;

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Round(transform.parent.position.x), Mathf.Round(transform.parent.position.y), transform.parent.position.z);
    }
}
