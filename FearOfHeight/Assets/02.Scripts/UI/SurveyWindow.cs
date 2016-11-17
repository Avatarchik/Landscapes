using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SurveyWindow : FOHUIWindow {

    public enum SurveyType
    {
        None,
        NotNervous,
        LittleNervous,
        VeryNervous
    }

    public SurveyType selectedSurvey;

    [SerializeField]
    private GameObject survey;
    [SerializeField]
    private GameObject[] surveyButton;

    public override void Init()
    {
        base.Init();
        game.background.bGMeshRenderer.material =
        game.contents.GetPanoramaMaterial(new FOHAccount.GameDataKey(game.FohStage.nowStageType,
            game.FohStage.nowLevelType));

        selectedSurvey = SurveyType.None;
    }

    public void NotNervousButtonExit()
    {
        if (selectedSurvey == SurveyType.NotNervous)
        {
            surveyButton[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void NotNervousButtonClick()
    {
        selectedSurvey = SurveyType.NotNervous;
        for (int i = 0; i < 3; i++)
        {
            if (((int)selectedSurvey - 1) == i)
            {
                surveyButton[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            else
            {
                surveyButton[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        game.FohStage.SetSurveyPoint(2);
    }

    public void LittleNervousButtonExit()
    {
        if (selectedSurvey == SurveyType.LittleNervous)
        {
            surveyButton[1].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void LittleNervousButtonClick()
    {
        selectedSurvey = SurveyType.LittleNervous;
        for (int i = 0; i < 3; i++)
        {
            if (((int)selectedSurvey - 1) == i)
            {
                surveyButton[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            else
            {
                surveyButton[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        game.FohStage.SetSurveyPoint(1);
    }

    public void VeryNervousButtonExit()
    {
        if (selectedSurvey == SurveyType.VeryNervous)
        {
            surveyButton[2].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void VeryNervousButtonClick()
    {
        selectedSurvey = SurveyType.VeryNervous;
        for (int i = 0; i < 3; i++)
        {
            if (((int)selectedSurvey - 1) == i)
            {
                surveyButton[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            else
            {
                surveyButton[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        game.FohStage.SetSurveyPoint(0);
    }

    public void ConfirmButtonClick()
    {
        if (selectedSurvey == SurveyType.None)
            return;

        survey.SetActive(false);

        game.FohStage.SetStar(game.FohStage.CalculateScore(1.0f , 1.0f , 1.0f));
        game.scene.SetState(SceneState.Evaluation);
    }
}
