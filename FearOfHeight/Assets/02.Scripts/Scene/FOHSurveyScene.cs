using UnityEngine;
using System.Collections;

public class FOHSurveyScene : FOHSceneManager
{
    protected override void Init()
    {
        base.Init();

    }

    protected override void Update()
    {
        base.Update();
        if(ui)
            ui.ManualUpdate();
    }
}
