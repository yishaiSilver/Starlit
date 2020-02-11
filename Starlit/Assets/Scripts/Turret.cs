using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private Ship ship;

    private string command;

    public float speed;
    public Transform globalTarget;

    public Transform firePoint1;
    public Transform firePoint2;
    public GameObject bulletPrefab;

    public float fireRate;
    public float accuracy;
    private float nextFire = 0.0f;
    //public float lampTime;

    private int shootCount;

    // Update is called once per frame
    private void Start()
    {
        ship = transform.parent.parent.gameObject.GetComponent<Ship>();
    }

    public void setCommand(string command)
    {
        this.command = command;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            command = "Mouse0";
        /* else if (Input.GetKey(KeyCode.T))
            command = "Mouse2";
        else if (Input.GetKey(KeyCode.E))
            command = "auto";
        else if (Input.GetKey(KeyCode.R) || Time.time - nextFire > lampTime)
            command = "StandBy"; */

        if (command == "Mouse0")
            LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        else if (command == "Mouse2")
            LookAt(globalTarget.position);
        else if (command == "auto")
        {
            LookAt(globalTarget.position);
            if (LookingAt(globalTarget.position))
                shoot();
        }
        else
            StandBy();

        

        if (Input.GetKey(KeyCode.Mouse0))
        {
            shoot();
        }
    }

    public void LookAt(Vector3 target)
    {
        Vector2 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    public bool LookingAt(Vector3 target)
    {
        Vector2 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Quaternion.Angle(rotation, transform.rotation) < 30)
            return true;
        else
            return false;
    }
    
    public void StandBy()
    {
        Quaternion rotation = this.transform.parent.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    // NOTE: Should try adding spread on gun.
    public void shoot()
    {
        if (Time.time > nextFire && !MouseInformation.onObject && ship.isFullscale())
        {
            if (!MouseInformation.onObject)
            {
                if (shootCount % 2 == 0)
                    Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
                else
                    Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
                shootCount++;
            }
            nextFire = Time.time + fireRate;
        }
    }
}