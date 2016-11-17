using UnityEngine;
using System.Collections;

public class FOHTime
{
    private static bool pause = false;
    public static bool Pause
    {
        get
        {
            return pause;
        }
        set
        {
            pause = value;

            if (pause == true && Time.timeScale > 0.0f)
                Time.timeScale = 0.0f;
            if (pause == false && Time.timeScale < 1.0f)
                Time.timeScale = 1.0f;
        }
    }
    public static float PlayTime { set; get; }

    public static float globalDeltaTime
    {
        get { return pause ? 0 : Time.deltaTime; }
    }
}
