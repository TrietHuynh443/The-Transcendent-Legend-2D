using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameDataType
{
    SKILL = 0,
    DEFAULT = -1,
    PLAYER = 1,
}
public abstract class GameDataContainer
{
    private GameDataType gameDataType;
    public GameDataContainer(GameDataType gameDataType)
    {
        this.gameDataType = gameDataType;
    }

    protected void SetType(GameDataType gameDataType)
    {
        this.gameDataType = gameDataType;
    }

    public abstract void SetData(object rawData);
    public abstract List<BaseData> GetData(DataFilterParams @params);

    protected bool IsMatchDataType(GameDataType gameDataType) { 
        return gameDataType == this.gameDataType;
    }

    
}
