using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (IsDestroyed) return null;

                _instance = FindExistingInstance() ?? CreateNewInstance();
            }
            return _instance;
        }
    }

    /// <summary>
    ///     If no instance of the T MonoBehaviour exists, creates a new GameObject in the scene
    ///     and adds T to it.
    /// </summary>
    private static T CreateNewInstance()
    {
        var containerGO = new GameObject("__" + typeof(T).Name + " (Singleton)");
        return containerGO.AddComponent<T>();
    }

    private static T FindExistingInstance()
    {
        T[] instances = GameObject.FindObjectsOfType<T>();
        if (instances == null ||  instances.Length == 0)
        {
            return null;
        }
        return instances[0];
    }

    public static bool IsDestroyed { get; private set; }

    public static bool IsAwaked {  get; private set; }

    public static bool IsStarted { get; private set; }


    protected virtual void SingletonAwake() { }

    protected virtual void SingletonStarted() { }

    protected virtual void SingletonOnDestroy() { }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = (T)this;
        }

        if (!IsAwaked) { 
            SingletonAwake();
            IsAwaked = true;
        }
    }

    private void Start()
    {
        if (!IsStarted)
        {
            SingletonStarted();
            IsStarted = true;
        }
    }

    private void OnDestroy()
    {

        if (!IsDestroyed)
        {
            SingletonOnDestroy();
            IsDestroyed = true;
        }
    }


}
