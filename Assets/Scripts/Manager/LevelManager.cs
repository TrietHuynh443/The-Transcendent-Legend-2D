using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingleton<LevelManager>
{
    private string levelSwitchName;
    private bool isSwitching = false;
    private bool isSwitchingLeftToRight = false;
    public bool IsSwitching => isSwitching;
    public string LevelSwitchName => levelSwitchName;
    public bool IsSwitchingLeftToRight => isSwitchingLeftToRight;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchScene(string sceneName, string switchName, bool isSwitchingLtR)
    {
        levelSwitchName = switchName;
        isSwitchingLeftToRight = isSwitchingLtR;
        StartLevelSwitch();
        LoadScene(sceneName);
    }

    public void StartLevelSwitch()
    {
        isSwitching = true;
    }

    public void EndLevelSwitch()
    {
        isSwitching = false;
    }
}