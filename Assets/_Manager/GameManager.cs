using System.Collections;
using System.Collections.Generic;
using Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    private ResourcesRoute routes;
    private GameDataManager _gameDataManagerInstance;
    //Change to EventAggregate
    // private GameEventManager _gameEventManagerInstance;
    private SoundManager _soundManager;
    private LevelManager _levelManager;
    [SerializeField] private GameObject _testEnemyPrefab;
    private Dictionary<EnemyType, BaseEnemy> _enemyMap;

    private Vector3 _playerCheckpointLocation;

    protected override void SingletonAwake()
    {
        routes = ResourcesRoute.Instance;
        _gameDataManagerInstance = GameDataManager.Instance;
        _soundManager = SoundManager.Instance;
        _levelManager = LevelManager.Instance;
        PlayerPrefs.SetInt("IsPlayerInit", -1);
        _enemyMap = new Dictionary<EnemyType, BaseEnemy>();
        DontDestroyOnLoad(this);
    }

    protected override void SingletonStarted()
    {
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

    public void SwitchScene(string sceneName, string switchName, bool isSwitchingLeftToRight)
    {
        _levelManager.SwitchScene(sceneName, switchName, isSwitchingLeftToRight);
    }

    public bool IsSwitchingLeftToRight()
    {
        return _levelManager.IsSwitchingLeftToRight;
    }

    public string GetLevelSwitchName()
    {
        return _levelManager.LevelSwitchName;
    }
    public void StartLevelSwitch()
    {
        _levelManager.StartLevelSwitch();
    }
    public void EndLevelSwitch()
    {
        _levelManager.EndLevelSwitch();
    }

    public bool IsLevelSwitching()
    {
        return _levelManager.IsSwitching;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

}
