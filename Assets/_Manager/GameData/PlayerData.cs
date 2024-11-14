using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GameData.PlayerData
{
    [Serializable]
    public class PlayerData 
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Base Attack")]
        public float BaseAttack { get; set; } = 0;
        [JsonProperty("Base Defense")]
        public float BaseDefense { get; set; } = 0;
        [JsonProperty("Base Special Attack")]
        public float BaseSpecialAttack { get; set; } = 0;
        [JsonProperty("Base Special Defense")]
        public float BaseSpecialDefense { get; set; } = 0;
        [JsonProperty("Base Speed")]
        public float BaseSpeed { get; set; } = 0;
        [JsonProperty("Base Attack Speed")]
        public float BaseAttackSpeed { get; set; }  = 0;
        [JsonProperty("Base Crit Rate")]
        public float BaseCritRate { get; set; } = 0;
        [JsonProperty("Base Crit Scale")]
        public float BaseCriteScale { get; set; } = 0;
    }

    public class PlayerDataContainer : GameDataContainer
    {
        private PlayerData[] _playersData;
        
        public PlayerDataContainer(GameDataType gameDataType) : base(gameDataType)
        {
        }

        public override List<BaseData> GetData(DataFilterParams @params)
        {
            if (!IsMatchDataType(@params.Type)) 
            {
                return null;
            }
            List<PlayerData> res = new();

            if(@params.Id != DataFilterParams.DEFAULT_ID && @params.Name != DataFilterParams.DEFAULT_NAME)
            {
                res = _playersData.Where(o => o.Id == @params.Id && o.Name == @params.Name).ToList();
            }
            else if (@params.Id != DataFilterParams.DEFAULT_ID) { 
                res = _playersData.Where(o=>o.Id == @params.Id).ToList();
            }
            else if (@params.Name != DataFilterParams.DEFAULT_NAME) { 
                res = _playersData.Where(o=>o.Name == @params.Name).ToList();
            }
            else
            {
                res = _playersData.ToList();
            }

            return res.Cast<BaseData>().ToList();
        }

        public override void SetData(object rawData)
        {
            _playersData = JsonConvert.DeserializeObject<PlayerData[]>((string)rawData);
            foreach(var playerData in _playersData) {
                Debug.Log(JsonConvert.SerializeObject(playerData));
            }
        }
    }
}

