using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    private float speed = 0;
    public Transform target;

    private Vector3 zAxis = new Vector3(0, 0, 1);

    void FixedUpdate()
    {
        transform.RotateAround(target.position, zAxis, speed * Time.deltaTime);
        transform.Rotate(0, 0, -speed * Time.deltaTime);
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    public void setSpeed(float mean, float range)
    {
        speed = mean + Random.Range(-range, range);
    }

    private void Start()
    {
        float rand = Random.Range(0.01f, 0.03f);
        transform.localScale = new Vector3(rand, rand, 0);
    }
}
