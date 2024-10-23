using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager> 
{
    private ResourcesRoute routes;
    private GameDataManager _gameDataManagerInstance;

    protected override void SingletonAwake()
    {
        routes = ResourcesRoute.Instance;
        _gameDataManagerInstance = GameDataManager.Instance;
    }
    protected override void SingletonStarted()
    {

        _gameDataManagerInstance.gameObject.transform.SetParent(transform);

        _gameDataManagerInstance.LoadSkillData();
    }

}
