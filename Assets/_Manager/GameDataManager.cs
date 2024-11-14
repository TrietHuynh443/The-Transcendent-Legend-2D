using System;
using System.Collections;
using System.Collections.Generic;
using GameData.PlayerData;
using UnityEngine;

public class GameDataManager : UnitySingleton<GameDataManager>
{
    private SkillDataContainer _skillDataContainer;
    private PlayerDataContainer _playerDataContainer;
    private Dictionary<GameDataType, GameDataContainer> _gameDataContainers = new Dictionary<GameDataType, GameDataContainer>();
    
    private void LoadSkillData()
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
        else if(type == GameDataType.PLAYER) return _playerDataContainer.GetData(@params);
        return null;
    }

    public void LoadAllData()
    {
        LoadSkillData();
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        if(_playerDataContainer == null)
        {
            _playerDataContainer = new PlayerDataContainer(GameDataType.PLAYER);
        }

        _playerDataContainer.SetData(FileUtils.Read(ResourcesRoute.CharacterDataPath));

        if (!_gameDataContainers.ContainsKey(GameDataType.PLAYER))
        {
            _gameDataContainers.Add(GameDataType.PLAYER, _skillDataContainer);
        }
        else
        {
            _gameDataContainers[GameDataType.PLAYER] = _skillDataContainer;
        }
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
