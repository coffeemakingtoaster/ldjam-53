using UnityEngine;

public class HyperDrive : MonoBehaviour
{
    public float speed = 1000f; //Controls velocity multiplier
    public GameObject accelerationPoint;

    public GameObject driftParticlesLeft;
    public GameObject driftParticlesRight;

    public float maxSideWaysSpeed = 30f;

    public float maxSpeedForFullTurning = 10f;

    Rigidbody rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rb equals the rigidbody on the player
    }

    void FixedUpdate()
    {
        // Check if player is in the air
        bool is_midair = !Physics.Raycast(transform.position, -transform.up, 2);

        float Input_vertical = Input.GetAxis("Vertical");
        float Input_horizontal = Input.GetAxis("Horizontal");
        if (rb.velocity.magnitude < 10)
        {
            Input_horizontal = Input_horizontal * (rb.velocity.magnitude / maxSpeedForFullTurning);
        }
        if (Input_horizontal != 0 && !is_midair)
        {
            Debug.Log("Drifting");
            driftParticlesLeft.GetComponent<ParticleSystem>().Play();
            driftParticlesRight.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            driftParticlesLeft.GetComponent<ParticleSystem>().Pause();
            driftParticlesRight.GetComponent<ParticleSystem>().Pause();
            driftParticlesLeft.GetComponent<ParticleSystem>().Clear();
            driftParticlesRight.GetComponent<ParticleSystem>().Clear();
        }
        rb.MoveRotation(Quaternion.Euler(0, transform.rotation.eulerAngles.y + 2f * Input_horizontal, 0));

        Vector3 sideWaysModifier = new Vector3(0, 0, 0);

        float xVel = transform.InverseTransformDirection(rb.velocity).z;
        Debug.Log(xVel);
        if (xVel > maxSideWaysSpeed)
        {
            sideWaysModifier.z = 100f;
        }

        Debug.Log(transform.forward * Time.deltaTime * (Input_vertical * speed));

        // Only apply if player is on the ground
        if (!is_midair)
        {
            //rb.MovePosition(transform.position + transform.forward * Time.deltaTime * (Input_vertical * speed));
            //rb.AddForce(transform.forward * Time.deltaTime * (Input_vertical * speed));
            rb.AddForceAtPosition((transform.forward + transform.TransformVector(sideWaysModifier)) * Time.deltaTime * (Input_vertical * speed), accelerationPoint.transform.position);
        }
    }
}
