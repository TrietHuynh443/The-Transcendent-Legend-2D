using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : UnitySingleton<GameDataManager>
{
    private SkillDataContainer _skillDataContainer;
    public void LoadSkillData()
    {
        if(_skillDataContainer == null)
        {
            _skillDataContainer = new SkillDataContainer();
        }

        _skillDataContainer.SetData(FileUtils.Read(ResourcesRoute.SkillsDataPath));
    }
    
    public SkillData GetData(object[] dataParams)
    {
        return null;
    }

}
