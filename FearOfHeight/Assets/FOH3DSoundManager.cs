using RenderHeads.Media.AVProVideo;
using UnityEngine;

public class FOH3DSoundManager : FOHBehavior
{
    public float fadeOutTime;
    public float fadeStartTime;
    public MeshRenderer avMesh;

    [SerializeField]
    private GameObject effects;

    [SerializeField]
    private GameObject lv1Preset;
    [SerializeField]
    private GameObject lv1Effect;
    [SerializeField]
    private GameObject lv2Preset;
    [SerializeField]
    private GameObject lv2Effect;
    [SerializeField]
    private GameObject lv3Preset;
    [SerializeField]
    private GameObject lv3Effect;
    [SerializeField]
    private GameObject lv4Preset;
    [SerializeField]
    private GameObject lv4Effect;
    [SerializeField]
    private GameObject lv5Preset;
    [SerializeField]
    private GameObject lv5Effect;

    private AudioSource[] allSources;

    private FOHPlayListener listener;
    private bool offStart = false;
    private float nowTotalVolume = 1.0f;
    private float nowTime = 0.0f;

    public void Init()
    {
        allSources = GetComponentsInChildren<AudioSource>();
        listener = FindObjectOfType<FOHPlayListener>();

        if (game.FohStage.nowStageType != StageType.N_S5)
        {
            listener.transform.SetParent(null);
            gameObject.SetActive(false);
            effects.SetActive(false);
            game.FohStage.mediaPlayer.m_StereoPacking = StereoPacking.None;
            avMesh.material.SetFloat("Stereo", 0);
            return;
        }

        listener.Init();
        Configure();
        if (game.FohStage.nowLevelType == LevelType.LV2)
            fadeStartTime = 4.0f;

        if (game.FohStage.nowLevelType == LevelType.LV4 || game.FohStage.nowLevelType == LevelType.LV5)
            return;

        game.FohStage.mediaPlayer.m_StereoPacking = StereoPacking.TopBottom;
        avMesh.material.SetFloat("Stereo" , 1);
    }

    public void OffAllSounds()
    {
        offStart = true;
    }

    private void OffAllSoundsRoutine()
    {
        if (!offStart)
            return;

        if(Mathf.Approximately(0.0f , nowTotalVolume) && offStart)
        {
            offStart = false;
            return;
        }

        nowTime += FOHTime.globalDeltaTime / fadeOutTime;
        nowTotalVolume = Mathf.Lerp(nowTotalVolume, 0.0f, nowTime);

        foreach (AudioSource source in allSources)
        {
            source.volume = nowTotalVolume;
        }
    }

    private void Configure()
    {
        switch (game.FohStage.nowLevelType)
        {
            case LevelType.LV1:
                lv1Preset.SetActive(true);
                lv1Effect.SetActive(true);

                lv2Effect.SetActive(false);
                lv3Effect.SetActive(false);
                lv4Effect.SetActive(false);
                lv5Effect.SetActive(false);
                lv2Preset.SetActive(false);
                lv3Preset.SetActive(false);
                lv4Preset.SetActive(false);
                lv5Preset.SetActive(false);
                return;
            case LevelType.LV2:
                lv1Effect.SetActive(false);

                lv2Effect.SetActive(true);
                lv3Effect.SetActive(false);
                lv4Effect.SetActive(false);
                lv5Effect.SetActive(false);

                lv1Preset.SetActive(false);
                lv2Preset.SetActive(true);
                lv3Preset.SetActive(false);
                lv4Preset.SetActive(false);
                lv5Preset.SetActive(false);
                return;
            case LevelType.LV3:
                lv1Effect.SetActive(false);

                lv2Effect.SetActive(false);
                lv3Effect.SetActive(true);
                lv4Effect.SetActive(false);
                lv5Effect.SetActive(false);

                lv1Preset.SetActive(false);
                lv2Preset.SetActive(false);
                lv3Preset.SetActive(true);
                lv4Preset.SetActive(false);
                lv5Preset.SetActive(false);
                return;
            case LevelType.LV4:
                lv1Effect.SetActive(false);

                lv2Effect.SetActive(false);
                lv3Effect.SetActive(false);
                lv4Effect.SetActive(true);
                lv5Effect.SetActive(false);

                lv1Preset.SetActive(false);
                lv2Preset.SetActive(false);
                lv3Preset.SetActive(false);
                lv4Preset.SetActive(true);
                lv5Preset.SetActive(false);
                return;
            case LevelType.LV5:
                lv1Effect.SetActive(false);

                lv2Effect.SetActive(false);
                lv3Effect.SetActive(false);
                lv4Effect.SetActive(false);
                lv5Effect.SetActive(true);

                lv1Preset.SetActive(false);
                lv2Preset.SetActive(false);
                lv3Preset.SetActive(false);
                lv4Preset.SetActive(false);
                lv5Preset.SetActive(true);
                return;
        }
    }

    public override void ManualUpdate()
    {
        if (game.FohStage.nowStageType != StageType.N_S5)
            return;

        base.ManualUpdate();
        listener.MoveByCurve();

        float remainTime = (game.FohStage.mediaPlayer.Info.GetDurationMs() - game.FohStage.mediaPlayer.Control.GetCurrentTimeMs()) / 1000.0f;

        if (!offStart && remainTime <= fadeStartTime)
        {
            OffAllSounds();
            return;
        }

        OffAllSoundsRoutine();
    }
}
