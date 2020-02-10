/*
 * Class used to create parallax effect of stars
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsBackground : MonoBehaviour
{
    SpriteRenderer mr;

    Material mat;

    public float speedDenominator;

    private void Start()
    {
        mr = GetComponent<SpriteRenderer>();
        mat = mr.material;

        mat.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / (transform.localScale.x * speedDenominator);
        offset.y = transform.position.y / (transform.localScale.y * speedDenominator);

        mat.mainTextureOffset = offset;

    }
}
