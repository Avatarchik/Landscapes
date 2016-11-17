using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RenderHeads.Media.AVProVideo;

public enum StageType
{   
    N_S2,
    N_S4,
    N_S5,
    Max
}

public enum LevelType
{
    Intro = 0,
    LV1,
    LV2,
    LV3,
    LV4,
    LV5,
    Max
}

public class FOHStage 
{
    public Game game { get { return Game.Instance; } }
    [SerializeField]
    public StageType nowStageType { private set; get; }
    public LevelType nowLevelType { private set; get; }
    public MediaPlayer mediaPlayer { private set; get; }
    public bool loading { private set; get; }

    private float ms;
    private float hbrNowTime = 0.0f;
    private const float hbrCheckTime = 1.0f;
    private int hbrTotal;
    private int hbrCount;
    private FOHAccount.GameData data;
    private const float pointingRotMin = 30.0f;
    private const float pointingRotMax = 90.0f;
    private float hoverNowTime = 0.0f;

    public void ManualUpdate()
    {
        SightCheck();
        HBRCheck();

        ms += FOHTime.globalDeltaTime;

        if (mediaPlayer == null)
            return;

 

        if (mediaPlayer.Control.IsFinished())
        {
            if (nowLevelType == LevelType.Intro)
            {
                LevelClear();
                LoadMovie();
                return;
            }

           RecordNowData();
        }
    }

    public void RecordNowData()
    {
        data.baseLine = game.account.baseLine;
        string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        SetDate(date);

        ms = mediaPlayer.Info.GetDurationMs();
    }

    public void Init()
    {
        mediaPlayer = Object.FindObjectOfType(typeof(MediaPlayer)) as MediaPlayer;
    }

    public void Reset()
    {
        hbrTotal = 0;
        hbrCount = 0;
        data = new FOHAccount.GameData();
        data.stageType = nowStageType;
        data.levelType = nowLevelType;
        ms = 0.0f;
    }

    public FOHAccount.GameData GetData()
    {
        return data;
    }


    public void SetSurveyPoint(int point)
    {
        data.surveyPoint = point;
    }

    public bool IsFinal()
    {
        int nextIndex = (int)nowLevelType + 1;
        if (File.Exists(Application.persistentDataPath + "/" +
            game.contents.GetMoviePath(nowStageType, (LevelType)nextIndex)))
            return false;

        return true;
    }

    public void LoadMovie()
    {
        loading = true;
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToPeristentDataFolder,
            game.contents.GetMoviePath(nowStageType, nowLevelType), false);
    }

    public void Play()
    {
        loading = false;
        mediaPlayer.Control.Play();
    }

    public void SetLevel(LevelType type)
    {
        nowLevelType = type;
        data.levelType = nowLevelType;
    }

    public void LevelClear()
    {
        nowLevelType++;
        SetLevel(nowLevelType);
    }

    public void SelectStage(StageType type)
    {
        nowStageType = type;
    }

    private void SetDate(string date)
    {
        data.date = date;
    }

    private void HBRCheck()
    {
        if (hbrNowTime <= 0.0f)
        {
            AndroidPluginManager.Instance().DemandGetHBR();
            hbrTotal += AndroidPluginManager.Instance().GetHBR();
            hbrCount++;
            hbrNowTime = hbrCheckTime;
        }
        hbrNowTime -= FOHTime.globalDeltaTime;
    }

    private void SightCheck()
    {
        if (nowLevelType == LevelType.Intro)
            return;

#if UNITY_EDITOR
        if (game.scene.ovr.transform.rotation.eulerAngles.x > pointingRotMin
         && game.scene.ovr.transform.rotation.eulerAngles.x < pointingRotMax)
            data.gazeTime += FOHTime.globalDeltaTime;
#endif

#if UNITY_ANDROID
        if (UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye).eulerAngles.x > pointingRotMin
         && UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye).eulerAngles.x < pointingRotMax)
            data.gazeTime += FOHTime.globalDeltaTime;
#endif
    }


    public int CalculateScore(float ratio1, float ratio2, float ratio3)
    {
        float gazeScore = (data.gazeTime / (ms / 1000.0f)) * 100.0f;        
        data.gazeScore = gazeScore;
        float hbrScore = 100.0f;
        if (game.gearS.IsConnected())
        {
            hbrScore = (((hbrTotal / hbrCount) - game.account.baseLine) / game.account.baseLine) * 100.0f;
            data.hbr = hbrTotal / hbrCount;
        }
        int score1 = 0;
        if (gazeScore > 80.0f)
            score1 = 2;
        else if (gazeScore > 50.0f)
            score1 = 1;
        else
            score1 = 0;

        int score2 = 0;
        if (hbrScore < 10.0f)
            score2 = 2;
        else if (hbrScore < 20.0f)
            score2 = 1;
        else
            score2 = 0;

        // return data.surveyPoint + score1 + score2;
        return data.surveyPoint + score1 + score2;
    }

    public void SetStar(int score)
    {
        if (score >= 5)
            data.star = 3;
        else if (score >= 3)
            data.star = 2;
        else
            data.star = 1;
    }

    public int GetStar()
    {
        return data.star;
    }
}
