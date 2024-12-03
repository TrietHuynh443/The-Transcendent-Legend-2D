using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;

    public static bool gameIsPaused = false;

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

    void Start()
    {
        //_resumeButton = _pauseUI.transform.Find("ResumeButton").GetComponent<Button>();
        //_menuButton = _pauseUI.transform.Find("MenuButton").GetComponent<Button>();

        //if(_resumeButton && _menuButton)
        //    Debug.Log("Pause buttons found");

        Debug.Log(gameIsPaused);

        _resumeButton.onClick.AddListener(OnResumeClicked);
        _menuButton.onClick.AddListener(OnMenuClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(SpecialKeyCodes["Pause"]))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame(gameIsPaused);
        }


    }

    private void PauseGame(bool gameIsPaused)
    {
        _pauseUI.SetActive(gameIsPaused);
        Time.timeScale = gameIsPaused ? 0f : 1f;
    }


    private void OnResumeClicked()
    {
        PauseGame(false);
    }

    private void OnMenuClicked()
    {
        // Go to the main menu or handle menu logic
        Debug.Log("Menu button clicked");
    }
}
