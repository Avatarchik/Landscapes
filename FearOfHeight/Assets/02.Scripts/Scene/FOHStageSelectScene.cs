﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FOHStageSelectScene : FSMBase
{
    private FOHUIActionManager ui;

    public GameObject crosshair;

    protected override void Awake()
    {
        base.Awake();
        game.SetScene(this);
        ui = FindObjectOfType<FOHUIActionManager>();
        OVRManager.cpuLevel = 0;
        OVRManager.gpuLevel = 2;
    }

    private void Start()
    {
        game.SetScene(this);
        state = SceneState.StageSelect;
        game.SE = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
        game.BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        game.NAVI = GameObject.FindGameObjectWithTag("NAVI").GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();
        if (game.input)
            game.input.ManualUpdate();
        if (ui)
            ui.ManualUpdate();
    }

    #region Welcome

    private IEnumerator WelcomeEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        game.ui.ChangeWindow(FOHWindowType.Welcome);
        yield break;
    }

    private IEnumerator WelcomeExitState()
    {
        game.blink.FadeIn();
        yield break;
    }
    #endregion

    #region Tutorial

    private IEnumerator TutorialEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        game.ui.ChangeWindow(FOHWindowType.Tutorial);
        yield break;
    }

    private IEnumerator TutorialExitState()
    {
        game.blink.FadeIn();
        yield break;
    }

    #endregion

    #region StageSelect

    private IEnumerator StageSelectEnterState()
    {
        game.blink.FadeOut();
        game.background.bGMesh.SetActive(true);
        game.background.bGFadeMesh.SetActive(false);
        game.FohStage.Reset();

        game.ui.ChangeWindow(FOHWindowType.StageSelect);
        game.background.BackgroundBlack();
        yield break;
    }

    private IEnumerator StageSelectExitState()
    {
        game.blink.FadeIn();
        yield break;
    }

    #endregion

    #region OtherResults

    private IEnumerator OtherResultsEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        SceneManager.LoadScene("OtherResults");
        yield break;
    }

    #endregion

    #region Play
    private IEnumerator PlayEnterState()
    {
        game.disableAllButton = true;
        game.blink.LongFadeIn(ChangeToPlayScene);
        yield break;
    }

    private void ChangeToPlayScene()
    {
        game.disableAllButton = false;
        SceneManager.LoadScene("Play");
    }
    #endregion

    #region Evaluation

    private IEnumerator ResultEnterState()
    {
        game.blink.FadeOut();
        SceneManager.LoadScene("Evaluation");
        yield break;
    }

    #endregion

    #region Option

    private IEnumerator OptionEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        game.ui.ChangeWindow(FOHWindowType.Option);
        yield break;
    }

    private IEnumerator OptionExitState()
    {
        game.blink.FadeIn();
        yield break;
    }

    #endregion

}
