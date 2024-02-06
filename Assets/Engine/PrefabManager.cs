using EDBG.Engine.ResourceManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    private bool isInitialized = false;
    private Dictionary<string, PrefabPool> pools;
    private Dictionary<string, GameObject> prefabDict;
    private Transform unactiveObjects;
    private readonly int initialPoolSize = 100;


    public PrefabManager(Transform unactiveObjects)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            this.unactiveObjects = unactiveObjects;
            pools = new Dictionary<string, PrefabPool>();
            prefabDict = new Dictionary<string, GameObject>();
        }
    }

    private bool SetQueue(string resource)
    {
        if (pools.ContainsKey(resource))
            return false;

        PrefabPool newPool = new PrefabPool();
        pools.Add(resource, newPool);

        return true;
    }

    private GameObject CreatePrefabInstance(string resource)
    {
        GameObject prefab;
        if (prefabDict.TryGetValue(resource, out prefab))
        {
            return GameObject.Instantiate(prefab);
        }
        else
        {
            Debug.Log("Prefab not registered for type: " + resource);
            return null;
        }
    }


    public void RegisterPrefab(string resourceName, GameObject prefab)
    {

        RegisterPrefab(resourceName, prefab, initialPoolSize);
    }

    public void RegisterPrefab(string resourceName, GameObject prefab, int poolSize)
    {

        if (!prefabDict.ContainsKey(resourceName))
        {
            prefabDict.Add(resourceName, prefab);

            if (!pools.ContainsKey(resourceName))
            {
                SetQueue(resourceName);
                PrefabPool prefabPool = pools[resourceName];
                for (int i = 0; i < poolSize; i++)
                {
                    GameObject obj = CreatePrefabInstance(resourceName);
                    obj.gameObject.SetActive(false);
                    obj.gameObject.transform.SetParent(unactiveObjects);
                    prefabPool.AddQueueObject(obj);
                }
            }
        }
        else
        {
            prefabDict[resourceName] = prefab;
        }
    }

    public GameObject RetrievePoolObject(string resourceName)
    {
        if (!pools.ContainsKey(resourceName))
            SetQueue(resourceName);

        PrefabPool prefabPool = pools[resourceName];
        GameObject retrieval = prefabPool.RetrieveQueueObject();

        if (retrieval == null)
        {
            retrieval = CreatePrefabInstance(resourceName);
        }

        if (retrieval == null)
            Debug.Log("Can't retrieve object");
        retrieval.gameObject.SetActive(true);
        return retrieval;
    }

    public void ReturnPoolObject(string resourceName, GameObject obj)
    {
        if (pools.ContainsKey(resourceName))
        {
            PrefabPool prefabPool = pools[resourceName];
            prefabPool.AddQueueObject(obj);
            obj.gameObject.transform.SetParent(unactiveObjects);
            obj.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Pool not found for type: " + resourceName);
        }
    }

    public void LoadAndRegisterGameObject(string resourceName, int amount)
    {
        GameObject prefab = Resources.Load<GameObject>(resourceName);
        if(prefab != null)
        {
            RegisterPrefab(resourceName, prefab);
        }
    }
}