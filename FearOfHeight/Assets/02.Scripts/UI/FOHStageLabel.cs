using DG.Tweening;
using UnityEngine.UI;
using I2.Loc;


public class FOHStageLabel : FOHBehavior
{
    public float waitTime = 3.0f;

    private Image renderer;
    private float nowTime;
    private bool start;

    private void Awake()
    {
        base.Awake();
        renderer = GetComponent<Image>();
    }

    private void FadeIn()
    {
        if (game.FohStage.nowLevelType == LevelType.LV1 || game.FohStage.nowLevelType == LevelType.Intro)
            game.sounds.Play("FOH_Landscapes_Other_005 " + LocalizationManager.CurrentLanguage, game.NAVI);
        else if (game.FohStage.nowLevelType == LevelType.LV2)
            game.sounds.Play("FOH_Landscapes_Other_006 " + LocalizationManager.CurrentLanguage, game.NAVI);
        else if (game.FohStage.nowLevelType == LevelType.LV3)
            game.sounds.Play("FOH_Landscapes_Other_007 " + LocalizationManager.CurrentLanguage, game.NAVI);

        renderer.DOFade(1.0f, 3.0f).OnComplete(() =>
        {
            FadeOut();
        });
    }

    private void FadeOut()
    {
        renderer.DOFade(0.0f, 3.0f);
    }

    private void Update()
    {
        if(start)
            return;
        if (nowTime < waitTime)
        {
            nowTime += FOHTime.globalDeltaTime;
            return;
        }

        start = true;
        FadeIn();
    }
}
