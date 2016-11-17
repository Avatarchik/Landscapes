using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;

public class FOHPlayScene : FOHSceneManager
{
    private FOH3DSoundManager soundManager;

    protected override void Init()
    {
        base.Init();

        game.FohStage.Init();
        game.FohStage.Reset();
        game.FohStage.LoadMovie();
        crosshair.SetActive(false);
#if UNITY_ANDROID
        AndroidPluginManager.Instance().DemandCheckHBR(true);
#endif
   
    }

    protected override void ExitState()
    {
        base.ExitState();
        crosshair.SetActive(true);
#if UNITY_ANDROID
        AndroidPluginManager.Instance().DemandCheckHBR(false);
#endif
    }

    protected override void Update()
    {
        base.Update();
        if(game.FohStage == null)
            return;

        game.FohStage.ManualUpdate();

        if(game.FohStage.nowStageType != StageType.N_S5)
            return;

        if(soundManager)
            soundManager.ManualUpdate();
    }

    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                if(soundManager)
                    return;
                soundManager = FindObjectOfType<FOH3DSoundManager>();
                soundManager.Init();
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                game.FohStage.Play();
                game.blink.FadeOut();
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                game.blink.FadeIn();
                if (game.FohStage.nowLevelType == LevelType.Intro)
                    return;
                if (game.account.useSelfEvaluation)
                {
                    SetState(SceneState.Survey);
                }
                else
                {
                    game.FohStage.SetStar(game.FohStage.CalculateScore(0.1f, 0.1f, 0.8f));
                    SetState(SceneState.Evaluation);
                }
                break;
        }
    }
}
