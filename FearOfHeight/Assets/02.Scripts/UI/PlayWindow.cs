using UnityEngine;
using System.Collections;
using I2.Loc;

public class PlayWindow : FOHUIWindow
{
    public override void ManualUpdate()
    {
        base.ManualUpdate();
        if (game.FohStage.nowStageType == StageType.N_S5)
        {
            if (game.FohStage.nowLevelType == LevelType.LV4)
            {
                if (game.FohStage.mediaPlayer.Control.GetCurrentTimeMs() >= 15f)
                {
                    // game.sounds.Play("FOH_Cityscapes_Main_001 " + LocalizationManager.CurrentLanguage, game.NAVI);
                }
            }

            if (game.FohStage.nowLevelType == LevelType.LV5)
            {
                if (game.FohStage.mediaPlayer.Control.GetCurrentTimeMs() >= 15f)
                {
                    // game.sounds.Play("FOH_Cityscapes_Main_001 " + LocalizationManager.CurrentLanguage, game.NAVI);
                }
            }
        }
    }
}
