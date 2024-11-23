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

    protected override void SingletonAwake()
    {
        routes = ResourcesRoute.Instance;
        _gameDataManagerInstance = GameDataManager.Instance;
        _soundManager = SoundManager.Instance;
        PlayerPrefs.SetInt("IsPlayerInit", -1);
        _enemyMap = new Dictionary<EnemyType, BaseEnemy>();
        DontDestroyOnLoad(this);
    }

    protected override void SingletonStarted()
    {
        _gameDataManagerInstance.gameObject.transform.SetParent(transform);
        _soundManager.gameObject.transform.SetParent(transform);
        _gameDataManagerInstance.LoadAllData();

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
}
