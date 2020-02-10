using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb2d;
    public float damage = 40f;
    public GameObject impactEffect;

    // Use this for initialization
    void Start()
    {
        rb2d.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.name.Length > 5 && hitInfo.name.Substring(0, 7).Equals("MFalcon"))
        {
            ShipInformation.hitAsteroid = true;
            Destroy(gameObject);
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }
}
