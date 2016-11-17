using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TitleWindow : FOHUIWindow
{

    [SerializeField] private Image tapToStartImage;
    [SerializeField] private Image disconnectedImage;
    [SerializeField] private Image connectedImage;
    private float curTime = 0f;
    private float curTime2 = 0f;
    private bool trigger = false;

    private int cnt = -1;

    public override void ManualUpdate()
    {
        base.ManualUpdate();
        curTime += Time.deltaTime;
        curTime2 += Time.deltaTime;

        if (curTime >= 0.5f)
        {
            if (tapToStartImage.color.a == 1f)
                tapToStartImage.color = new Color(1f, 1f, 1f, 0.5f);
            else
                tapToStartImage.color = new Color(1f, 1f, 1f, 1f);
            curTime = 0f;
        }

        if (game.gearS.IsConnected())
        {
            connectedImage.gameObject.SetActive(true);
            disconnectedImage.gameObject.SetActive(false);
            if (curTime2 >= 0.4f)
            {
                connectedImage.transform.localPosition = new Vector3(100f * cnt, 0f, 0f);
                cnt++;
                if (cnt == 2)
                    cnt = -1;

                curTime2 = 0f;
            }
        }

        else
        {
            connectedImage.gameObject.SetActive(false);
            disconnectedImage.gameObject.SetActive(true);
            if (curTime2 >= 0.7f)
            {
                if (disconnectedImage.enabled == false)
                {
                    disconnectedImage.enabled = true;
                }
                else
                {
                    disconnectedImage.enabled = false;
                }
                curTime2 = 0f;
            }
        }
    }
}
