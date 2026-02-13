using UnityEngine;

public class VFXController : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private string explosionVFXName = "Explosion";

    [Header("Status")]
    public bool spawnEffectOnDisable = true;

    private void OnDisable()
    {
        if (!spawnEffectOnDisable || VFXPool.Instance == null) return;

        VFXPool.Instance.Spawn(explosionVFXName, transform.position, Quaternion.identity);
    }

    public void PlayExplosion()
    {
        if (VFXPool.Instance != null)
        {
            VFXPool.Instance.Spawn(explosionVFXName, transform.position, Quaternion.identity);
        }
    }
}