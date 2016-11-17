using UnityEngine;
using System.Collections;

public class FOHResultScene : FOHSceneManager
{
    public static SceneState previousScene;

    protected override void Init()
    {
        base.Init();
        // game.background.BackgroundSplash();
    }

    protected override void Update()
    {
        base.Update();
        if (ui)
            ui.ManualUpdate();
    }

    protected override void ExitState()
    {
        base.ExitState();
        //        FadeIn();
    }
}
