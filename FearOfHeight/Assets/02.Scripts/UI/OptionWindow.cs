using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionWindow : FOHUIWindow
{
    [SerializeField] private GameObject Option;
    [SerializeField] private GameObject PopUp;

    [SerializeField]
    private GameObject GearSOnButton;
    [SerializeField]
    private GameObject GearSOffButton;
    [SerializeField]
    private GameObject SelfEvaluationOnButton;
    [SerializeField]
    private GameObject SelfEvaluationOffButton;

    public override void Init()
    {
        base.Init();
        Option.SetActive(true);
        PopUp.SetActive(false);
    }

    public override void Active()
    {
        base.Active();
        InitOptionButton();
    }

    void InitOptionButton()
    {
        if (game.account.useGearS2)
        {
            GearSOnButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
            GearSOffButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
        else
        {
            GearSOnButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
            GearSOffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }

        if (game.account.useSelfEvaluation)
        {
            SelfEvaluationOnButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
            SelfEvaluationOffButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
        else
        {
            SelfEvaluationOnButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
            SelfEvaluationOffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void Btn_Back_Click()
    {
        game.mainScene.state = SceneState.StageSelect;
        game.account.Save();
    }

    public void Btn_Info_Click()
    {
        game.mainScene.state = SceneState.Welcome;
    }

    public void Btn_Tutorial_Click()
    {
        game.mainScene.state = SceneState.Tutorial;
    }

    public void On_Btn_UseGearS2_Exit()
    {
        if (game.account.useGearS2)
        {
            GearSOnButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On_Btn_UseGearS2_Click()
    {
        game.account.useGearS2 = true;
        GearSOnButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        GearSOffButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    public void Off_Btn_UseGearS2_Exit()
    {
        if (!game.account.useGearS2)
        {
            GearSOffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void Off_Btn_UseGearS2_Click()
    {
        game.account.useGearS2 = false;
        GearSOnButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
        GearSOffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void On_Btn_UseSelfEvaluation_Exit()
    {
        if (game.account.useSelfEvaluation)
        {
            SelfEvaluationOnButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On_Btn_UseSelfEvaluation_Click()
    {
        game.account.useSelfEvaluation = true;
        SelfEvaluationOnButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        SelfEvaluationOffButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    public void Off_Btn_UseSelfEvaluation_Exit()
    {
        if (!game.account.useSelfEvaluation)
        {
            SelfEvaluationOffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void Off_Btn_UseSelfEvaluation_Click()
    {
        game.account.useSelfEvaluation = false;
        SelfEvaluationOnButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
        SelfEvaluationOffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void Btn_Reset_Click()
    {
        Option.SetActive(false);
        PopUp.SetActive(true);
    }

    public void Btn_ResetYes_Click()
    {
        Option.SetActive(true);
        InitOptionButton();
        PopUp.SetActive(false);
        game.account.historyDatas.Clear();
    }

    public void Btn_ResetNo_Click()
    {
        Option.SetActive(true);
        InitOptionButton();
        PopUp.SetActive(false);
    }
}
