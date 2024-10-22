using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager> 
{
    private ResourcesRoute routes;
    public static GameDataManager GameDataManagerInstance { get; private set; }

    protected override void SingletonAwake()
    {
        routes = ResourcesRoute.Instance;
    }
    protected override void SingletonStarted()
    {
        GameDataManagerInstance = GameDataManager.Instance;
        GameDataManagerInstance.gameObject.transform.SetParent(transform);

        GameDataManagerInstance.LoadSkillData();
    }

}
