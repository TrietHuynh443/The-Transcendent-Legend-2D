using GameData.PlayerData;
using GameEvent;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerDataSO", fileName = "new PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    private PlayerStats _currentStats = new PlayerStats();
    private PlayerStats _previousStats;

    public void Init()
    {
        if (PlayerPrefs.GetInt("IsPlayerInit") > 0) return;

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
        };

        PlayerPrefs.SetInt("IsPlayerInit", 1);
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
        if(_currentStats.Health < 0)
        {
            EventAggregator.RaiseEvent<DeadEvent>(new DeadEvent());
        }
    }

    [System.Serializable]
    private class PlayerStats
    {
        public float Health;
        public float Attack;
        public float Defense;
        public float SpecialAttack;
        public float SpecialDefense;
        public float AttackSpeed;
        public float Speed;
        public float CritRate;

        // Clone method for creating a deep copy
        public PlayerStats Clone()
        {
            return (PlayerStats)MemberwiseClone();
        }
    }
}
