using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 20f;
    public float range;
    public Rigidbody2D rb2d;
    public float damage = 40f;
    public GameObject impactEffect;
    private float modifier;

    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        rb2d.velocity = transform.up * speed;
        originalPosition = transform.position;
        modifier = Random.Range(0.93f, 1.07f);
        range *= modifier;
	}

    private void Update()
    {
        if(Vector3.Distance(originalPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Ship")
        {
            ShipInformation.hitAsteroid = true;
            Destroy(gameObject);
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }

    public void addVelocity(Vector2 v)
    {
        rb2d.velocity += v;
    }
}
