using UnityEngine;
using UnityEngine.UI;

public class DisplayStatus : MonoBehaviour
{
    // TIME
    public Text text;
    private float time = 0;

    // FPS 
    private float timeLeft = 0.0f;
    private float accum = 0.0f;
    private int frames = 0;
    private string strFPS = null;
    private float updateInterval = 0.5f;

    void Update ()
	{
        text.text = time.ToString("0") + "Sec / TEMP : " + OVRManager.batteryTemperature + " / " + strFPS + " / CPU : " + OVRManager.cpuLevel + " / GPU : " + OVRManager.gpuLevel + " / Device : " + SystemInfo.deviceModel + "/" + SystemInfo.deviceName;
	    time += Time.deltaTime;
        UpdateFPS();
	}

    
    void UpdateFPS()
    {
        timeLeft -= Time.unscaledDeltaTime;
        accum += Time.unscaledDeltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeLeft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = frames / accum;

            strFPS = System.String.Format("FPS: {0:F2}", fps);

            timeLeft += updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
