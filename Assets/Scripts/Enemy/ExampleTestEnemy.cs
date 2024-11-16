using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleTestEnemy : BaseEnemy
{
    [SerializeField] private GameObject prefab;
    public override GameObject GetEnemyPrefab()
    {
        return prefab;
    }

    public override void SetEnemyPrefab(GameObject enemyPrefab)
    {
        prefab = enemyPrefab;
    }

}
