using EDBG.Engine.ResourceManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    private bool isInitialized = false;
    private readonly Dictionary<ActorTypeEnum, PrefabPool> pools;
    private readonly Dictionary<ActorTypeEnum, GameObject> prefabDict;
    private Transform unactiveObjects;
    private readonly int initialPoolSize = 100;


    public PrefabManager(Transform unactiveObjects)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            this.unactiveObjects = unactiveObjects;
            pools = new Dictionary<ActorTypeEnum, PrefabPool>();
            prefabDict = new Dictionary<ActorTypeEnum, GameObject>();
        }
    }

    private bool SetQueue(ActorTypeEnum type)
    {
        if (pools.ContainsKey(type))
            return false;

        PrefabPool newPool = new PrefabPool();
        pools.Add(type, newPool);

        return true;
    }

    private GameObject CreatePrefabInstance(ActorTypeEnum type)
    {
        GameObject prefab;
        if (prefabDict.TryGetValue(type, out prefab))
        {
            return GameObject.Instantiate(prefab);
        }
        else
        {
            Debug.Log("Prefab not registered for type: " + type);
            return null;
        }
    }


    public void RegisterPrefab(ActorTypeEnum type, GameObject prefab)
    {

        RegisterPrefab(type, prefab, initialPoolSize);
    }

    public void RegisterPrefab(ActorTypeEnum type, GameObject prefab, int poolSize)
    {

        if (!prefabDict.ContainsKey(type))
        {
            prefabDict.Add(type, prefab);

            if (!pools.ContainsKey(type))
            {
                SetQueue(type);
                PrefabPool prefabPool = pools[type];
                for (int i = 0; i < poolSize; i++)
                {
                    GameObject obj = CreatePrefabInstance(type);
                    obj.gameObject.SetActive(false);
                    obj.gameObject.transform.SetParent(unactiveObjects);
                    prefabPool.AddQueueObject(obj);
                }
            }
        }
        else
        {
            prefabDict[type] = prefab;
        }
    }

    public GameObject RetrievePoolObject(ActorTypeEnum type)
    {
        if (!pools.ContainsKey(type))
            SetQueue(type);

        PrefabPool prefabPool = pools[type];
        GameObject retrieval = prefabPool.RetrieveQueueObject();

        if (retrieval == null)
        {
            retrieval = CreatePrefabInstance(type);
        }

        if (retrieval == null)
            Debug.Log("Can't retrieve object: " + type);
        retrieval.gameObject.SetActive(true);
        return retrieval;
    }

    public void ReturnPoolObject(ActorTypeEnum type, GameObject obj)
    {
        if (pools.ContainsKey(type))
        {
            PrefabPool prefabPool = pools[type];
            prefabPool.AddQueueObject(obj);
            obj.gameObject.transform.SetParent(unactiveObjects);
            obj.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Pool not found for type: " + type);
        }
    }

    public void LoadAndRegisterGameObject(ActorTypeEnum type, int amount)
    {
        GameObject prefab = Resources.Load<GameObject>(type.ToString());
        if(prefab != null)
        {
            RegisterPrefab(type, prefab);
        }
    }
}