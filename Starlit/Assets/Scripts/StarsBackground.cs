/*
 * Class used to create parallax effect of stars
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsBackground : MonoBehaviour
{

    public float speedDenominator;

    // Update is called once per frame
    void Update()
    {

        SpriteRenderer mr = GetComponent<SpriteRenderer>();

        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / (transform.localScale.x * speedDenominator);
        offset.y = transform.position.y / (transform.localScale.y * speedDenominator);

        mat.mainTextureOffset = offset;

    }
}
