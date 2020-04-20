using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public bool debug;

    private HUDManager hudManager;
    public PlayerController playerController;
    public StarSystemManager starSystemManager;

    //-----Ship Characteristics
    public bool isPlayer;
    public float thrust;
    public float maxSpeed;
    public float rotSpeed;
    public float alignSpeed;
    public float shrinkSpeed;
    private Vector3 shipSize;
    //--------------------------

    //-----Ship Attributes
    private Rigidbody2D rb2d;
    private Animator animator;
    private float rotation = 90;
    private float oppositeDirectionAngle;
    public bool thrusting = false;
    private int batterySize;
    private int shieldSize;
    private int healthSize;
    private int batteryStatus;
    private int shieldStatus;
    private int healthStatus;
    //--------------------------

    //-----Ship Initiatives
    public Transform targetGlobal;
    //public GameObject[] landableObjects;
    private LandableObject[] landables;

    //------landing
    public LandableObject targetObject;
    public bool landing = false;
    private bool stoppedFirst = false;
    private bool landingTargetTargeted = false;
    private bool distanceToStop = false;
    private bool thrustingForLand = false;
    private bool stoppedAtTarget = false;
    private bool alignedWithTarget = false;
    private bool landed = false;
    private bool shouldMaximize = false;
    private bool notFullscale;
    //---------------------------

    //-----Jumping
    private bool jumping;
    private float jumpTime = 1f;
    private float jumpSpeed = 200f;
    private float jumpExitSpeed = 2f;
    private bool stoppedForJump = false;
    private bool lookedAtStar = false;
    private bool inHyperSpace = false;
    private bool acceleratedForJump = false;
    private bool jumped = false;
    private float initAccelerationTime = -1;
    private float jumpDistance;
    private float jumpExitPositionRange = 10f;
    //---------------------------

    //Current star system -------
    private GameObject overObject;
    public float currentCargo;
    public float cargoSize;
    public string shipCode;
    //private ShipScript script;
    //---------------------------

    public GameObject sprite;

    private Map map;

    //-----World Interaction
    public StarSystem starSystem;
    private MapNode targetOnMap;
    private Stack<MapJump> currentDirections;

    public GameObject debugIndicator;
    public GameObject debugIndicator2;

    void Start()
    {
        shipSize = transform.localScale;

        rb2d = GetComponent<Rigidbody2D>();

        animator = sprite.GetComponent<Animator>();
        ShipInformation.jumpped = true;

        //script = GetComponent<ShipScript>();

        UpdateStarSystem();

        currentCargo = 0;

        alignSpeed = 1f;

        //map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

        float jumpDeacceleration = (jumpExitSpeed - jumpSpeed) / jumpTime;
        jumpDistance = (jumpSpeed * jumpTime) + 0.5f * jumpDeacceleration * jumpTime * jumpTime;
    }

    private void FixedUpdate()
    {
        // Rotate
        rb2d.MoveRotation(rotation - 90);

        // Unshrink if necessary
        if (shouldMaximize)
        {
            shouldMaximize = !Maximize();
        }

        // Set Target
        if (Input.GetMouseButtonDown(1) && isFullscale())
        {
            StopAutomatic();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                targetObject = hit.collider.gameObject.GetComponent<LandableObject>();
                landing = true;
                SetLandingTargetText(hit.collider.gameObject);
            }
        }

        // Move with the planet if landed
        if (landed)
        {
            MoveTowards();
        }
    }

    public void Thrust()
    {
        if (thrusting)
        {
            Vector2 direction = new Vector2(Mathf.Cos((rotation) * Mathf.Deg2Rad), Mathf.Sin((rotation) * Mathf.Deg2Rad));
            rb2d.AddForce(direction * thrust);

            if (!jumping)
            {
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            }

            if (sprite.activeSelf)
                animator.SetBool("Firing", true);

            thrusting = false;
        }
        else
        {
            if (sprite.activeSelf)
                animator.SetBool("Firing", false);
        }
    }

    public void ExecuteCustomScript()
    {
        //script.ExecuteCode();
    }

    public void Automatic()
    {
        //if (script.shouldExecute)
        //{
        //    ExecuteCustomScript();
        //}
        if (landing)
            Land(targetObject);
        else if (landed)
        {
            MoveTowards(targetObject.transform);
        }

        if (jumping)
            Jump();

        Thrust();
    }

    public bool Manual()
    {
        if (inHyperSpace)
        {
            return false;
        }

        bool ret = false;

        // Accelerate
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            thrusting = true;
            ret = true;
        }

        // Stop
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Stop();
            ret = true;
        }

        // Turn right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotation -= rotSpeed * Time.deltaTime;
            ret = true;
        }

        // Turn left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotation += rotSpeed * Time.deltaTime;
            ret = true;
        }

        // Land
        if (Input.GetKey(KeyCode.L))
        {
            targetObject = FindClosestLandableObject();
            SetLandingTargetText(targetObject.gameObject);
            landing = true;
        }

        if (Input.GetKey(KeyCode.T))
        {
            TakeOff();
        }

        if (Input.GetKey(KeyCode.U))
        {
            //script.shouldExecute = true;
        }
        else if (ret || Input.GetKey(KeyCode.I))
        {
            //script.shouldExecute = false;
            ret = true;
        }

        if (Input.GetKey(KeyCode.K))
        {
            //currentDirections = map.getStackTo(starSystem, targetStar);
        }

        if (!(currentDirections == null || currentDirections.Count == 0) && (jumping || Input.GetKey(KeyCode.J)))
        {
            jumping = true;
        }

        if (Input.GetKey(KeyCode.M))
        {
            playerController.toggleMap();
        }

        Thrust();
        return ret;
    }

    public void TurnRight()
    {
        rotation -= rotSpeed * Time.deltaTime;
    }

    public void TurnLeft()
    {
        rotation += rotSpeed * Time.deltaTime;
    }

    private bool LookAt(Transform target)
    {
        var dir = target.position - transform.position;
        float angleTo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotation = Mathf.MoveTowardsAngle(rotation, angleTo, rotSpeed * Time.deltaTime);

        if (rotation == angleTo)
            return true;
        else
            return false;
    }

    private bool LookAt(float targetAngle)
    {
        rotation = Mathf.MoveTowardsAngle(rotation, targetAngle, rotSpeed * Time.deltaTime);

        if (rotation == targetAngle)
            return true;
        else
            return false;
    }

    public bool LookOpposite()
    {
        oppositeDirectionAngle = Mathf.Atan2(-rb2d.velocity.y, -rb2d.velocity.x) * Mathf.Rad2Deg;
        rotation = Mathf.MoveTowardsAngle(rotation, oppositeDirectionAngle, rotSpeed * Time.deltaTime);

        if (rotation == oppositeDirectionAngle)
            return true;
        else
            return false;
    }

    public bool Stop()
    {
        if (rb2d.velocity.magnitude == 0)
            return true;

        if (LookOpposite())
        {
            thrusting = true;
            if (rb2d.velocity.magnitude - (thrust / rb2d.mass * Time.deltaTime) < 0)
            {
                rb2d.velocity = new Vector2(0, 0);
                thrusting = false;
            }
        }

        if (rb2d.velocity.magnitude == 0)
            return true;
        else
            return false;
    }

    public bool StopLook(Transform target)
    {
        float angleVelocity = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;

        Vector3 dir = target.position - transform.position;
        float angleTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float theta = Mathf.DeltaAngle(angleTarget, angleVelocity);

        float radialVelocity = Mathf.Sin(theta * Mathf.Deg2Rad) * rb2d.velocity.magnitude;


        float badVelocityComponent = Mathf.Sin(theta) * angleVelocity;

        float angleToTurn = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rotation = Mathf.MoveTowardsAngle(rotation, angleToTurn, rotSpeed * Time.deltaTime);

        if (debugIndicator != null)
            debugIndicator.transform.position = dir + transform.position;
        if (debugIndicator2 != null)
            //debugIndicator2.transform.position = estimatedPosition + transform.position;

            /*float stopAngle = Mathf.Atan2(-rb2d.velocity.y, -rb2d.velocity.x) * Mathf.Rad2Deg;

            var dir = target.position - transform.position;
            float angleToTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float angleToTurn = Mathf.DeltaAngle(angleToTarget, stopAngle) / 2f + angleToTarget;
            */

            //float angleToTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //float angleVelocity = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(angleVelocity - angleTarget) <= 10)
                return true;
        /*{

            float delta = Vector2.Distance(target.transform.position, transform.position);
            float timeToRotate = 180f / (rotSpeed);
            float distToStop = (rb2d.velocity.magnitude * timeToRotate) + (Mathf.Pow(rb2d.velocity.magnitude, 2) / (2 * (thrust / rb2d.mass)));

            if (debug)
            {
                Debug.Log(delta - distToStop);
            }

            if (delta - distToStop >= 0)
            {
                if(rotation == angleToTurn)
                    thrusting = true;

                return false;
            }
            else
                return true;
        }
        if (rotation == angleToTurn)
            thrusting = true;*/
        if (rotation == angleToTurn)
            thrusting = true;
        return false;
    }

    public LandableObject FindClosestLandableObject()
    {
        landables = starSystem.GetLandables();

        if (landables.Length > 0)
        {
            LandableObject closest = landables[0];
            float dist = Vector2.Distance(transform.position, closest.transform.position);

            foreach (LandableObject obj in landables)
            {
                if (Vector2.Distance(transform.position, obj.transform.position) < dist)
                {
                    closest = obj;
                    dist = Vector2.Distance(transform.position, obj.transform.position);
                }
            }

            return closest;
        }
        return null;
    }

    public bool MoveTowards(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, alignSpeed * Time.deltaTime);

        if (transform.position.x == target.position.x && transform.position.y == target.position.y)
            return true;
        else
            return false;
    }

    public void MoveTowards()
    {
        MoveTowards(targetObject.transform);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag != "Weapon")
        {
            //Debug.Log("Hit");
            overObject = hitInfo.gameObject;
        }


        //animator.SetBool("Hit", true);
    }

    public void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.name != "Laser(Clone)")
        {
            overObject = null;
        }
    }

    /*public bool Land(LandableObject target)
    {
        //------Stopping
        if (!stoppedFirst && !StopLook(target.transform))
            return false;
        else
            stoppedFirst = true;
        //--------------------------

        if (!distanceToStop && !ThrustToTarget(target.transform))
            return false;
        else
            distanceToStop = true;

        //--------Stopping at target
        if (!Stop() && !stoppedAtTarget)
            return false;
        else
        {
            stoppedAtTarget = true;
            thrusting = false;
        }

        //-------Aligning with Target and shrink
        bool moved = MoveTowards(target.transform);
        bool shrunk = Shrink();

        if (!moved || !shrunk)
            return false;
        else // -------------------------------FINISHED
        {
            // reset landing bools
            landing = false;
            stoppedFirst = false;
            landingTargetTargeted = false;
            distanceToStop = false;
            thrustingForLand = false;
            stoppedAtTarget = false;

            // the eagle has landed
            landed = true;

            if (playerController != null)
                playerController.notifyOfLand(gameObject, target);
            //
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% OPEN WORLD MENU
            //
            return true;
        }
    }*/

    public bool Land(LandableObject target)
    {
        //------Stopping
        if (!stoppedFirst && !Stop())
            return false;
        else
            stoppedFirst = true;
        //--------------------------

        //-------Turning
        if (!landingTargetTargeted && !LookAt(target.transform))
            return false;
        else
            landingTargetTargeted = true;
        //---------------------------

        //-------Thrusting to Target
        if (!distanceToStop && !ThrustToTarget(target.transform))
            return false;
        else
            distanceToStop = true;
        //----------------------------

        //-------Aligning with Target
        if (!distanceToStop && !ThrustToTarget(target.transform))
            return false;
        else
            distanceToStop = true;
        //----------------------------

        //--------Stopping at target
        if (!Stop() && !stoppedAtTarget)
            return false;
        else
        {
            stoppedAtTarget = true;
            thrusting = false;
        }

        //-------Aligning with Target and shrink
        bool moved = MoveTowards(target.transform);
        bool shrunk = Shrink();

        if (!moved || !shrunk)
            return false;
        else // -------------------------------FINISHED
        {
            // reset landing bools
            landing = false;
            stoppedFirst = false;
            landingTargetTargeted = false;
            distanceToStop = false;
            thrustingForLand = false;
            stoppedAtTarget = false;

            // the eagle has landed
            landed = true;

            if (playerController != null)
                playerController.notifyOfLand(gameObject, target);
            //
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% OPEN WORLD MENU
            //
            return true;
        }
    }

    public void TakeOff()
    {
        StopAutomatic();
        thrusting = true;
        Thrust();
    }

    public void StopAutomatic()
    {
        // reset landing bools
        landing = false;
        stoppedFirst = false;
        landingTargetTargeted = false;
        distanceToStop = false;
        thrustingForLand = false;
        stoppedAtTarget = false;
        landed = false;

        // reset jumping bools
        jumping = false;

        shouldMaximize = true;

        ResetMenu();
    }

    public void ResetMenu()
    {
        if (playerController != null)
            playerController.notifyOfTakeoff();
    }

    private bool ThrustToTarget(Transform target)
    {
        // thrust while target is grater than distance away.

        float delta = Vector2.Distance(target.transform.position, transform.position);
        float timeToRotate = Mathf.Abs(Mathf.DeltaAngle(rotation, Mathf.Atan2(-rb2d.velocity.y, -rb2d.velocity.x) * Mathf.Rad2Deg)) / (rotSpeed);
        float distToStop = (rb2d.velocity.magnitude * timeToRotate) + (Mathf.Pow(rb2d.velocity.magnitude, 2) / (2 * (thrust / rb2d.mass)));

        if (delta - distToStop >= 0)
        {
            LookAt(target);
            thrusting = true;
            return false;
        }
        else
            return true;

    }

    public void SetLandingTargetText(GameObject target)
    {
        if (playerController != null)
        {
            playerController.SetTarget(target);
        }
    }

    public bool Shrink()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0, 0, 0), shrinkSpeed * Time.deltaTime);
        notFullscale = true;

        if (transform.localScale == new Vector3(0, 0, 0))
            return true;
        else
            return false;
    }

    public bool Maximize()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, shipSize, shrinkSpeed * Time.deltaTime);

        if (transform.localScale == shipSize)
        {
            notFullscale = false;
            return true;
        }
        else
            return false;
    }

    public bool isFullscale()
    {
        return !notFullscale;
    }

    public void setPlayerController(PlayerController pc)
    {
        playerController = pc;
    }

    public bool UpdateCode(string source)
    {
        shipCode = source;
        return true; // script.UpdateScript(this, starSystem, source);
    }

    public void UpdateStarSystem()
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");

        foreach (GameObject star in stars)
        {
            if (star.layer == gameObject.layer)
            {
                starSystem = star.GetComponent<StarSystem>();
            }
        }
    }

    public void JumpToStarSystem(StarSystem to)
    {
        if (playerController != null)
        {
            playerController.JumpToSystem(to.getLayerInt());
        }
        starSystemManager.TransferSystems(starSystem, to, gameObject);
    }

    public void enableSprite()
    {
        //Debug.Log(gameObject.name);
        sprite.SetActive(true);
    }

    public void disableSprite()
    {
        sprite.SetActive(false);
    }

    public Vector2 getVelocity()
    {
        return rb2d.velocity;
    }

    public StarSystem getStarSystem()
    {
        return starSystem;
    }

    public void setJumpPath(Stack<MapJump> directions)
    {
        currentDirections = directions;
    }

    public void setTargetOnMap(MapNode target)
    {
        targetOnMap = target;
    }

    public MapNode getTargetOnMap()
    {
        return targetOnMap;
    }

    public bool Jump()
    {
        //------Stopping
        if (!stoppedForJump && !Stop())
            return false;
        else
            stoppedForJump = true;
        //--------------------------

        //-------Turning
        if (!lookedAtStar && !LookAt(currentDirections.Peek().getAngle()))
            return false;
        else
            lookedAtStar = true;
        //---------------------------

        //-------Accelerating
        if (!acceleratedForJump && !JumpAccelerate(true))
        {
            inHyperSpace = true;
            return false;
        }
        else
            acceleratedForJump = true;
        //--------------------------

        //--------Changing Star System
        if (!jumped)
        {
            transform.position = getEntryPositionForJump();
            JumpToStarSystem(currentDirections.Peek().getTargetStar());
            starSystem = currentDirections.Pop().getTargetStar();
            jumped = true;
        }
        //-------------------------

        //-------Deaccelerating
        if (!JumpAccelerate(false))
            return false;
        else
        {
            jumping = (currentDirections.Count != 0) ? true : false;
            stoppedForJump = false;
            lookedAtStar = false;
            acceleratedForJump = false;
            jumped = false;
            inHyperSpace = false;

            return true;
        }
    }

    public bool JumpAccelerate(bool enteringHyperspace)
    {
        if (initAccelerationTime == -1)
        {
            initAccelerationTime = Time.time;
        }

        float progress = (Time.time - initAccelerationTime) / jumpTime;

        Vector2 direction = new Vector2(Mathf.Cos((rotation) * Mathf.Deg2Rad), Mathf.Sin((rotation) * Mathf.Deg2Rad));

        if (enteringHyperspace)
        {
            rb2d.velocity = Vector2.LerpUnclamped(new Vector2(0, 0), direction * jumpSpeed, progress);

            //rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, jumpSpeed);

            if (rb2d.velocity.magnitude >= jumpSpeed)
            {
                initAccelerationTime = -1;
                return true;
            }
        }
        else
        {
            rb2d.velocity = Vector2.Lerp(direction * jumpSpeed, direction * jumpExitSpeed, progress);

            if (rb2d.velocity.magnitude <= jumpExitSpeed)
            {
                rb2d.velocity = direction * jumpExitSpeed;
                initAccelerationTime = -1;
                return true;
            }
        }

        return false;
    }

    // SHOULD IMPLEMENT TIERED LAYERS HERE FOR HAVING MORE STAR SYSTEMS IN A SINGLE LAYER
    public Vector3 getEntryPositionForJump()
    {
        Vector3 starLocation = currentDirections.Peek().getTargetStar().transform.position;
        Vector3 endLocation = new Vector3(starLocation.x + Random.Range(-jumpExitPositionRange, jumpExitPositionRange),
            starLocation.x + Random.Range(-jumpExitPositionRange, jumpExitPositionRange),
            starLocation.z);

        Vector3 direction = new Vector2(Mathf.Cos((rotation + 180) * Mathf.Deg2Rad), Mathf.Sin((rotation + 180) * Mathf.Deg2Rad));
        Vector3 exitJump = direction * jumpDistance;

        Vector3 exitSpot = endLocation + exitJump;


        return exitSpot;
    }

    public float getShieldProportion()
    {
        return shieldStatus / shieldSize;
    }

    public float getBatteryProportion()
    {
        return batteryStatus / batterySize;
    }

    public float getHealthProportion()
    {
        return healthStatus / healthSize;
    }
}