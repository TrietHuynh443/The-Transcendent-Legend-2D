using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public static class EnemyFactory
    {
        public static BaseEnemy CreateEnemy(EnemyType type, GameObject prefab)
        {
            BaseEnemy enemy = null;
            switch(type)
            {
                case EnemyType.Test:
                    enemy = new ExampleTestEnemy();
                    break;
            }
            enemy.SetEnemyPrefab(prefab);
            return enemy;
        }
    }

    public enum EnemyType
    {
        Villager = 0,
        SickMouse = 1,
        BossChapterOne = 2,
        BossChapterTwo = 3,
        Test = -1,
    }
}
