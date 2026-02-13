using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcadeCar : MonoBehaviour
{
    public float topSpeed = 20f;
    public float acceleration = 60f;
    public float turnSpeed = 150f;

    private Rigidbody rb;
    private float currentSteer;
    private float currentAccel;

    [Header("Randomize Stats")]
    public bool randomStats = false;

    public float minSpeed = 15f;
    public float maxSpeed = 50f;
    public float minAcceleration = 50f;
    public float maxAcceleration = 125f;
    public float minTurnSpeed = 120f;
    public float maxTurnSpeed = 300f;
    private void Start()
    {
        if (randomStats)
        {
            topSpeed = Random.Range(minSpeed, maxSpeed);
            acceleration = Random.Range(minAcceleration, maxAcceleration);
            turnSpeed = Random.Range(minTurnSpeed, maxTurnSpeed);
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 1f;
        rb.angularDamping = 2f;
    }

    public void Drive(float accel, float steer)
    {
        currentAccel = accel;
        currentSteer = steer;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < topSpeed)
        {
            rb.AddForce(transform.forward * currentAccel * acceleration, ForceMode.Acceleration);
        }

        if (rb.linearVelocity.magnitude > 0.5f)
        {
            float turn = currentSteer * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }

        Vector3 lateralVel = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);
        rb.AddForce(-lateralVel * 10f, ForceMode.Acceleration);

        if (rb.position.y < -20f)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Offroad"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            SplinePredictorAI ai = GetComponent<SplinePredictorAI>();
            if (ai != null)
            {
                ai.OnCarCollision(collision);
                Debug.Log("Choque");
            }
        }
    }
}