using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEvent 
{
    public static bool _gameIsPaused;
    public void Pause()
    {
        _gameIsPaused = !_gameIsPaused;
        Time.timeScale = _gameIsPaused ? 0 : 1;
    }


    // Start is called before the first frame update
}
