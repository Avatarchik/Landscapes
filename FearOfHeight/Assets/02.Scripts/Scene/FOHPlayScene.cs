using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;

public class FOHPlayScene : FOHSceneManager
{
    private FOH3DSoundManager soundManager;

    public GameObject avMesh;

    protected override void Awake()
    {
        base.Awake();
        avMesh.GetComponent<MeshRenderer>().enabled = false;
    }

    protected override void Init()
    {
        base.Init();

#if UNITY_ANDROID && !UNITY_EDITOR
        // S6
	    if (SystemInfo.deviceModel.Contains("G920") || SystemInfo.deviceModel.Contains("G925") ||
	        SystemInfo.deviceModel.Contains("G928") || SystemInfo.deviceModel.Contains("N920"))
	    {
	        OVRManager.cpuLevel = 0;
            OVRManager.gpuLevel = 3;
	    }       

        // Others
	    else
	    {
            OVRManager.cpuLevel = 0;
            OVRManager.gpuLevel = 2;
        }
#endif

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
                if (soundManager)
                    return;
                soundManager = FindObjectOfType<FOH3DSoundManager>();
                soundManager.Init();
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                avMesh.GetComponent<MeshRenderer>().enabled = true;
                game.FohStage.Play();
                game.blink.FadeOut();
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                game.blink.FadeIn();
                if (game.FohStage.nowLevelType == LevelType.Intro)
                    return;
                if (game.account.useSelfEvaluation)
                {
                    OVRManager.cpuLevel = 0;
                    OVRManager.gpuLevel = 1;
                    SetState(SceneState.Survey);
                }
                else
                {
                    OVRManager.cpuLevel = 0;
                    OVRManager.gpuLevel = 1;
                    game.FohStage.SetStar(game.FohStage.CalculateScore(0.1f, 0.1f, 0.8f));
                    SetState(SceneState.Evaluation);
                }
                break;
        }
    }
}
