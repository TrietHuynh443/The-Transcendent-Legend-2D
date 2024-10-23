using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDataType{
    SKILL = 0,
}
public abstract class GameData
{
    public abstract void SetData(object rawData);
}
