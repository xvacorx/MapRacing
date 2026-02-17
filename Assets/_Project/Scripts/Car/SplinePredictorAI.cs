using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class SplinePredictorAI : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float lookAheadDistance = 8f;

    private ArcadeCar car;

    public bool isMainCar = false;

    [Header("Lane Offset")]
    public float laneOffset = 0f;
    public float maxLaneOffset = 4f;

    [Header("Modifiers")]
    public bool chaosEnabled = false;
    public bool changeLineEnabled = false;
    public float chaosValue = 0.5f;
    public float changeLineChanceValue = 0.01f;
    private float currentChaosValue;
    private float currentChangeLineChanceValue;
    private float chaosSeed;

    void Start()
    {
        car = GetComponent<ArcadeCar>();
        if (splineContainer == null)
        {
            splineContainer = FindAnyObjectByType<SplineContainer>();
        }

        if (isMainCar)
        {
            laneOffset = 0f;
        }
        else
        {
            laneOffset = Random.Range(-maxLaneOffset, maxLaneOffset); // Se escoge un carril de forma aleatoria
        }

        chaosSeed = Random.Range(0f, 1000f);
    }

    void Update()
    {
        if (splineContainer == null) return;
        /*
        if (!car.isGrounded)
        {
            car.Drive(0f, 0f);
            return;
        }
        */

        if (car.isGrounded)
        {
            var spline = splineContainer.Spline;
            SplineUtility.GetNearestPoint(spline, splineContainer.transform.InverseTransformPoint(transform.position), out float3 nearestLocal, out float t);

            float length = spline.GetLength();
            float tOffset = lookAheadDistance / length;
            float targetT = (t + tOffset) % 1f;

            float3 tangent = spline.EvaluateTangent(targetT);
            float3 up = new float3(0, 1, 0);

            float3 normal = math.normalize(math.cross(up, tangent)); // Vector lateral (la normal a la tangente de la spline)

            if (chaosEnabled)
            {
                currentChaosValue = chaosValue;
            }
            else
            {
                currentChaosValue = 0;
            }

            float chaos = Mathf.Sin(Time.time * 2f + chaosSeed) * currentChaosValue; // Se agrega ruido en forma sinusoidal para generar 'caos' en el trayecto del carro

            if (changeLineEnabled)
            {
                currentChangeLineChanceValue = changeLineChanceValue;
            }
            else
            {
                currentChangeLineChanceValue = 0;
            }

            if (Random.value < currentChangeLineChanceValue)
            {
                laneOffset = Random.Range(-maxLaneOffset, maxLaneOffset); // Hay cierta probabilidad de que se recalcule el carril
            }


            float3 targetLocalPos = spline.EvaluatePosition(targetT);
            float3 offsetLocalPos = targetLocalPos + normal * (laneOffset + chaos);
            Vector3 targetWorldPos = splineContainer.transform.TransformPoint(offsetLocalPos);

            Vector3 relativeTarget = transform.InverseTransformPoint(targetWorldPos);

            float steerInput = relativeTarget.x / relativeTarget.magnitude;

            car.Drive(1f, steerInput);

            Debug.DrawLine(transform.position, targetWorldPos, Color.green);
        }

        
    }

    public void OnCarCollision(Collision collision)
    {
        Vector3 contactNormal = collision.contacts[0].normal; // Dirección del impacto

        float side = Vector3.Dot(transform.right, contactNormal); // Será positivo o negativo según la dirección del impacto

        if (side > 0)
        {
            laneOffset -= maxLaneOffset * 0.5f;
        }
        else
        {
            laneOffset += maxLaneOffset * 0.5f;
        }

        laneOffset = Mathf.Clamp(laneOffset, -maxLaneOffset, maxLaneOffset);

    }
}