using System.Collections;
using System.Collections.Generic;
using Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>, IGameEventListener<StartGameEvent>
{
    private ResourcesRoute routes;

    private GameDataManager _gameDataManagerInstance;

    //Change to EventAggregate
    // private GameEventManager _gameEventManagerInstance;
    private SoundManager _soundManager;
    private LevelManager _levelManager;
    [SerializeField] private GameObject _testEnemyPrefab;
    private Dictionary<EnemyType, BaseEnemy> _enemyMap;

    private SceneSaveDataSO playerQuickRespawnData;
    
    public SceneSaveDataSO PlayerQuickRespawnData => playerQuickRespawnData;

    protected override void SingletonAwake()
    {
        // PlayerPrefs.SetInt("IsPlayerInit", -1);
        _levelManager = LevelManager.Instance;
        _enemyMap = new Dictionary<EnemyType, BaseEnemy>();
        DontDestroyOnLoad(this);
        routes = GetComponentInChildren<ResourcesRoute>();
        routes.PlayerDataSO.Init();

    }

    protected override void SingletonStarted()
    {
        _gameDataManagerInstance = GameDataManager.Instance;
        _soundManager = GetComponentInChildren<SoundManager>();
        _gameDataManagerInstance.gameObject.transform.SetParent(transform);
        _soundManager.gameObject.transform.SetParent(transform);
        EventAggregator.Register<StartGameEvent>(this);
        //Play Main Theme
        StartCoroutine(PlayMainThemeMusic());
    }

    protected override void SingletonOnDestroy()
    {
        base.SingletonOnDestroy();
        EventAggregator.Unregister<StartGameEvent>(this);
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

    public void SetPlayerQuickRespawnLocation(Vector3 pos)
    {
        // _gameDataManagerInstance.SetField("Position", pos);

        playerQuickRespawnData = ScriptableObject.CreateInstance<SceneSaveDataSO>();
        playerQuickRespawnData.SceneName = SceneManager.GetActiveScene().name;
        playerQuickRespawnData.CheckPointPos = pos;
    }

    public void RespawnPlayer(SceneSaveDataSO checkpointData, PlayerController playerController)
    {
        playerController.gameObject.transform.position = checkpointData.CheckPointPos;
    }

    public void SwitchScene(string sceneName, string switchName, Vector2 velocity, bool isVerticalSwitch)
    {
        _levelManager.SwitchScene(sceneName, switchName, velocity, isVerticalSwitch);
    }

    public Vector2 GetSwitchVelocity()
    {
        return _levelManager.Velocity;
    }

    public bool IsVerticalSwitch()
    {
        return _levelManager.IsVerticalSwitch;
    }

    public bool IsLevelSwitching()
    {
        return _levelManager.IsSwitching;
    }

    public void FinishLevelSwitch()
    {
        _levelManager.FinishSceneSwitch();
    }

    public string GetLevelSwitchName()
    {
        return _levelManager.LevelSwitchName;
    }

    public void Handle(StartGameEvent @event)
    {
        string name = "";
        if (@event.Level == 1)
        {
            name = "Cemetery Graveyard";
        }

        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}