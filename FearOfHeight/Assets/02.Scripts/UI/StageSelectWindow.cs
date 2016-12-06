using UnityEngine;
using DG.Tweening;
using I2.Loc;

public class StageSelectWindow : FOHUIWindow
{
    public GameObject[] baselineBar;
    public GameObject[] highlightBar;

    public GameObject mainWrapper;
    public GameObject subWrapper;

    private StageWrapper[] stageWrapper = new StageWrapper[3];


    private StageType selectedStage;

    private float tweenTime = 0.7f;

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < 3; i++)
        {
            stageWrapper = GetComponentsInChildren<StageWrapper>();
        }
    }

    public override void Active()
    {
        base.Active();

        selectedStage = StageType.Max;

        for (int i = 0; i < stageWrapper.Length; i++)
        {
            stageWrapper[i].Reset();

            for (int j = 0; j < stageWrapper[i].levelWrapper.Length; j++)
            {
                  stageWrapper[i].levelWrapper[j].Reset();
            }
        }

        baselineBar[0].transform.localPosition = SetYPos(baselineBar[0], 180f);
        baselineBar[1].transform.localPosition = SetYPos(baselineBar[1], -195f);

        highlightBar[0].transform.localPosition = SetXPos(highlightBar[0], 0f);
        highlightBar[1].transform.localPosition = SetXPos(highlightBar[1], 0f);

        mainWrapper.transform.localPosition = SetYPos(mainWrapper, 0f);
        subWrapper.transform.localPosition = SetYPos(subWrapper, -275f);
    }

    public override void ManualUpdate()
    {
        if (game.input.IsKeyDown(InputType.ClickEmpty))
        {
            if (selectedStage != StageType.Max)
            {
                if (game.ui.tweening == false)
                {
                    baselineBar[0].transform.DOLocalMoveY(150, tweenTime);
                    baselineBar[1].transform.DOLocalMoveY(-175f, tweenTime);

                    mainWrapper.transform.DOLocalMoveY(0f, tweenTime);
                    subWrapper.transform.DOLocalMoveY(-275f, tweenTime);

                    stageWrapper[(int)selectedStage].Close();

                    selectedStage = StageType.Max;
                }
            }
        }
    }    

    #region Enter
    private void Btn_Stage_Enter(int stage)
    {
        if (selectedStage.ToString() == StageType.Max.ToString())
        {
            highlightBar[0].transform.DOLocalMoveX((stage - 2) * 500, tweenTime);
            highlightBar[1].transform.DOLocalMoveX((stage - 2) * 500, tweenTime);
        }
    }

    public void Btn_Stage1_Enter()
    {
        Btn_Stage_Enter(1);
        game.background.BackGroundTween(BackgroundType.Elevator);

    }

    public void Btn_Stage2_Enter()
    {
        Btn_Stage_Enter(2);
        
        game.background.BackGroundTween(BackgroundType.SkyWalk);
    }

    public void Btn_Stage3_Enter()
    {
        Btn_Stage_Enter(3);
        
        game.background.BackGroundTween(BackgroundType.HeightSimulator);
    }
    #endregion

    #region Exit
    private void Btn_Stage_Exit(int stage)
    {
        stage -= 1;
        if (selectedStage.ToString() == ((StageType)stage).ToString())
        {
            DOTween.Pause("onExitTween");
        }
    }

    public void Btn_Stage1_Exit()
    {
        Btn_Stage_Exit(1);
    }

    public void Btn_Stage2_Exit()
    {
        Btn_Stage_Exit(2);
    }

    public void Btn_Stage3_Exit()
    {
        Btn_Stage_Exit(3);
    }
    #endregion

    #region Click

    private void Btn_Stage_Click(int stage)
    {
        stage -= 1;
        game.ui.tweening = true;

        if (selectedStage.ToString() != ((StageType)stage).ToString())
        {
            if (selectedStage.ToString() == StageType.Max.ToString())
            {
                baselineBar[0].transform.DOLocalMoveY(320f, tweenTime);
                baselineBar[1].transform.DOLocalMoveY(-325f, tweenTime);

                mainWrapper.transform.DOLocalMoveY(130f, tweenTime);
                subWrapper.transform.DOLocalMoveY(-405f, tweenTime);

                selectedStage = (StageType)stage;
            }

            else
            {
                stageWrapper[(int)selectedStage].Close();

                selectedStage = (StageType)stage;

                highlightBar[0].transform.DOLocalMoveX(((int)selectedStage - 1) * 500f, tweenTime);
                highlightBar[1].transform.DOLocalMoveX(((int)selectedStage - 1) * 500f, tweenTime);
            }          

            StartCoroutine(stageWrapper[(int)selectedStage].Open());
        }
    }

    public void Btn_Stage1_Click()
    {
        game.FohStage.SelectStage(StageType.N_S2);
        Btn_Stage_Click(1);
    }

    public void Btn_Stage2_Click()
    {
        game.FohStage.SelectStage(StageType.N_S4);
        Btn_Stage_Click(2);
    }

    public void Btn_Stage3_Click()
    {
        game.FohStage.SelectStage(StageType.N_S5);
        Btn_Stage_Click(3);
    }

    public void Btn_Level1_Click()
    {
        game.sounds.Play("FOH_Landscapes_Other_005 " + LocalizationManager.CurrentLanguageCode, game.NAVI);
        
        game.FohStage.SetLevel(LevelType.Intro);
        if(game.FohStage.nowStageType == StageType.N_S5)
            game.FohStage.SetLevel(LevelType.LV1);
        game.mainScene.state = SceneState.Play;
    }

    public void Btn_Level2_Click()
    {
        game.sounds.Play("FOH_Landscapes_Other_006 " + LocalizationManager.CurrentLanguageCode, game.NAVI);
        game.FohStage.SetLevel(LevelType.LV2);
        game.mainScene.state = SceneState.Play;
    }

    public void Btn_Level3_Click()
    {
        game.sounds.Play("FOH_Landscapes_Other_007 " + LocalizationManager.CurrentLanguageCode, game.NAVI);
        game.FohStage.SetLevel(LevelType.LV3);
        game.mainScene.state = SceneState.Play;
    }

    public void Btn_Level4_Click()
    {
        game.sounds.Play("FOH_Landscapes_Other_008 " + LocalizationManager.CurrentLanguageCode, game.NAVI);
        game.FohStage.SetLevel(LevelType.LV4);
        game.mainScene.state = SceneState.Play;
    }

    public void Btn_Level5_Click()
    {
        game.sounds.Play("FOH_Landscapes_Other_009 " + LocalizationManager.CurrentLanguageCode, game.NAVI);
        game.FohStage.SetLevel(LevelType.LV5);
        game.mainScene.state = SceneState.Play;
    }

    public void Btn_Option_Click()
    {
        game.mainScene.state = SceneState.Option;
    }

    public void Btn_Result_Click()
    {
        game.mainScene.state = SceneState.OtherResults;
    }
    #endregion

    private Vector3 SetXPos(GameObject go, float des)
    {
        return new Vector3(des, go.transform.localPosition.y, go.transform.localPosition.z);
    }

    private Vector3 SetYPos(GameObject go, float des)
    {
        return new Vector3(go.transform.localPosition.x, des, go.transform.localPosition.z);
    }

    private Vector3 SetZPos(GameObject go, float des)
    {
        return new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, des);
    }

    private Color SetFade(float des)
    {
        return new Color(1f, 1f, 1f, des);
    }
}
