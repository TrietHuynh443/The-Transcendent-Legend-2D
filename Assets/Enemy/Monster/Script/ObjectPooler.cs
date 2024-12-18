using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooler
{
    public static Dictionary<string, Component> poolLookup = new Dictionary<string, Component>();
    public static Dictionary<string, Queue<Component>> poolDictionary = new Dictionary<string, Queue<Component>>();

    public static void EnqueueObject<T>(T item, string name) where T : Component
    {
        if (!item.gameObject.activeSelf) { return; }

        item.transform.position = Vector2.zero;
        poolDictionary[name].Enqueue(item);
        item.gameObject.SetActive(false);
    }

    public static T DequeueObject<T>(string key) where T : Component
    {
        if (poolDictionary[key].TryDequeue(out var item))
        {
            return (T)item;
        }   
        return (T)EnqueueNewInstance(poolLookup[key], key);
        //return (T)poolDictionary[key].Dequeue();
    }

    public static T EnqueueNewInstance<T>(T item, string name) where T : Component
    {
        T newInstance = Object.Instantiate(item);
        newInstance.gameObject.SetActive(false);
        newInstance.transform.position = Vector2.zero;
        poolDictionary[name].Enqueue(newInstance);
        return newInstance;
    }

    public static void SetupPool<T>(T poolItemPrefab, int poolSize, string dictionaryEntry) where T : Component
    {
        if(!poolDictionary.TryAdd(dictionaryEntry, new Queue<Component>()) ||
        !poolLookup.TryAdd(dictionaryEntry, poolItemPrefab))
        {
            //already added
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            T poolInstance = Object.Instantiate(poolItemPrefab);
            poolInstance.gameObject.SetActive(false);
            poolDictionary[dictionaryEntry].Enqueue(poolInstance);
        }
    } 
}
