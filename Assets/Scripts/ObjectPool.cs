using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefabToPool;
    public int amountToPool;
    public Transform parentTransform;
    public List<GameObject> pooledObjects;

    private void Start()
    {
        if (parentTransform == null) parentTransform = transform;
        pooledObjects = new List<GameObject>();
        for (var i = 0; i < amountToPool; i++)
        {
            var temp = Instantiate(prefabToPool, parentTransform);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (var pooledObject in pooledObjects.Where(pooledObject => !pooledObject.activeInHierarchy))
        {
            return pooledObject;
        }

        return AddToPool();
    }

    private GameObject AddToPool()
    {
        print("Insufficient " + prefabToPool + "in object pool, adding 1 more");
        var addedObject = Instantiate(prefabToPool, parentTransform);
        addedObject.SetActive(false);
        pooledObjects.Add(addedObject);
        return addedObject;
    }
}
