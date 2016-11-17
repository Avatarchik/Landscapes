using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ResultWindow : FOHUIWindow
{
    public static FOHAccount.GameData selectedData { set; get; }  

    [SerializeField]
    private GameObject reportWindow;

    [SerializeField]
    private GameObject baseline;
    [SerializeField]
    private GameObject hbr;
    [SerializeField]
    private GameObject noData;

    [SerializeField]
    private Image baselineGage;
    [SerializeField]
    private Text baselineText;

    [SerializeField]
    private Image hbrGage;
    [SerializeField]
    private Text hbrText;

    [SerializeField]
    private Text gazeText;    

    public override void Init()
    {
        base.Init();

        game.background.BackGroundTween((BackgroundType)((int)selectedData.stageType + 1));

        baselineGage.fillAmount = 0f;
        hbrGage.fillAmount = 0f;


        if (selectedData.baseLine <= 0)
        {
            Debug.Log("No GearS Data");
            baseline.SetActive(false);
            hbr.SetActive(false);
            noData.SetActive(true);
        }
        else
        {
            baseline.SetActive(true);
            hbr.SetActive(true);
            noData.SetActive(false);
        }

        baselineText.text = selectedData.baseLine.ToString();
        hbrText.text = selectedData.hbr.ToString();

        // 모든 심박수의 최대량은 100을 기준으로 설정
        baselineGage.DOFillAmount(selectedData.baseLine / 100.0f, 2f);
        hbrGage.DOFillAmount(selectedData.hbr / 100.0f, 2f);

        StartCoroutine("GazeValueTween");
    }

    private IEnumerator GazeValueTween()
    {
        for(int i=0; i<100; i++)
        {
            gazeText.text = Random.Range(0, 100).ToString();
            yield return new WaitForSeconds(0.01f);
        }
        gazeText.text = selectedData.gazeScore.ToString("0");
    }

    public void BackButtonClick()
    {
        // HISTORY로 되돌아가기
        if (FOHResultScene.previousScene.ToString() == SceneState.OtherResults.ToString())
        {
            game.scene.SetState(SceneState.OtherResults);//TODO : 리팩토링 필요(씬나눠야함 히스토리)
            return;
        }
        // RESULT로 되돌아가기
        if (FOHResultScene.previousScene.ToString() == SceneState.Evaluation.ToString())
        {
            game.scene.SetState(SceneState.Evaluation);
        }
    }
}
