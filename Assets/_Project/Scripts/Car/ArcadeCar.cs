using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcadeCar : MonoBehaviour
{
    public float topSpeed = 20f;
    public float acceleration = 60f;
    public float turnSpeed = 150f; // Aumentado para respuesta arcade

    private Rigidbody rb;
    private float currentSteer;
    private float currentAccel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        // Importante: Unity 6 usa linearDamping en lugar de drag
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
        // 1. Aceleración constante
        if (rb.linearVelocity.magnitude < topSpeed)
        {
            rb.AddForce(transform.forward * currentAccel * acceleration, ForceMode.Acceleration);
        }

        // 2. Giro Arcade (Rotación directa para evitar que el cubo sea torpe)
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            float turn = currentSteer * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }

        // 3. Simulación de agarre (Evita que el cubo se deslice de lado)
        Vector3 lateralVel = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);
        rb.AddForce(-lateralVel * 10f, ForceMode.Acceleration);
    }
}