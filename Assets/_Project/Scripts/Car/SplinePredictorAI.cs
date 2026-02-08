using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplinePredictorAI : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float lookAheadDistance = 8f;

    private ArcadeCar car;

    void Start()
    {
        car = GetComponent<ArcadeCar>();
        if (splineContainer == null)
        {
            splineContainer = FindObjectOfType<SplineContainer>();
        }
    }

    void Update()
    {
        if (splineContainer == null) return;

        var spline = splineContainer.Spline;
        SplineUtility.GetNearestPoint(spline, splineContainer.transform.InverseTransformPoint(transform.position), out float3 nearestLocal, out float t);

        float length = spline.GetLength();
        float tOffset = lookAheadDistance / length;
        float targetT = (t + tOffset) % 1f;

        float3 targetLocalPos = spline.EvaluatePosition(targetT);
        Vector3 targetWorldPos = splineContainer.transform.TransformPoint(targetLocalPos);

        Vector3 relativeTarget = transform.InverseTransformPoint(targetWorldPos);

        float steerInput = relativeTarget.x / relativeTarget.magnitude;

        car.Drive(1f, steerInput);

        Debug.DrawLine(transform.position, targetWorldPos, Color.green);
    }
}