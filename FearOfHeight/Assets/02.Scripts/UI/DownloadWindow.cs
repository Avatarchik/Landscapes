using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DownloadWindow : FOHUIWindow
{
    public GameObject startDownloadWindow;
    public GameObject downloadingWindow;
    public EnergyBar currentProgressBar;
    public EnergyBar totalProgressBar;

    public GameObject[] photos;
    private Image[] photosImage = new Image[10];
    private int photoCnt;

    private int slideShowCnt;
    private int slideShowHeadIndex;
    private float slideShowSpeed = 50f;

    private FOHDownload download;

    #region Tween
    /*
    public void TweenComment()
    {
        comments[commentCnt].SetActive(true);

        commentsImage[commentCnt].DOFade(0f, 5f).SetEase(Ease.Linear).From();
        commentsImage[commentCnt].DOFade(0f, 5f).SetEase(Ease.Linear).SetDelay(5f).OnComplete(TweenCommentComplete);
    }

    public void TweenCommentComplete()
    {
        comments[commentCnt].SetActive(false);
        commentCnt++;
        if (commentCnt > 9)
        {
            commentCnt = 0;
        }
        if (game.mainScene.state.ToString() == SceneState.Download.ToString())
        {
            TweenComment();
        }
    }
    */
    #endregion

    public override void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            photosImage[i] = photos[i].GetComponent<Image>();
            photos[i].transform.DOLocalMoveX(1200f, 0f);
            photosImage[i].DOFade(0f, 0f);
        }

        download = FindObjectOfType<FOHDownload>();
    }

    public override void Active()
    {

        for (int i = 0; i < 10; i++)
        {
            photos[i].SetActive(false);
        }

        photoCnt = 0;

        if (download.IsRequireDownload())
        {
            base.Active();

            startDownloadWindow.SetActive(true);
            downloadingWindow.SetActive(false);
            return;
        }

        if (game.account.firstPlay)
            game.mainScene.state = SceneState.Welcome;
        else
            game.mainScene.state = SceneState.StageSelect;
    }

    public void OnStartDownloadConfirmClick()
    {
        startDownloadWindow.SetActive(false);
        downloadingWindow.SetActive(true);
        download.Init();
        SlideShowStart();
    }

    public void SetCurrentProgress(int progress)
    {
        currentProgressBar.valueCurrent = progress;
    }

    public void SetTotalProgress(int progress)
    {
        totalProgressBar.valueCurrent = progress;
    }

    public void ProgressBarInit(int total)
    {
        totalProgressBar.valueMax = total;
        totalProgressBar.valueCurrent = 0;
        currentProgressBar.valueCurrent = 0;
    }

    // SlideSHow
    private void SlideShowStart()
    {
        slideShowCnt = -1;
        slideShowHeadIndex = 0;
        SlideShowFadeIn();
    }

    private void SlideShowFadeIn()
    {
        slideShowCnt++;
        if (slideShowCnt > 9)
        {
            slideShowCnt = 0;
        }
        photos[slideShowCnt].SetActive(true);
        photos[slideShowCnt].transform.DOLocalMoveX(0f, 10f).SetEase(Ease.Linear).OnComplete(SlideShowFadeOut);
        photosImage[slideShowCnt].DOFade(1f, 5f).SetEase(Ease.Linear);
    }

    private void SlideShowFadeOut()
    {        
        photos[slideShowCnt].transform.DOLocalMoveX(-1200f, 10f).SetEase(Ease.Linear);
        photosImage[slideShowCnt].DOFade(0f, 5f).SetDelay(5f).SetEase(Ease.Linear).OnComplete(SlideShowDisable);
        SlideShowFadeIn();
    }

    private void SlideShowDisable()
    {
        photos[slideShowHeadIndex].SetActive(false);
        photos[slideShowHeadIndex].transform.DOLocalMoveX(1200f, 0f);
        photosImage[slideShowHeadIndex].DOFade(0f, 0f);
        slideShowHeadIndex++;
        if (slideShowHeadIndex > 9)
        {
            slideShowHeadIndex = 0;
        }        
    }
}
 