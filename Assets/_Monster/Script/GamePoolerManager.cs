using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoolerManager : MonoBehaviour
{
    public Bullet bulletPrefabs;
    private void Awake()
    {
        SetupPool();
    }

    private void SetupPool()
    {
        ObjectPooler.SetupPool(bulletPrefabs, 10, "Bullet");
    }
}
