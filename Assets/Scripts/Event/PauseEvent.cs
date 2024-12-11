using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEvent 
{
    public static bool IsPaused;
    
    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;
    }
    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }


    // Start is called before the first frame update
}
