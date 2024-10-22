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
public class SkillData
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


public class SkillDataContainer : GameData
{
    private SkillData[] _allSkillDatas;
    private Dictionary<string, SkillData> _skillDictionary;
    public override void SetData(object rawData)
    {
        Debug.Log((string)rawData);
        _allSkillDatas = JsonConvert.DeserializeObject<SkillData[]>((string)rawData);
        _skillDictionary = _allSkillDatas
                                .Where(o => o.Name != null) // Filter out nulls
                                .ToDictionary(o => o.Name, o => o); ;
        //_data = JsonUtility.FromJson<SkillData>(rawData.ToString());
    }

    public SkillData GetSkill(string name)
    {
        _skillDictionary.TryGetValue(name, out SkillData skillData);

        return skillData;
    }
}
