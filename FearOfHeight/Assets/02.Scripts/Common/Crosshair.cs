using UnityEngine;
using System.Collections;

public class Crosshair : FOHBehavior
{
    public GameObject highlight;

    private float nowTime = 0.0f;
    private float clickTime = 0.05f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !highlight.activeSelf)
        {
            highlight.SetActive(true);
            return;
        }

        if (highlight.activeSelf)
        {
            if (nowTime < clickTime)
            {
                nowTime += Time.deltaTime;
                return;
            }

            highlight.SetActive(false);
            nowTime = 0.0f;
        }
    }


}
