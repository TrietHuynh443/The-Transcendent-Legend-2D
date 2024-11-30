using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingleton<LevelManager>
{
    private string levelSwitchName;
    private Vector2 velocity;
    private bool isVerticalSwitch = false;
    public string LevelSwitchName => levelSwitchName;
    public Vector2 Velocity => velocity;
    public bool IsVerticalSwitch => isVerticalSwitch;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchScene(string sceneName, string switchName, Vector2 vec, bool isVertSwitch)
    {
        levelSwitchName = switchName;
        velocity = vec;
        isVerticalSwitch = isVertSwitch;
        LoadScene(sceneName);
    }
}