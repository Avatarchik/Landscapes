﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
using DG.Tweening;

public class TutorialWindow : FOHUIWindow
{
    [SerializeField]
    private GameObject tutorial;
    [SerializeField]
    private RawImage text;

    public float scrollingStartYOffset;
    public float scrollingSpeed;
    public float scrollingDelay;

    private int currentText = 0;

    public override void Init()
    {
        base.Init();
    }

    public override void Active()
    {
        base.Active();
        tutorial.SetActive(true);
        StartCoroutine(ScrollingRoutine());
    }

    public void SkipButtonClick()
    {
        if (game.account.firstPlay)
            game.mainScene.state = SceneState.StageSelect;
        else
            game.mainScene.state = SceneState.Option;
    }

    private IEnumerator ScrollingRoutine()
    {
        text.material.SetTextureOffset("_MainTex", new Vector2(0f, scrollingStartYOffset));
        yield return new WaitForSeconds(scrollingDelay);
        while (true)
        {
            text.material.SetTextureOffset("_MainTex", new Vector2(0f, text.material.GetTextureOffset("_MainTex").y - scrollingSpeed * Time.deltaTime));
            if (text.material.GetTextureOffset("_MainTex").y <= 0)
            {
                text.material.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
                break;
            }
            yield return null;
        }
    }
}
