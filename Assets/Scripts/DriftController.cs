using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriftController : MonoBehaviour
{

    #region Parameters
    public float Accel = 15.0f;         // In meters/second2
    public float TopSpeed = 30.0f;      // In meters/second

    public float BoostTopSpeed = 60.0f;

    public float GripX = 12.0f;          // In meters/second2
    public float GripZ = 3.0f;          // In meters/second2
    public float Rotate = 190;       // In degree/second
    public float RotVel = 0.8f;         // Ratio of forward velocity transfered on rotation

    public float BoostFactor = 0.5f;

    public float GravityIncreaseHeight = 35f;

    public float JumpFactor = 1f;

    public float maxTiltAngle = 30f;

    public float flyingGroundDistance = 0.2f;

    // Center of mass, fraction of collider boundaries (= half of size)
    // 0 = center, and +/-1 = edge in the pos/neg direction.
    public Vector3 CoM = new Vector3(0f, .5f, 0f);


    // Ground & air angular drag
    // reduce stumbling time on ground but maintain on-air one
    float AngDragG = 5.0f;
    float AngDragA = 0.05f;

    // Rotational
    float MinRotSpd = 1f;           // Velocity to start rotating
    float MaxRotSpd = 4f;           // Velocity to reach max rotation
    public AnimationCurve SlipL;    // Slip hysteresis static to full (x, input = speed)
    public AnimationCurve SlipU;    // Slip hysteresis full to static (y, output = slip ratio)
    public float SlipMod = 20f;     // Basically widens the slip curve
                                    // (determine the min speed to reach max slip)

    public bool shouldTilt = true;

    #endregion

    #region Intermediate
    Rigidbody rigidBody;
    Bounds groupCollider;
    float distToGround;

    // The actual value to be used (modification of parameters)
    float rotate;
    float accel;
    float gripX;
    float gripZ;
    float rotVel;
    float slip;     // The value used based on Slip curves

    // For determining drag direction
    float isRight = 1.0f;
    float isForward = 1.0f;

    bool isRotating = false;
    bool isGrounded = true;
    bool isStumbling = false;

    bool isBoosting = false;

    bool isJumping = false;

    // Control signals
    float inThrottle = 0f;
    [HideInInspector] public float inTurn = 0f;
    bool inSlip = false;

    bool hasParticles = false;

    Vector3 spawnP;
    Quaternion spawnR;

    Vector3 vel = new Vector3(0f, 0f, 0f);
    Vector3 pvel = new Vector3(0f, 0f, 0f);
    #endregion

    GameObject VehicleModel;

    GameObject TurbineModel;

    GameObject JumperModel;

    Vector3 initialPosition;

    Quaternion initialRotation;

    float initialRigidBodyMass;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Store start location & rotation
        spawnP = transform.position;
        spawnR = transform.rotation;

        groupCollider = GetBounds(gameObject);     // Get the full collider boundary of group
        distToGround = groupCollider.extents.y;    // Pivot to the outermost collider

        // Move the CoM to a fraction of colliders boundaries
        rigidBody.centerOfMass = Vector3.Scale(groupCollider.extents, CoM);

        //distToGround = transform.position.y + 1f;

        VehicleModel = transform.Find("Model").gameObject;
        TurbineModel = transform.Find("Turbines").gameObject;
        JumperModel = transform.Find("Jumper").gameObject;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialRigidBodyMass = rigidBody.mass;
    }

    // Called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, rigidBody.velocity / 2, Color.green);

        // Reset to spawn if out of bounds
        if (transform.position.y < -10)
        {
            transform.position = spawnP;
            transform.rotation = spawnR;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rigidBody.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivateTurbine();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ActivateJumper();
        }
    }

    void ActivateTurbine()
    {
        TurbineModel.SetActive(true);
    }

    void ActivateJumper()
    {
        JumperModel.SetActive(true);
    }

    // Called once multiple times per frame 
    // (according to physics setting)
    void FixedUpdate()
    {

        if (transform.position.y < 14.7)
        {
            resetPlayer();
        }
        else if (transform.position.y > GravityIncreaseHeight)
        {
            // Increase mass of object if it surpasses the intended height
            rigidBody.mass = initialRigidBodyMass + ((transform.position.y - GravityIncreaseHeight) * (initialRigidBodyMass));
        }
        else if (rigidBody.mass > initialRigidBodyMass)
        {
            rigidBody.mass = initialRigidBodyMass;
        }


        InputKeyboard();

        accel = Accel;
        rotate = Rotate;
        gripX = GripX;
        gripZ = GripZ;
        rotVel = RotVel;
        rigidBody.angularDrag = AngDragG;

        // Adjustment in slope
        accel = accel * Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        accel = accel > 0f ? accel : 0f;
        gripZ = gripZ * Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        gripZ = gripZ > 0f ? gripZ : 0f;
        gripX = gripX * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad);
        gripX = gripX > 0f ? gripX : 0f;

        // A short raycast to check below
        isGrounded = Physics.Raycast(transform.position, -transform.up, flyingGroundDistance);
        Debug.DrawRay(transform.position, -transform.up, Color.green, flyingGroundDistance);
        if (!isGrounded && !isBoosting)
        {
            rotate = 0f;
            accel = 0f;
            gripX = 0f;
            gripZ = 0f;
            rigidBody.angularDrag = AngDragA;
        }

        // Prevent the rotational input intervenes with physics angular velocity 
        isStumbling = rigidBody.angularVelocity.magnitude > 0.1f * Rotate * Time.deltaTime;
        if (isStumbling)
        {
            //rotate = 0f;
        }

        // Start turning only if there's velocity
        if (pvel.magnitude < MinRotSpd)
        {
            rotate = 0f;
        }
        else
        {
            rotate = pvel.magnitude / MaxRotSpd * rotate;
        }

        if (rotate > Rotate) rotate = Rotate;

        // Calculate grip based on sideway velocity in hysteresis curve
        if (!inSlip)
        {
            // Normal => slip
            slip = this.SlipL.Evaluate(Mathf.Abs(pvel.x) / SlipMod);
            if (slip == 1f) inSlip = true;
        }
        else
        {
            // Slip => Normal
            slip = this.SlipU.Evaluate(Mathf.Abs(pvel.x) / SlipMod);
            if (slip == 0f) inSlip = false;
        }


        //rotate *= (1f + 0.5f * slip);   // Overall rotation, (body + vector)
        rotate *= (1f - 0.3f * slip);   // Overall rotation, (body + vector)
        rotVel *= (1f - slip);          // The vector modifier (just vector)



        // Execute the commands
        Controller();   // pvel assigment in here

        // Get the local-axis velocity after rotation
        vel = transform.InverseTransformDirection(rigidBody.velocity);


        // Rotate the velocity vector
        // vel = pvel => Transfer all (full grip)
        if (isRotating)
        {
            vel = vel * (1 - rotVel) + pvel * rotVel; // Partial transfer
            //vel = vel.normalized * speed;
        }


        // Sideway grip
        isRight = vel.x > 0f ? 1f : -1f;
        vel.x -= isRight * gripX * Time.deltaTime;  // Accelerate in opposing direction
        if (vel.x * isRight < 0f) vel.x = 0f;       // Check if changed polarity


        // Straight grip
        isForward = vel.z > 0f ? 1f : -1f;
        vel.z -= isForward * gripZ * Time.deltaTime;
        if (vel.z * isForward < 0f) vel.z = 0f;


        // Enforce Top speed only when not boosting and not midair
        if (vel.z > TopSpeed && !isBoosting && isGrounded) vel.z = TopSpeed;
        if (vel.z > BoostTopSpeed && isBoosting) vel.z = BoostTopSpeed;
        else if (vel.z < -TopSpeed) vel.z = -TopSpeed;

        rigidBody.velocity = transform.TransformDirection(vel);

        // Prevent out of bounds boosting
        if (isBoosting && (transform.position.y < GravityIncreaseHeight))
        {
            Debug.Log("Boosting");
            rigidBody.AddForce(transform.forward * BoostFactor * accel, ForceMode.Impulse);
        }

        ParticleSystem[] boostParticleEmitters = TurbineModel.GetComponentsInChildren<ParticleSystem>();
        // Representative value to indicate state of all related emitters
        bool areBoostingParticlesActive = boostParticleEmitters[0].isPlaying;

        // Start at boost start
        if (isBoosting && !areBoostingParticlesActive)
        {
            foreach (ParticleSystem p in boostParticleEmitters)
            {
                p.Play();
            }
        }
        // Stop Particles when boost ended
        else if (!isBoosting && areBoostingParticlesActive)
        {
            foreach (ParticleSystem p in boostParticleEmitters)
            {
                p.Stop();
                p.Clear();
            }
        }



        if (isJumping && isGrounded)
        {
            Debug.Log("Jumping");
            rigidBody.AddForce(transform.up * JumpFactor * accel, ForceMode.Impulse);
        }

    }



    #region Controllers
    // Get input values from keyboard
    void InputKeyboard()
    {
        inThrottle = Input.GetAxisRaw("Vertical");
        inTurn = Input.GetAxisRaw("Horizontal");

        isBoosting = TurbineModel.activeSelf;
        if (isBoosting)
        {
            isBoosting = Input.GetAxisRaw("Fire3") > 0;
        }

        isJumping = JumperModel.activeSelf;
        if (isJumping)
        {
            Debug.Log("Checking " + Input.GetAxisRaw("Jump").ToString());
            isJumping = Input.GetAxisRaw("Jump") > 0;
        }
    }

    // Executing the queued inputs
    void Controller()
    {

        if (inThrottle > 0.5f || inThrottle < -0.5f)
        {
            rigidBody.velocity += transform.forward * inThrottle * accel * Time.deltaTime;
            gripZ = 0f;     // Remove straight grip if wheel is rotating

        }


        isRotating = false;

        // Get the local-axis velocity before new input (+x, +y, and +z = right, up, and forward)
        pvel = transform.InverseTransformDirection(rigidBody.velocity);

        // Turn statically
        if (inTurn > 0.5f || inTurn < -0.5f)
        {
            float dir = (pvel.z < 0) ? -1 : 1;    // To fix direction on reverse
            RotateGradConst(inTurn * dir);
            if (shouldTilt)
            {
                Transform vehicleTransform = VehicleModel.transform;
                float xAngle = vehicleTransform.transform.localEulerAngles.x;
                Debug.Log(xAngle);
                if (xAngle < maxTiltAngle || xAngle > (360 - maxTiltAngle))
                {
                    xAngle += (50f * inTurn * Time.deltaTime);
                }

                if (xAngle >= maxTiltAngle && xAngle < 180)
                {
                    xAngle = maxTiltAngle - 0.1f;
                }
                else if (xAngle <= (360 - maxTiltAngle) && xAngle > 180)
                {
                    xAngle = (360 - maxTiltAngle) + 0.1f;
                }
                rotateModel(Quaternion.Euler(xAngle, vehicleTransform.transform.localEulerAngles.y, vehicleTransform.transform.localEulerAngles.z));
            }
        }
        else
        {
            if (shouldTilt)
            {
                Transform vehicleTransform = VehicleModel.transform;
                float xAngle = vehicleTransform.transform.localEulerAngles.x;
                if (xAngle != 0)
                {
                    int direction = 1;
                    if (xAngle < 180)
                    {
                        direction = -1;
                    }
                    xAngle += 50f * Time.deltaTime * direction;

                    if (xAngle < 1.5 || xAngle > 358.5)
                    {
                        xAngle = 0;
                    }
                }
                rotateModel(Quaternion.Euler(xAngle, vehicleTransform.transform.localEulerAngles.y, vehicleTransform.transform.localEulerAngles.z));
            }
        }

        bool needsDriftParticles = Mathf.Abs(Vector3.Dot(transform.right, rigidBody.velocity)) > 5;
        // No drift particles while flying
        if (!isGrounded)
        {
            needsDriftParticles = false;
        }

        if (needsDriftParticles && !hasParticles)
        {
            ParticleSystem emmitter = transform.Find("DriftParticles").GetComponent<ParticleSystem>();

            emmitter.Play();
            hasParticles = true;
        }
        else if (!needsDriftParticles && hasParticles)
        {
            ParticleSystem emmitter = transform.Find("DriftParticles").GetComponent<ParticleSystem>();

            emmitter.Stop();
            hasParticles = false;
        }

    }
    #endregion



    #region Rotation Methods
    /* Advised to not read eulerAngles, only write: https://answers.unity.com/questions/462073/
     * As it turns out, the problem isn't there. */

    /* As is: Conflict with physical Y-axis rotation, must be disabled.
     * Current methods:
     * 1. Prevent rotational input when there's angular velocity.
     * 2. Significantly increase angular drag while grounded.
     * 3. Result: rotation responding to environment, responsive input, & natural stumbling.
     */

    Vector3 drot = new Vector3(0f, 0f, 0f);


    void RotateGradConst(float isCW)
    {
        // Delta = right(taget) - left(current)
        drot.y = isCW * rotate * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(drot.y, transform.up);
        isRotating = true;
    }

    #endregion



    #region Utilities

    // Get bound of a large 
    public static Bounds GetBounds(GameObject obj)
    {

        // Switch every collider to renderer for more accurate result
        Bounds bounds = new Bounds();
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        if (colliders.Length > 0)
        {

            //Find first enabled renderer to start encapsulate from it
            foreach (Collider collider in colliders)
            {

                if (collider.enabled)
                {
                    bounds = collider.bounds;
                    break;
                }
            }

            //Encapsulate (grow bounds to include another) for all collider
            foreach (Collider collider in colliders)
            {
                if (collider.enabled)
                {
                    bounds.Encapsulate(collider.bounds);
                }
            }
        }
        return bounds;
    }

    void resetPlayer()
    {
        Debug.Log("Good luck you are on your own");
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        rigidBody.velocity = new Vector3(0, 0, 0);
    }

    void rotateModel(Quaternion tilt)
    {
        VehicleModel.transform.localRotation = tilt;
        TurbineModel.transform.localRotation = tilt;
        JumperModel.transform.localRotation = tilt;
    }
    #endregion
}
