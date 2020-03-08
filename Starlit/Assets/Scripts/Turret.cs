using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private Ship ship;

    private string command;

    public float speed;
    public Transform globalTarget;

    public AudioClip impact;
    public float volume = 0.7f;
    public AudioSource source;

    public Transform firePoint1;
    public Transform firePoint2;
    public GameObject bulletPrefab;

    public float fireRate;
    private float nextFire = 0.0f;
    public float lampTime;
    private int shootCount;

    // Used to track target object's acceleration
    public GameObject targetLeaded1;
    public GameObject targetLeaded3;
    private float initRadialSpeed;
    private Vector3 targetVelocity;

    // Update is called once per frame
    private void Start()
    {
        initRadialSpeed = 0;
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
        //else if (Input.GetKey(KeyCode.T))
        //    command = "Mouse2";
        else if (Input.GetKey(KeyCode.E))
            command = "auto";
        else if (Input.GetKey(KeyCode.R))
            command = "StandBy";

        if (command == "Mouse0")
            LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        else if (command == "Mouse2")
            LookAt(globalTarget.position);
        else if (command == "auto")
        {
            if (true)
            {
                LockOn1(globalTarget.gameObject);
                Vector3 leadedPosition = LockOn1(globalTarget.gameObject);
                if (leadedPosition != new Vector3(0, 0, 0))
                {
                    LookAt(leadedPosition);
                    if (LookingAt(leadedPosition))
                        shoot();
                }
            }
            else
            {
                LookAt(globalTarget.position);
                if (LookingAt(globalTarget.position))
                    shoot();
            }
        }
        else
            StandBy();



        if (Input.GetKey(KeyCode.Mouse0))
        {
            shoot();
        }
    }

    // look at the target
    public void LookAt(Vector3 target)
    {
        Vector2 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    public Vector3 LockOn2(GameObject target)
    {
        Vector3 displacement = target.transform.position - transform.position;

        Vector3 targetVelocity = target.GetComponent<Rigidbody2D>().velocity;

        Vector3 withVelocity = displacement + targetVelocity;

        float radialVelocity = (withVelocity - displacement).magnitude;

        float timeToContact = displacement.magnitude / (bulletPrefab.GetComponent<Bullet>().speed - radialVelocity);

        Vector3 expectedPosition = target.transform.position + (targetVelocity * timeToContact);

        if (targetLeaded1 != null)
        {
            targetLeaded1.transform.position = expectedPosition;
        }

        return expectedPosition;
    }

    // Uses Radial velocity; original
    public Vector3 LockOn1(GameObject target)
    {
        // Get the distance, direction
        Vector2 displacement = target.transform.position - transform.position;
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg - 90;

        // get the radial speed
        targetVelocity = target.transform.GetComponent<Rigidbody2D>().velocity;
        float targetVelocityAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x);
        float deltaAngle = Mathf.DeltaAngle(angle, targetVelocityAngle * Mathf.Rad2Deg);
        float theta = 90 - deltaAngle;
        float radialSpeed = Mathf.Sin(theta * Mathf.Deg2Rad) * targetVelocity.magnitude;

        float targetRadialAcceleration = (radialSpeed - initRadialSpeed) / Time.deltaTime;

        // find the time to contact
        float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;
        // quadratic equation:
        float negB = -1 * (radialSpeed - bulletSpeed);
        float sqrtOperand = Mathf.Pow(negB, 2) - 2 * targetRadialAcceleration * displacement.magnitude;
        float dividend = 0.5f * targetRadialAcceleration;

        if (sqrtOperand > 0 && targetRadialAcceleration > 0)
        {
            float timeToContact1 = (negB + Mathf.Sqrt(sqrtOperand)) / dividend;
            float timeToContact2 = (negB - Mathf.Sqrt(sqrtOperand)) / dividend;

            float timeToContact = 0;

            if (timeToContact2 < 0)
                timeToContact = timeToContact1;
            else
                timeToContact = Mathf.Min(timeToContact1, timeToContact2);

            timeToContact = Mathf.Max(timeToContact, 0);
            
            Vector3 expectedPosition = target.transform.position // displacement
                + (targetVelocity * timeToContact);          // vi * t 
                                                             //+ (targetVelocity / targetVelocity.magnitude) * 0.5f * targetAcceleration * Mathf.Pow(timeToContact, 2); // 1/2 a t ^2

            if (targetLeaded1 != null)
            {
                targetLeaded1.transform.position = expectedPosition;
            }

            if ((expectedPosition - transform.position).magnitude <= bulletPrefab.GetComponent<Bullet>().range)
            {
                initRadialSpeed = radialSpeed;
                return expectedPosition;
            }
        }
        initRadialSpeed = radialSpeed;
        return new Vector3(0, 0, 0);
    }

    // Uses Radial velocity; original
    public Vector3 LockOn3(GameObject target)
    {
        // Get the distance, direction
        Vector2 displacement = target.transform.position - transform.position;
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg - 90;

        // get the radial speed
        targetVelocity = target.transform.GetComponent<Rigidbody2D>().velocity;
        float targetVelocityAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x);
        float deltaAngle = Mathf.DeltaAngle(angle, targetVelocityAngle * Mathf.Rad2Deg);
        float theta = 90 - deltaAngle;
        float radialSpeed = Mathf.Sin(theta * Mathf.Deg2Rad) * targetVelocity.magnitude;

        float targetRadialAcceleration = (radialSpeed - initRadialSpeed) / Time.deltaTime;

        // find the time to contact
        float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;

        if ((bulletSpeed - radialSpeed) > 0)
        {
            float timeToContact = displacement.magnitude / (bulletSpeed - radialSpeed);

            Vector3 expectedPosition = target.transform.position // displacement
                + (targetVelocity * timeToContact);          // vi * t 
                                                             //+ (targetVelocity / targetVelocity.magnitude) * 0.5f * targetAcceleration * Mathf.Pow(timeToContact, 2); // 1/2 a t ^2

            if (targetLeaded3 != null)
            {
                targetLeaded3.transform.position = expectedPosition;
            }

            if ((expectedPosition - transform.position).magnitude <= bulletPrefab.GetComponent<Bullet>().range)
            {
                initRadialSpeed = radialSpeed;
                return expectedPosition;
            }
        }
        initRadialSpeed = radialSpeed;
        return new Vector3(0, 0, 0);
    }

    // are you looking at the target?
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

    // shoot
    public void shoot()
    {
        if (Time.time > nextFire && !MouseInformation.onObject && ship.isFullscale())
        {
            if (!MouseInformation.onObject)
            {
                if (shootCount % 2 == 0)
                {
                    GameObject laser = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
                    laser.layer = gameObject.layer;
                }
                else
                {
                    GameObject laser = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
                    laser.layer = gameObject.layer;
                }
                shootCount++;
            }
            nextFire = Time.time + fireRate;
        }
    }
}