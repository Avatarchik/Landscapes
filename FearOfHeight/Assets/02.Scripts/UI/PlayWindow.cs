using UnityEngine;
using System.Collections;
using I2.Loc;

public class PlayWindow : FOHUIWindow
{
    private bool fallReady;

    public override void Init()
    {
        base.Init();
        fallReady = false;

    }

    public override void ManualUpdate()
    {
        base.ManualUpdate();
        if (game.FohStage.nowStageType == StageType.N_S5)
        {
            if (game.FohStage.nowLevelType == LevelType.LV4)
            {
                if (fallReady == false && game.FohStage.mediaPlayer.Control.GetCurrentTimeMs() >= 12000f)
                {
                    print("dd");
                    fallReady = true;
                    game.sounds.Play("FOH_Landscapes_Other_002 " + LocalizationManager.CurrentLanguage, game.NAVI);
                }
            }

            if (game.FohStage.nowLevelType == LevelType.LV5)
            {
                if (fallReady == false && game.FohStage.mediaPlayer.Control.GetCurrentTimeMs() >= 18000f)
                {
                    print("dd");
                    fallReady = true;
                    game.sounds.Play("FOH_Landscapes_Other_002 " + LocalizationManager.CurrentLanguage, game.NAVI);
                }
            }
        }
    }
}
