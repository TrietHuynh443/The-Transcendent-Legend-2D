using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcesRoute: UnitySingleton<ResourcesRoute>
{
    public static string SkillsDataPath => Instance._skillsDataPath;
    public static string CharacterDataPath =>Instance._characterDataPath;
    public static string EnemiesDataPath => Instance._enemiesDataPath;

    [SerializeField] private string _skillsDataPath;
    [SerializeField] private string _characterDataPath;
    [SerializeField] private string _enemiesDataPath;

}
