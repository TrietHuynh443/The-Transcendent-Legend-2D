using System.Collections;
using System.Collections.Generic;
using Factory;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    private ResourcesRoute routes;
    private GameDataManager _gameDataManagerInstance;
    private GameEventManager _gameEventManagerInstance;

    [SerializeField] private GameObject _testEnemyPrefab;
    private Dictionary<EnemyType, BaseEnemy> _enemyMap;

    private Vector3 _playerCheckpointLocation;

    protected override void SingletonAwake()
    {
        routes = ResourcesRoute.Instance;
        _gameDataManagerInstance = GameDataManager.Instance;
        _gameEventManagerInstance = GameEventManager.Instance;
        PlayerPrefs.SetInt("IsPlayerInit", -1);

        _enemyMap = new Dictionary<EnemyType, BaseEnemy>();
        DontDestroyOnLoad(this);
    }

    protected override void SingletonStarted()
    {
        _gameDataManagerInstance.gameObject.transform.SetParent(transform);
        _gameEventManagerInstance.gameObject.transform.SetParent(transform);

        _gameDataManagerInstance.LoadAllData();
    }

    public GameObject SpawnEnemy(EnemyType type, Vector3 pos)
    {
        if (!_enemyMap.ContainsKey(type))
        {
            switch (type)
            {
                case EnemyType.Test:
                    var enemy = EnemyFactory.CreateEnemy(type, _testEnemyPrefab);
                    _enemyMap.Add(type, enemy);
                    break;
            }
        }

        return Instantiate(_enemyMap[type].GetEnemyPrefab(), pos, Quaternion.identity);
    }

    public void SetPlayerRespawnLocation(Vector3 pos)
    {
        // _gameDataManagerInstance.SetField("Position", pos);

        _playerCheckpointLocation = pos;
    }

    public void RespawnPlayer(GameObject _playerObject)
    {
        Debug.Log(_playerCheckpointLocation);
        _playerObject.transform.position = _playerCheckpointLocation;
    }
}
