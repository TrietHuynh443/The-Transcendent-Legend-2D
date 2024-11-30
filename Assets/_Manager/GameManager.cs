using System.Collections;
using System.Collections.Generic;
using Factory;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    private ResourcesRoute routes;
    private GameDataManager _gameDataManagerInstance;
    //Change to EventAggregate
    // private GameEventManager _gameEventManagerInstance;
    private SoundManager _soundManager;
    [SerializeField] private GameObject _testEnemyPrefab;
    private Dictionary<EnemyType, BaseEnemy> _enemyMap;

    private Vector3 _playerCheckpointLocation;

    protected override void SingletonAwake()
    {
        PlayerPrefs.SetInt("IsPlayerInit", -1);


        _enemyMap = new Dictionary<EnemyType, BaseEnemy>();
        DontDestroyOnLoad(this);
    }

    protected override void SingletonStarted()
    {
        _gameDataManagerInstance = GameDataManager.Instance;
        routes = GetComponentInChildren<ResourcesRoute>();
        _soundManager = GetComponentInChildren<SoundManager>();
        _gameDataManagerInstance.gameObject.transform.SetParent(transform);
        _soundManager.gameObject.transform.SetParent(transform);
        //Play Main Theme
        StartCoroutine(PlayMainThemeMusic());
    }


    private IEnumerator PlayMainThemeMusic()
{
    var loadOperation = Resources.LoadAsync<AudioClip>(ResourcesRoute.MainThemePath);

    yield return loadOperation;

    if (loadOperation.asset is AudioClip clip)
    {
        _soundManager.PlayMusic(clip, true);
    }
    else
    {
        Debug.LogError($"Failed to load AudioClip from path: {ResourcesRoute.MainThemePath}");
    }
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
