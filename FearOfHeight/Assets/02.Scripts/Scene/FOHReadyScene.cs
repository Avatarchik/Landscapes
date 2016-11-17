using UnityEngine;
using System.Collections;
using PostProcess;
using UnityEngine.SceneManagement;

public class FOHReadyScene : FSMBase
{
    private OVRCameraRig OVRCamRig;

    public GameObject crosshair;

    protected override void Awake()
    {
        base.Awake();
        OVRCamRig = FindObjectOfType<OVRCameraRig>();

        Application.targetFrameRate = 60;
        OVRManager.instance.vsyncCount = 1;
        OVRManager.cpuLevel = 1;
        OVRManager.gpuLevel = 1;
    }

    private void Start()
    {
        state = SceneState.Loading;
    }

    protected override void Update()
    {
        base.Update();
        if (game.input)
            game.input.ManualUpdate();
        game.ui.ManualUpdate();
    }

    #region Loading

    private IEnumerator LoadingEnterState()
    {
//        InputTracking.Recenter();
        OVRManager.cpuLevel = 2;
        OVRManager.gpuLevel = 0;

        game.SetScene(this);
        game.Init();
        game.SE = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
        game.BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        game.NAVI = GameObject.FindGameObjectWithTag("NAVI").GetComponent<AudioSource>();

        state = SceneState.Title;
        yield break;
    }

    private IEnumerator LoadingExitState()
    {
        OVRManager.cpuLevel = 0;
        OVRManager.gpuLevel = 1;

        yield break;
    }

    #endregion

    #region Title

    private IEnumerator TitleEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        game.ui.ChangeWindow(FOHWindowType.Title);
        yield break;
    } 

    private void TitleUpdate()
    {
        if (game.input.IsKeyDown(InputType.Click) && !BlinkEffect.isPlaying)
        {
            game.input.KeyUp(InputType.Click);
            game.sounds.Play("UI_BTN_Select", game.SE);
            if (game.account.useGearS2)
                state = SceneState.GearS;
            else
                state = SceneState.StageSelect;
        }
    }

    private IEnumerator TitleExitState()
    {
        game.blink.FadeIn();
        yield break;
    }

    #endregion

    #region ConnectingConfirm

    private IEnumerator ConnectingConfirmEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundBlack();
        // game.ui.ActivateConnectingConfirmWindow();
        yield break;
    }

    private void ConnectingConfirmUpdate()
    {
        if (game.input.IsKeyDown(InputType.Click))
        {
            game.input.KeyUp(InputType.Click);
            state = SceneState.StageSelect;
        }
    }

    private IEnumerator ConnectingConfirmExitState()
    {
//        FadeIn();
        OVRCamRig.GetComponentInChildren<OVRScreenFade>().SendMessage("StartFadeIn");
        yield break;
    }

    #endregion

    #region StageSelect

    private IEnumerator StageSelectEnterState()
    {
        game.blink.FadeIn(LoadStageSelectScene);
        yield break;
    }

    private void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelect");
    }

    #endregion

    #region GearS

    private IEnumerator GearSEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        game.ui.ChangeWindow(FOHWindowType.GearS);
        yield break;
    }

    private IEnumerator GearSExitState()
    {
        game.blink.FadeIn();
        game.account.baseLine = game.gearS.GetBaselineHeartbeatRate();
        yield break;
    }

    #endregion

    #region Download

    private IEnumerator DownloadEnterState()
    {
        game.blink.FadeOut();
        game.background.BackgroundSplash();
        game.ui.ChangeWindow(FOHWindowType.Download);
        yield break;
    }

    private IEnumerator DownloadExitState()
    {
        game.blink.FadeIn();
        yield break;
    }

    #endregion

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
        game.account.Reset();
        yield break;
    }

    #endregion
}
