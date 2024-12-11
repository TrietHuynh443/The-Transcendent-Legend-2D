using GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingleton<LevelManager>
{
    private string levelSwitchName;
    private Vector2 velocity;
    private bool isVerticalSwitch = false;
    private bool isSwitching = false;
    public string LevelSwitchName => levelSwitchName;
    public Vector2 Velocity => velocity;
    public bool IsVerticalSwitch => isVerticalSwitch;
    public bool IsSwitching => isSwitching;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchScene(string sceneName, string switchName, Vector2 vec, bool isVertSwitch)
    {
        isSwitching = true;
        levelSwitchName = switchName;
        velocity = vec;
        isVerticalSwitch = isVertSwitch;
        EventAggregator.RaiseEvent<LevelCompletionAchievementProgress>(new LevelCompletionAchievementProgress()
        {
            LevelName = sceneName,
        });
        LoadScene(sceneName);
    }

    public void FinishSceneSwitch()
    {
        isSwitching = false;
    }
}