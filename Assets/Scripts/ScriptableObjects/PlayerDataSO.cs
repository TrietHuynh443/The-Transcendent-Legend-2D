using System.Collections.Generic;
using GameData.PlayerData;
using GameEvent;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerDataSO", fileName = "new PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    private PlayerStats _currentStats = new PlayerStats();
    private PlayerStats _previousStats;

    public PlayerStats CurrentStats => _currentStats;

    public PlayerStats OriginalStats { get; private set; }

    public void Init()
    {
        var playerOriginalData = GameDataManager.Instance.GetCurrentPlayerData();
        _currentStats = new PlayerStats
        {
            Attack = playerOriginalData.BaseAttack,
            Defense = playerOriginalData.BaseDefense,
            SpecialAttack = playerOriginalData.BaseSpecialAttack,
            SpecialDefense = playerOriginalData.BaseSpecialDefense,
            AttackSpeed = playerOriginalData.BaseAttackSpeed,
            Speed = playerOriginalData.BaseSpeed,
            CritRate = playerOriginalData.BaseCritRate,
            Health = playerOriginalData.BaseHealth,
            _ownedAchievements = new HashSet<int>(_currentStats._ownedAchievements)
        };

        OriginalStats = new PlayerStats
        {
            Attack = playerOriginalData.BaseAttack,
            Defense = playerOriginalData.BaseDefense,
            SpecialAttack = playerOriginalData.BaseSpecialAttack,
            SpecialDefense = playerOriginalData.BaseSpecialDefense,
            AttackSpeed = playerOriginalData.BaseAttackSpeed,
            Speed = playerOriginalData.BaseSpeed,
            CritRate = playerOriginalData.BaseCritRate,
            Health = playerOriginalData.BaseHealth,
        };
    }

    public void AddToCurrentPlayerData(
        float additionalAttack = 0,
        float additionalDefense = 0,
        float additionalSpecialAttack = 0,
        float additionalSpecialDefense = 0,
        float additionalAttackSpeed = 0,
        float additionalSpeed = 0,
        float additionalCrit = 0)
    {
        _currentStats.Attack += additionalAttack;
        _currentStats.Defense += additionalDefense;
        _currentStats.SpecialAttack += additionalSpecialAttack;
        _currentStats.SpecialDefense += additionalSpecialDefense;
        _currentStats.AttackSpeed += additionalAttackSpeed;
        _currentStats.Speed += additionalSpeed;
        _currentStats.CritRate += additionalCrit;
    }

    public void BackupCurrentPlayerData()
    {
        _previousStats = _currentStats.Clone();
    }

    public void RestoreBackupPlayerData()
    {
        if (_previousStats == null)
        {
            return;
        }

        _currentStats = _previousStats.Clone();
    }

    public void LoseHealth(float damage)
    {
        _currentStats.Health -= damage;
        Debug.Log($"Player: {_currentStats.Health}");
        if (_currentStats.Health < 0)
        {
            EventAggregator.RaiseEvent(new PlayerDieEvent
            {
                DieAnimationTime = 1f
            });
        }
    }
    
    public void AddAchievement(int achievement)
    {
        _currentStats._ownedAchievements.Add(achievement);
    }

    public bool HasAchievement(int achievement)
    {
        return _currentStats._ownedAchievements.Contains(achievement);
    }

    public void ClearAchievements()
    {
        _currentStats._ownedAchievements.Clear();
    }

    [System.Serializable]
    public class PlayerStats
    {
        public float Health;
        public float Attack;
        public float Defense;
        public float SpecialAttack;
        public float SpecialDefense;
        public float AttackSpeed;
        public float Speed;
        public float CritRate;

        // HashSet to store achievements by their int values
        internal HashSet<int> _ownedAchievements = new HashSet<int>();
        public IReadOnlyCollection<int> OwnedAchievements => _ownedAchievements;

        // Methods to manage achievements
        

        // Clone method for creating a deep copy
        public PlayerStats Clone()
        {
            var clonedStats = (PlayerStats)MemberwiseClone();
            clonedStats._ownedAchievements = new HashSet<int>(_ownedAchievements);
            return clonedStats;
        }
    }
}
