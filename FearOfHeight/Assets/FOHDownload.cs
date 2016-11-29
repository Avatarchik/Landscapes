using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FOHDownload : FSMBase
{
    public enum State
    {
        Wait,
        Downloading,
        Write,
        Error,
        Done
    }

    private readonly List<string> requireFileNames = new List<string>();
    private int index = 0;

    private WWW www;

    private DownloadWindow window;

    protected override void Awake()
    {
        base.Awake();
        window = FindObjectOfType<DownloadWindow>();
        CheckRequierFile();
    }

    public void Init()
    {
        window.ProgressBarInit(requireFileNames.Count);
        state = State.Downloading;
    }

    public bool IsRequireDownload()
    {
        if (requireFileNames.Count > 0)
            return true;

        return false;
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
                if (stage == StageType.N_S5 && level == LevelType.Intro)
                    continue;

                string fileName = stage.ToString() + "_" + level.ToString() + ".mp4";
                if (!File.Exists(Application.persistentDataPath + "/" + fileName))
                    requireFileNames.Add(fileName);
            }
        }
    }

//    #region Wait
//
//    private IEnumerator WaitEnterState()
//    {
//        Debug.Log("wait state");
//        www.Dispose();
//        yield break;
//    }
//
//    private void WaitUpdate()
//    {
//#if UNITY_ANDROID && !UNITY_EDITOR
//           if (OVRManager.isHmdPresent)
//        {
//            state = State.Downloading;
//        }
//#endif
//    }
//
//    #endregion

    #region Downloading

    private const string baseUrl = "https://d3ex532dz7y8ie.cloudfront.net/";
    private string nowUrl = "";
    private bool downloadSetting;

    private IEnumerator DownloadingEnterState()
    {
        Debug.Log("Downloading state");

        nowUrl = baseUrl + requireFileNames[index];
        www = new WWW(nowUrl);

        yield return null;

        downloadSetting = true;
    }

    private void DownloadingUpdate()
    {
        if(!downloadSetting)
            return;

#if UNITY_ANDROID && !UNITY_EDITOR
        if (!OVRManager.isHmdPresent)
        {
            state = State.Error;
            return;
        }
#endif

        if (www.error != null)
        {
            state = State.Error;
            return;
        }

        window.SetCurrentProgress((int)(www.progress * 100));

        if (!www.isDone)
            return;

        state = State.Write;
    }

    private IEnumerator DownloadingExitState()
    {
        downloadSetting = false;
        yield break;
    }

    #endregion

    #region Error

    private IEnumerator ErrorEnterState()
    {
        Debug.Log("error state");
        game.mainScene.state = SceneState.Error;
        yield break;
    }

    #endregion

    #region Write

    private IEnumerator WriteEnterState()
    {
        Debug.Log("write state");
        File.WriteAllBytes(Application.persistentDataPath + "/" + requireFileNames[index], www.bytes);
        window.SetTotalProgress(window.totalProgressBar.valueCurrent + 1);

        yield return Yield.One;

        index++;

        if (index == requireFileNames.Count)
        {
            state = State.Done;
            yield break;
        }

        state = State.Downloading;
    }

    private IEnumerator WriteExitState()
    {
        www.Dispose();
        yield break;
    }

    #endregion

    #region Done

    private IEnumerator DoneEnterState()
    {
        game.mainScene.state = SceneState.Welcome;
        yield break;
    }

    #endregion
}
