using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int size;
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<T> pool = new ();
    
    [SerializeField] private Transform parentTransform;

    private void Start()
    {
        if (parentTransform == null) parentTransform = transform;
        for (var i = 0; i < size; i++)
        {
            var clone = Instantiate(prefab, parentTransform);
            clone.SetActive(false);
            pool.Add(clone.GetComponent<T>());
        }
    }

    public T GetPooledObject()
    {
        foreach (var pooledObject in pool.Where(pooledObject => !pooledObject.gameObject.activeInHierarchy))
        {
            return pooledObject;
        }
        return AddToPool();
    }

    private T AddToPool()
    {
        var clone = Instantiate(prefab, parentTransform);
        clone.SetActive(false);
        var cloneScript = clone.GetComponent<T>();
        pool.Add(cloneScript);
        return cloneScript;
    }
}
