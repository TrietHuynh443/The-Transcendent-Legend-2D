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
    /// <summary>
    /// If load by Resources.Load --> dont need AppPath;
    /// </summary>
    public static string MainThemePath => Instance._mainThemePath;
    public static string OnHitSfxPath => Instance._onHitSfx;

    public PlayerDataSO PlayerDataSO => _playerDataSO;
    public SceneSaveDataSO SceneSaveDataSO => _sceneSaveDataSO;

    [Header("Game Data Paths")]
    [SerializeField] private string _skillsDataPath;
    [SerializeField] private string _characterDataPath;
    [SerializeField] private string _enemiesDataPath;
    [SerializeField] private string _inputDataPath;
    [Header("Sound Paths")]
    [SerializeField] private string _mainThemePath;
    [SerializeField] private string _onHitSfx;

    [Header("Scriptable Objects")]
    [SerializeField] private PlayerDataSO _playerDataSO;
    [SerializeField] private SceneSaveDataSO _sceneSaveDataSO;
}
