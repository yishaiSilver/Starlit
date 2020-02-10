using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{

    public float speed;
    public Transform target;

    private Vector3 zAxis = new Vector3(0, 0, 1);

    void FixedUpdate()
    {
        transform.RotateAround(target.position, zAxis, speed * Time.deltaTime);
        transform.Rotate(0, 0, -speed * Time.deltaTime);
    }
}