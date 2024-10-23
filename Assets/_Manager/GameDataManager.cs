using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : UnitySingleton<GameDataManager>
{
    private SkillDataContainer _skillDataContainer;
    private Dictionary<GameDataType, GameDataContainer> _gameDataContainers = new Dictionary<GameDataType, GameDataContainer>();
    
    public void LoadSkillData()
    {
        if(_skillDataContainer == null)
        {
            _skillDataContainer = new SkillDataContainer(GameDataType.SKILL);
        }

        _skillDataContainer.SetData(FileUtils.Read(ResourcesRoute.SkillsDataPath));

        if (!_gameDataContainers.ContainsKey(GameDataType.SKILL))
        {
            _gameDataContainers.Add(GameDataType.SKILL, _skillDataContainer);
        }
        else
        {
            _gameDataContainers[GameDataType.SKILL] = _skillDataContainer;
        }
    }
    
    public List<BaseData> GetData(DataFilterParams @params)
    {
        GameDataType type = @params.Type;
        if (type == GameDataType.SKILL) return _skillDataContainer.GetData(@params);

        return null;
    }
}

public class DataFilterParams
{
    public GameDataType Type { get; set; } = GameDataType.DEFAULT;
    public string Name { get; set; } = DEFAULT_NAME;
    public int Id { get; set; } = DEFAULT_ID;

    public static string DEFAULT_NAME = "DEFAULT NAME";
    public static int DEFAULT_ID = -1;
}
