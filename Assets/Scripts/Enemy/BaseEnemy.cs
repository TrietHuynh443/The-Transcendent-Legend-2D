using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy: MonoBehaviour
{
    public abstract GameObject GetEnemyPrefab();
    public abstract void SetEnemyPrefab(GameObject enemyPrefabs);
}
