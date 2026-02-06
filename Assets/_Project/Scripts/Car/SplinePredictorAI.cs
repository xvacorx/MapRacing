using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplinePredictorAI : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float lookAheadDistance = 8f; // Distancia de la "zanahoria" que el auto persigue

    private ArcadeCar car;

    void Start() => car = GetComponent<ArcadeCar>();

    void Update()
    {
        if (splineContainer == null) return;

        // 1. Encontrar dónde estamos en la Spline (Espacio Local)
        var spline = splineContainer.Spline;
        SplineUtility.GetNearestPoint(spline, splineContainer.transform.InverseTransformPoint(transform.position), out float3 nearestLocal, out float t);

        // 2. Calcular el punto objetivo adelante (la "Zanahoria")
        float length = spline.GetLength();
        float tOffset = lookAheadDistance / length;
        float targetT = (t + tOffset) % 1f;

        // 3. Convertir punto de la Spline a posición de MUNDO
        float3 targetLocalPos = spline.EvaluatePosition(targetT);
        Vector3 targetWorldPos = splineContainer.transform.TransformPoint(targetLocalPos);

        // 4. Calcular el ángulo hacia ese punto
        Vector3 relativeTarget = transform.InverseTransformPoint(targetWorldPos);

        // Si el punto está a la derecha, el ángulo es positivo; a la izquierda, negativo.
        float steerInput = relativeTarget.x / relativeTarget.magnitude;

        // 5. Enviar al controlador
        car.Drive(1f, steerInput);

        // Debug para ver la línea que el auto intenta seguir
        Debug.DrawLine(transform.position, targetWorldPos, Color.green);
    }
}