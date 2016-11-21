using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using I2.Loc;

public class EvaluationWindow : FOHUIWindow
{
    [SerializeField]
    private GameObject resultWindow;

    [SerializeField]
    private GameObject tryAgainButton;

    [SerializeField]
    private GameObject selectLocationButton;

    [SerializeField]
    private GameObject nextLevelButton;

    /*
    [SerializeField]
    private GameObject nextLockButton;
    */

    [SerializeField]
    private GameObject[] stars;

    [SerializeField]
    private GameObject[] grades;

    public override void Init()
    {
        base.Init();
        game.background.bGMeshRenderer.material =
        game.contents.GetPanoramaMaterial(new FOHAccount.GameDataKey(game.FohStage.nowStageType,
            game.FohStage.nowLevelType));

        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(false);
            grades[i].SetActive(false);
        }

        int index = game.FohStage.GetStar() - 1;

        stars[index].SetActive(true);
        grades[index].SetActive(true);

        stars[index].GetComponent<Image>().DOFade(0f, 1f).From();
        grades[index].GetComponent<Image>().DOFade(0f, 1f).From();
        grades[index].transform.DOLocalMoveX(-700f, 1f).From();

        if(game.FohStage.GetData().star == 1)
            game.sounds.Play("FOH_OTH_ETC_08 " + LocalizationManager.CurrentLanguage, game.NAVI);

        if (game.account.GetHighScoreData(new FOHAccount.GameDataKey(game.FohStage.nowStageType, game.FohStage.nowLevelType)).star < 2)
        {
            if (game.FohStage.IsFinal())
            {
                nextLevelButton.SetActive(false);
                // nextLockButton.SetActive(false);
                tryAgainButton.transform.DOLocalMoveX(-175f, 0f);
                selectLocationButton.transform.DOLocalMoveX(175f, 0f);
            }

            else
            {
                nextLevelButton.SetActive(false);
                tryAgainButton.transform.DOLocalMoveX(-175f, 0f);
                selectLocationButton.transform.DOLocalMoveX(175f, 0f);
                // nextLockButton.SetActive(true);
            }
        }
        else
        {
            if (game.FohStage.IsFinal())
            {
                nextLevelButton.SetActive(false);
                tryAgainButton.transform.DOLocalMoveX(-175f, 0f);
                selectLocationButton.transform.DOLocalMoveX(175f, 0f);
                // nextLockButton.SetActive(false);
            }
            else
            {
                nextLevelButton.SetActive(true);
                tryAgainButton.transform.DOLocalMoveX(-350f, 0f);
                nextLevelButton.transform.DOLocalMoveX(0f, 0f);
                selectLocationButton.transform.DOLocalMoveX(350f, 0f);
                // nextLockButton.SetActive(false);
            }

            if (game.account.TryUnlock(new FOHAccount.GameDataKey(game.FohStage.nowStageType, game.FohStage.nowLevelType)))
            {
                if (game.FohStage.nowStageType == StageType.N_S5 && game.FohStage.nowLevelType == LevelType.LV3)
                {
                    game.sounds.Play("FOH_Landscapes_Other_015 " + LocalizationManager.CurrentLanguage, game.NAVI);
                    return;
                }

                if (game.FohStage.nowLevelType == LevelType.LV1 || game.FohStage.nowLevelType == LevelType.Intro)
                    game.sounds.Play("FOH_Landscapes_Other_011 " + LocalizationManager.CurrentLanguage, game.NAVI);
                else if (game.FohStage.nowLevelType == LevelType.LV2)
                    game.sounds.Play("FOH_Landscapes_Other_013 " + LocalizationManager.CurrentLanguage, game.NAVI);
                else if (game.FohStage.nowLevelType == LevelType.LV3)
                    game.sounds.Play("FOH_OTH_ETC_05 " + LocalizationManager.CurrentLanguage, game.NAVI);
            }
            else
            {
                if (game.FohStage.GetData().star == 2)
                {
                    game.sounds.Play("FOH_OTH_ETC_07 " + LocalizationManager.CurrentLanguage, game.NAVI);
                    return;
                }

                if (game.FohStage.GetData().star == 3)
                {
                    game.sounds.Play("FOH_OTH_ETC_06 " + LocalizationManager.CurrentLanguage, game.NAVI);
                }
            }
        }
    }

    public void NextLevelButtonClick()
    {
        game.FohStage.LevelClear();
        game.scene.SetState(SceneState.Play);
    }

    public void SeeReportButtonClick()
    {
        FOHResultScene.previousScene = SceneState.Evaluation;
        game.scene.SetState(SceneState.Result);
    }

    public void TryAgainButtonClick()
    {
        game.FohStage.Reset();
        game.scene.SetState(SceneState.Play);
    }

    public void BackToMenuButtonClick()
    {
        game.scene.SetState(SceneState.StageSelect);
    }
}
