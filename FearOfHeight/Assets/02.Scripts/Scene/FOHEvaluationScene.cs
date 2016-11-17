public class FOHEvaluationScene : FOHSceneManager
{
    protected override void Init()
    {
        ReportStage();
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
        if (ui)
            ui.ManualUpdate();
    }

    private void ReportStage()
    {
        FOHAccount.GameData data = game.FohStage.GetData();
        game.account.AddHistoryData(data);
        ResultWindow.selectedData = data;
        game.account.TrySetHighScoreData(data);
    }

    protected override void ExitState()
    {
        base.ExitState();
        game.background.BackgroundSplash();
    }
}
