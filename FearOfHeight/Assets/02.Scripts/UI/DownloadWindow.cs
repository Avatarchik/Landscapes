using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using DG.Tweening;


public class DownloadWindow : FOHUIWindow
{
    public GameObject startDownloadWindow;
    public GameObject downloadingWindow;
    public EnergyBar currentProgressBar;
    public EnergyBar totalProgressBar;

    public GameObject[] comments;
    private Image[] commentsImage = new Image[10];
    private int commentCnt;

    public GameObject[] photos;
    private Image[] photosImage = new Image[10];
    private int photoCnt;

    private int slideShowCnt;
    private int slideShowHeadIndex;
    private float slideShowSpeed = 50f;

    public bool error { private set; get; }

    private bool downloadComplete;
    private List<string> requireFileNames = new List<string>(); 
    private WWW www;
    // private string baseUrl = "https://s3.ap-northeast-2.amazonaws.com/fearofheight/";
    private string baseUrl = "https://s3.ap-northeast-2.amazonaws.com/clicked/FearOfHeight/Landscapes/";

    private IEnumerator downloadRoutine;
    private bool downloading = false;

    #region Tween

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

    #endregion

    public override void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            commentsImage[i] = comments[i].GetComponent<Image>();
            photosImage[i] = photos[i].GetComponent<Image>();
            photos[i].transform.DOLocalMoveX(1200f, 0f);
            photosImage[i].DOFade(0f, 0f);
        }
    }

    public override void Active()
    {
        for (int i = 0; i < 10; i++)
        {
            comments[i].SetActive(false);
            photos[i].SetActive(false);
        }

        commentCnt = 0;
        photoCnt = 0;

        TweenComment();

        CheckRequierFile();

        if (requireFileNames.Count > 0)
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

    public override void ManualUpdate()
    {
        base.ManualUpdate();
        #if UNITY_ANDROID && !UNITY_EDITOR
        if(downloading)
        {
            if (!OVRManager.isHmdPresent)
            {
                StopCoroutine(downloadRoutine);
                www.Dispose();
                StartCoroutine(WaitForFileDownload());
            }
        }
        #endif

        if (!downloadComplete)
            return;

        game.mainScene.state = SceneState.Welcome;
    }

    public void OnStartDownloadConfirmClick()
    {
        startDownloadWindow.SetActive(false);
        downloadingWindow.SetActive(true);
        StartDownload();
        SlideShowStart();
    }

    private void CheckRequierFile()
    {
        for (int i = 0; i < (int)StageType.Max; i++)
        {
            StageType stage = (StageType)i;
            for (int j = 0; j < (int)LevelType.Max; j++)
            {
                LevelType level = (LevelType)j;

                if (stage != StageType.N_S5 && (level == LevelType.LV4 || level == LevelType.LV5))
                    continue;
                if(stage == StageType.N_S5 && level == LevelType.Intro)
                    continue;

                string fileName = stage.ToString() + "_" + level.ToString() + ".mp4";
                if (!File.Exists(Application.persistentDataPath + "/" + fileName))
                    requireFileNames.Add(fileName);
            }
        }
    }

    private IEnumerator WaitForFileDownload()
    {
        while (true)
        {
            if (OVRManager.isHmdPresent == true)
            {
                requireFileNames.Clear();
                CheckRequierFile();
                StartDownload();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator FileDownload()
    {
        ProgressBarInit();

        for (int i = 0; i < requireFileNames.Count; i++)
        {
            www = new WWW(baseUrl + requireFileNames[i]);
        
            while (true)
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    error = true;
                    break;
                }

                currentProgressBar.valueCurrent = (int)(www.progress * 100);

                if (www.isDone)
                {
                    File.WriteAllBytes(Application.persistentDataPath + "/" + requireFileNames[i], www.bytes);
                    totalProgressBar.valueCurrent++;
                    break;
                }

                yield return null;
            }
        }

        if (error)
            Debug.LogError(www.error);

        yield return new WaitForSeconds(3f);

        DownloadComplete();
    }

    private void DownloadComplete()
    {
        downloadComplete = true;
        downloading = false;
        www.Dispose();
    }

    private void ProgressBarInit()
    {
        totalProgressBar.valueMax = requireFileNames.Count;
        totalProgressBar.valueCurrent = 0;
        currentProgressBar.valueCurrent = 0;
    }

    private void StartDownload()
    {
        downloadRoutine = FileDownload();
        StartCoroutine(downloadRoutine);
        downloading = true;
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
        if (slideShowCnt > 10)
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
        if (slideShowHeadIndex > 10)
        {
            slideShowHeadIndex = 0;
        }        
    }
}
 