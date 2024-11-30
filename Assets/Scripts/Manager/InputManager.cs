using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : UnitySingleton<InputManager>
{
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
            { "Pause", KeyCode.Escape },
        };

    void Update()
    {
        if (Input.GetKeyDown(SpecialKeyCodes["Pause"]))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        EventAggregator.RaiseEvent<PauseGameEvent>(new PauseGameEvent());
        Time.timeScale = 0;
    }
}

public class PauseGameEvent
{
}