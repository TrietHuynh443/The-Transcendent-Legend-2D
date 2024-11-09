using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GameData.InputData{
    public class InputData : BaseData
    {
        public int Id;
        public string Name;
        public string Keycode;
    }

    public class InputDataContainer : GameDataContainer
    {
        private InputData[] _inputData;
        private Dictionary<string, KeyCode> _keysMap = new();
        public static readonly Dictionary<string, KeyCode> SpecialKeyCodes = new Dictionary<string, KeyCode>
        {
            { "Shift", KeyCode.LeftShift },
            { "RightShift", KeyCode.RightShift },
            { "Ctrl", KeyCode.LeftControl },
            { "RightCtrl", KeyCode.RightControl },
            { "Alt", KeyCode.LeftAlt },
            { "RightAlt", KeyCode.RightAlt },
            { "Tab", KeyCode.Tab },
            { "Space", KeyCode.Space },
            { "CapsLock", KeyCode.CapsLock },
            { "Escape", KeyCode.Escape },
            { "Enter", KeyCode.Return },
            { "Backspace", KeyCode.Backspace },
            { "Delete", KeyCode.Delete },
            { "Insert", KeyCode.Insert },
            { "Home", KeyCode.Home },
            { "End", KeyCode.End },
            { "PageUp", KeyCode.PageUp },
            { "PageDown", KeyCode.PageDown },
            { "UpArrow", KeyCode.UpArrow },
            { "DownArrow", KeyCode.DownArrow },
            { "LeftArrow", KeyCode.LeftArrow },
            { "RightArrow", KeyCode.RightArrow },
            { "NumLock", KeyCode.Numlock },
            { "PrintScreen", KeyCode.Print },
            { "ScrollLock", KeyCode.ScrollLock },
            { "Pause", KeyCode.Pause }
        };
        public InputDataContainer(GameDataType gameDataType) : base(gameDataType)
        {
            
        }

        public override List<BaseData> GetData(DataFilterParams @params)
        {
            if (!IsMatchDataType(@params.Type)) 
            {
                return null;
            }
            List<InputData> res = new List<InputData>();

            if(@params.Id != DataFilterParams.DEFAULT_ID && @params.Name != DataFilterParams.DEFAULT_NAME)
            {
                res = _inputData.Where(o => o.Id == @params.Id && o.Name == @params.Name).ToList();
            }
            else if (@params.Id != DataFilterParams.DEFAULT_ID) { 
                res = _inputData.Where(o=>o.Id == @params.Id).ToList();
            }
            else if (@params.Name != DataFilterParams.DEFAULT_NAME) { 
                res = _inputData.Where(o=>o.Name == @params.Name).ToList();
            }
            else
            {
                res = _inputData.ToList();
            }

            return res.Cast<BaseData>().ToList();
        
        }

        public override void SetData(object rawData)
        {
            _inputData = JsonConvert.DeserializeObject<InputData[]>((string)rawData);
            ParseKeyMap();
        }

        private void ParseKeyMap()
        {
            foreach (var input in _inputData){
                if(input.Name == null) continue;
                KeyCode inputKeyCode = MapKeyCode(input.Keycode);
                if (!_keysMap.ContainsKey(input.Name) && inputKeyCode != KeyCode.None){
                    _keysMap.Add(input.Name, inputKeyCode);
                }
            }
        }

        private KeyCode MapKeyCode(string c)
        {
            if (string.IsNullOrEmpty(c))
                return KeyCode.None;

            if (SpecialKeyCodes.TryGetValue(c, out KeyCode specialKeyCode))
                return specialKeyCode;

            if (c.Length == 1)
            {
                char ch = c[0];
                // Handle digits
                if (char.IsDigit(ch))
                {
                    return (KeyCode)((int)KeyCode.Alpha0 + (ch - '0'));
                }
                else if (char.IsLetter(ch))
                {
                    if (char.IsUpper(ch))
                    {
                        return (KeyCode)((int)KeyCode.A + (ch - 'A'));
                    }
                    else
                    {
                        return (KeyCode)((int)KeyCode.A + (ch - 'a'));
                    }
                }
            }

            return KeyCode.None;
        }

        public KeyCode GetKeyCode(string Name){
            if(_keysMap.TryGetValue(Name, out KeyCode keycode)){
                return keycode;
            }
            return KeyCode.None;
        }
    }
}
