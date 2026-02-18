using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject minePrefab;
    public Transform playerCar;

    [Header("Settings")]
    public float spawnYOffset = 0.1f;
    public float spawnZOffset = 2.5f;

    public void SpawnMine()
    {
        if (minePrefab == null || playerCar == null)
        {
            Debug.LogWarning("Falta asignar referencias a MineSpawner");
            return;
        }

        Vector3 spawnPos = playerCar.position - playerCar.forward * spawnZOffset;
        spawnPos.y += spawnYOffset;
        

        Instantiate(minePrefab, spawnPos, Quaternion.identity);
    }
  
}
