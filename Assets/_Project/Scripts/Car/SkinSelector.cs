using UnityEngine;
using System.Collections.Generic;

public class SkinSelector : MonoBehaviour
{
    [Header("Configuración de Jerarquía")]
    [SerializeField] private Transform skinsContainer;

    void Start()
    {
        InitializeSkin();
    }

    private void InitializeSkin()
    {
        if (skinsContainer == null)
        {
            skinsContainer = transform.Find("Skins");
            if (skinsContainer == null)
            {
                Debug.LogError($"[SkinSelector] No se encontró el contenedor 'Skins' en {gameObject.name}");
                return;
            }
        }

        List<GameObject> allSkins = new List<GameObject>();
        GameObject playerSkin = null;

        foreach (Transform child in skinsContainer)
        {
            GameObject skinObj = child.gameObject;
            skinObj.SetActive(false);

            allSkins.Add(skinObj);

            if (skinObj.name.Equals("Player", System.StringComparison.OrdinalIgnoreCase))
            {
                playerSkin = skinObj;
            }
        }

        if (gameObject.CompareTag("Player"))
        {
            AssignPlayerSkin(playerSkin);
        }
        else
        {
            AssignRandomSkin(allSkins, playerSkin);
        }
    }

    private void AssignPlayerSkin(GameObject playerSkin)
    {
        if (playerSkin != null)
        {
            playerSkin.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"[SkinSelector] El objeto {gameObject.name} es Player pero no tiene una skin llamada 'Player'.");
        }
    }

    private void AssignRandomSkin(List<GameObject> allSkins, GameObject playerSkin)
    {
        List<GameObject> npcOptions = allSkins.FindAll(s => s != playerSkin);

        if (npcOptions.Count > 0)
        {
            int randomIndex = Random.Range(0, npcOptions.Count);
            npcOptions[randomIndex].SetActive(true);
        }
    }
}