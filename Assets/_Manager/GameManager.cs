using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager> 
{
    private ResourcesRoute routes;
    private GameDataManager _gameDataManagerInstance;
    private GameEventManager _gameEventManagerInstance;

    protected override void SingletonAwake()
    {
        routes = ResourcesRoute.Instance;
        _gameDataManagerInstance = GameDataManager.Instance;
        _gameEventManagerInstance = GameEventManager.Instance;
    }
    protected override void SingletonStarted()
    {

        _gameDataManagerInstance.gameObject.transform.SetParent(transform);
        _gameEventManagerInstance.gameObject.transform.SetParent(transform);

        _gameDataManagerInstance.LoadAllData();
    }

}
