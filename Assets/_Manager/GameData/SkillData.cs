using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public enum SkillType
{

}

[Serializable]
public class SkillData : BaseData
{
    public int Id;
    public string Name;
    public string Description;
    public string Status;
    public float? Damage;
    public float? SeftDamage;
    public float? Cooldown;
    public float? Consume;
    public string Type;
    public float? Range;
}


public class SkillDataContainer : GameDataContainer
{
    private SkillData[] _allSkillDatas;
    //private Dictionary<string, SkillData> _skillDictionary;

    public SkillDataContainer(GameDataType gameDataType) : base(gameDataType)
    {
        //SetType(GameDataType.SKILL);
    }

    public override void SetData(object rawData)
    {
        Debug.Log((string)rawData);
        _allSkillDatas = JsonConvert.DeserializeObject<SkillData[]>((string)rawData);

        //_data = JsonUtility.FromJson<SkillData>(rawData.ToString());
    }

    public override List<BaseData> GetData(DataFilterParams @params)
    {
        if (!IsMatchDataType(@params.Type)) {
            return null;
        }
        List<SkillData> res = new List<SkillData>();
        if(@params.Id != DataFilterParams.DEFAULT_ID && @params.Name != DataFilterParams.DEFAULT_NAME)
        {
            res = _allSkillDatas.Where(o => o.Id == @params.Id && o.Name == @params.Name).ToList();
        }
        else if (@params.Id != DataFilterParams.DEFAULT_ID) { 
            res = _allSkillDatas.Where(o=>o.Id == @params.Id).ToList();
        }
        else if (@params.Name != DataFilterParams.DEFAULT_NAME) { 
            res = _allSkillDatas.Where(o=>o.Name == @params.Name).ToList();
        }
        else
        {
            res = _allSkillDatas.ToList();
        }

        return res.Cast<BaseData>().ToList();
    }
}
