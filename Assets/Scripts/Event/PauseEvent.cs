using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEvent 
{
    public static bool IsPaused;
    public void Pause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;
    }


    // Start is called before the first frame update
}
