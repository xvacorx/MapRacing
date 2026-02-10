using UnityEngine;
using System.Collections.Generic;

public class VFXPool : MonoBehaviour
{
    public static VFXPool Instance { get; private set; }

    [System.Serializable]
    public struct PoolItem
    {
        public string name;
        public GameObject prefab;
    }

    public List<PoolItem> vfxLibrary;
    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject Spawn(string vfxName, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(vfxName))
        {
            PoolItem item = vfxLibrary.Find(x => x.name == vfxName);
            if (item.prefab == null)
            {
                Debug.LogWarning($"[VFXPool] No existe el VFX: {vfxName}");
                return null;
            }
            _poolDictionary.Add(vfxName, new Queue<GameObject>());
        }

        GameObject obj;

        if (_poolDictionary[vfxName].Count > 0 && !_poolDictionary[vfxName].Peek().activeInHierarchy)
        {
            obj = _poolDictionary[vfxName].Dequeue();
        }
        else
        {
            PoolItem item = vfxLibrary.Find(x => x.name == vfxName);
            obj = Instantiate(item.prefab, transform);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        _poolDictionary[vfxName].Enqueue(obj);

        return obj;
    }
}