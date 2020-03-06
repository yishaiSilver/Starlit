/*
 * Class used to create parallax effect of stars
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsBackground : MonoBehaviour
{
    Renderer rend;
    Material mat;

    public float speedDenominator;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;

        //mat.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }

    // Credits: quill18creates (youtube)
    // Purpose: updates the stars' offset to give appearance of movement.
    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / (transform.localScale.x * speedDenominator);
        offset.y = transform.position.y / (transform.localScale.y * speedDenominator);

        mat.mainTextureOffset = offset;
    }
}
