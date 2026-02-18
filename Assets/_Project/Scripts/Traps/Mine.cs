using UnityEngine;

public class MineTrap : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 1500f;
    [SerializeField] private float upwardModifier = 1.5f;

    [Header("Visuals")]
    [SerializeField] private string vfxName = "MineExplosion";

    private bool _hasExploded = false;

    private void OnEnable()
    {
        _hasExploded = false;
    }

    public void Trigger()
    {
        if (_hasExploded) return;
        Explode();
    }

    private void Explode()
    {
        _hasExploded = true;

        if (VFXPool.Instance != null)
        {
            VFXPool.Instance.Spawn(vfxName, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
                
                rb.AddTorque(Random.insideUnitSphere * 1000f, ForceMode.Impulse); // prueba para ver que los autos giren en el aire

                Vector3 torque = rb.transform.right * 1600f;
                rb.AddTorque(torque, ForceMode.Impulse);

                Vector3 torque2 = rb.transform.right * 800f;
                rb.AddTorque(torque2, ForceMode.Impulse);

            }
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            Trigger();
        }
    }
}