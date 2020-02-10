using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public float numOfAsteroids;
    public float meanSpeed;
    public float rangeSpeed;
    public float minRadius;
    public float maxRadius;
    public AsteroidList asteroidList;

    public void spawnAll()
    {
        for (int i = 0; i < numOfAsteroids; i++)
        {
            float radius = Random.Range(minRadius, maxRadius);
            float degree = Random.Range(0, 360);
            Asteroid ast = Instantiate(asteroidList.Asteroids[Random.Range(0, asteroidList.Asteroids.Length)], transform.position + (new Vector3(radius * Mathf.Cos(degree * Mathf.Deg2Rad), radius * Mathf.Sin(degree * Mathf.Deg2Rad))), new Quaternion());
            ast.setTarget(transform);
            ast.setSpeed(meanSpeed, rangeSpeed);
        }
    }

    private void Start()
    {
        spawnAll();
    }
}
