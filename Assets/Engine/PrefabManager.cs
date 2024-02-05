using EDBG.Engine.ResourceManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    private bool isInitialized = false;
    private Dictionary<Type, object> pools;
    private Dictionary<Type, GameObject> prefabDict;
    private Transform unactiveObjects;
    private readonly int initialPoolSize = 100;


    public PrefabManager(Transform unactiveObjects)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            this.unactiveObjects = unactiveObjects;
            pools = new Dictionary<Type, object>();
            prefabDict = new Dictionary<Type, GameObject>();
        }
    }

    private bool SetQueue<T>() where T : Component
    {
        if (pools.ContainsKey(typeof(T)))
            return false;

        PrefabPool<T> newPool = new PrefabPool<T>();
        pools.Add(typeof(T), newPool);

        return true;
    }

    private T CreatePrefabInstance<T>() where T : Component
    {
        GameObject prefab;
        if (prefabDict.TryGetValue(typeof(T), out prefab))
        {
            return GameObject.Instantiate(prefab).GetComponent<T>();
        }
        else
        {
            Debug.Log("Prefab not registered for type: " + typeof(T));
            return null;
        }
    }


    public void RegisterPrefab<T>(GameObject prefab) where T : Component
    {

        RegisterPrefab<T>(prefab, initialPoolSize);
    }

    public void RegisterPrefab<T>(GameObject prefab, int poolSize) where T : Component
    {

        if (!prefabDict.ContainsKey(typeof(T)))
        {
            prefabDict.Add(typeof(T), prefab);

            if (!pools.ContainsKey(typeof(T)))
            {
                SetQueue<T>();
                PrefabPool<T> prefabPool = (PrefabPool<T>)pools[typeof(T)];
                for (int i = 0; i < poolSize; i++)
                {
                    T obj = CreatePrefabInstance<T>();
                    obj.gameObject.SetActive(false);
                    obj.gameObject.transform.SetParent(unactiveObjects);
                    prefabPool.AddQueueObject(obj);
                }
            }
        }
        else
        {
            prefabDict[typeof(T)] = prefab;
        }
    }

    public T RetrievePoolObject<T>() where T : Component, new()
    {
        if (!pools.ContainsKey(typeof(T)))
            SetQueue<T>();

        PrefabPool<T> prefabPool = (PrefabPool<T>)pools[typeof(T)];
        T retrieval = prefabPool.RetrieveQueueObject();

        if (retrieval == null)
        {
            retrieval = CreatePrefabInstance<T>();
        }

        if (retrieval == null)
            Debug.Log("Can't retrieve object");
        retrieval.gameObject.SetActive(true);
        return retrieval;
    }

    public void ReturnPoolObject<T>(T obj) where T : Component
    {
        if (pools.ContainsKey(typeof(T)))
        {
            PrefabPool<T> prefabPool = (PrefabPool<T>)pools[typeof(T)];
            prefabPool.AddQueueObject(obj);
            obj.gameObject.transform.SetParent(unactiveObjects);
            obj.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Pool not found for type: " + typeof(T));
        }
    }
}