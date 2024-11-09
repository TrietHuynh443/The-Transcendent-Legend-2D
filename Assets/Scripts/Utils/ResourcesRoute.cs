using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcesRoute: UnitySingleton<ResourcesRoute>
{
    private static string AppPath = Application.dataPath + "/";
    public static string SkillsDataPath => AppPath + Instance._skillsDataPath;
    public static string CharacterDataPath =>AppPath + Instance._characterDataPath;
    public static string EnemiesDataPath => AppPath + Instance._enemiesDataPath;

    public static string InputDataPath => AppPath + Instance._inputDataPath;

    [SerializeField] private string _skillsDataPath;
    [SerializeField] private string _characterDataPath;
    [SerializeField] private string _enemiesDataPath;
    [SerializeField] private string _inputDataPath;

}
