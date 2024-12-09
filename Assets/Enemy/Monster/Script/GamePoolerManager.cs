using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoolerManager : MonoBehaviour
{
    public BulletBase BulletPrefabs;
    private void Awake()
    {
        SetupPool();
    }

    private void SetupPool()
    {
        ObjectPooler.SetupPool(BulletPrefabs, 10, "Sword");
    }
}
