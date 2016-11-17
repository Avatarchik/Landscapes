using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

public class FOHAccount
{
    public Game game { get { return Game.Instance; } }

    public struct GameDataKey
    {
        public StageType stageType;
        public LevelType levelType;

        public GameDataKey(StageType stageType, LevelType levelType)
        {
            this.stageType = stageType;
            this.levelType = levelType;
        }
    }

    public class GameData
    {
        public StageType stageType;
        public LevelType levelType;
        public int star;
        public string date;
        public int baseLine;
        public float hbr;
        public float gazeTime;
        public float gazeScore;
        public int surveyPoint;
    }

    public int historyCount { set; get; }
    public bool firstPlay { set; get; }
    public StageType lastStage { set; get; }
    public LevelType lastLevel { set; get; }
    public bool useGearS2 { set; get; }
    public bool useSelfEvaluation { set; get; }
    public int baseLine { set; get; }

    private readonly Dictionary<GameDataKey , bool> unlocks = new Dictionary<GameDataKey, bool>(); 

    private readonly Dictionary<GameDataKey , GameData> highScoreDatas = new Dictionary<GameDataKey, GameData>();

    public List<GameData> historyDatas = new List<GameData>();

    public FOHAccount()
    {
        firstPlay = true;
        useGearS2 = true;
        useSelfEvaluation = true;
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        LoadDefaultPreset();
        Save();
    }

    public bool IsUnlock(GameDataKey key)
    {      
        bool tmp;
        if (unlocks.TryGetValue(key, out tmp))
        {
            return tmp;
        }

        Debug.LogError("Not Found : " + key.stageType + key.levelType);
        return tmp;
    }

    public void SetUnlock(GameDataKey key , bool unlock)
    {
        if (!unlocks.ContainsKey(key))
        {
            unlocks.Add(key , unlock);
            return;
        }
        unlocks[key] = unlock;
        Save();
    }

    public GameData GetHighScoreData(GameDataKey key)
    {
        GameData tmp;
        if (highScoreDatas.TryGetValue(key, out tmp))
        {
            return tmp;
        }

        Debug.LogError("Not Found : " + key.stageType + key.levelType);
        return tmp;
    }

    public bool TryUnlock(GameDataKey key)
    {
        GameData tmp;
        bool result;
        if (highScoreDatas.TryGetValue(key, out tmp))
        {
            if (tmp.star >= 2 && unlocks.TryGetValue(key, out result))
            {
                if (!result)
                {
                    SetUnlock(key, true);
                    return true;
                }
            }
        }

        Debug.LogWarning("Not Found Data : " + key.stageType + key.levelType);
        return false;
    }

    public void TrySetHighScoreData(GameData data)
    {
        GameDataKey key = new GameDataKey(data.stageType, data.levelType);
        if (!highScoreDatas.ContainsKey(key))
        {
            highScoreDatas.Add(key, data);
            Save();
            return;
        }

        if(highScoreDatas[key].star >= data.star)
            return;

        SetHighScoreData(key , data);
    }


    public void SetHighScoreData(GameDataKey key, GameData data)
    {
        if (!highScoreDatas.ContainsKey(key))
            return;

        highScoreDatas[key] = data;
        Save();
    }

    public void AddHistoryData(GameData data)
    {
        if (historyDatas.Contains(data))
            return;

        if (historyDatas.Count >= 27)
            historyDatas.RemoveAt(historyDatas.Count-1);

        historyDatas.Insert(0 , data);
        Save(); 
    }

    public void Save()
    {
        Util.SaveLocalData("LastStage" , lastStage.ToString());
        Util.SaveLocalData("LastLevel", lastLevel.ToString());
        Util.SaveLocalData("UseSelfEvaluation", useSelfEvaluation);
        Util.SaveLocalData("UseGearS2", useGearS2);
        Util.SaveLocalData("FirstPlayCheck", firstPlay);
        SaveHistory();
        SaveHighScore();
        SaveUnlock();
    }

    public void Load()
    {
        if(Util.HasKey("LastStage"))
            lastStage = (StageType)Enum.Parse(typeof(StageType), Util.LoadLocalDataString("LastStage"));
        if (Util.HasKey("LastLevel"))
            lastLevel = (LevelType)Enum.Parse(typeof(LevelType), Util.LoadLocalDataString("LastLevel"));
        if (Util.HasKey("UseSelfEvaluation"))
            useSelfEvaluation = Util.LoadLocalDataBool("UseSelfEvaluation");
        if (Util.HasKey("UseGearS2"))
            useGearS2 = Util.LoadLocalDataBool("UseGearS2");
        if (Util.HasKey("FirstPlayCheck"))
            firstPlay = Util.LoadLocalDataBool("FirstPlayCheck");
        if (Util.HasKey("HistoryCount"))
            historyCount = Util.LoadLocalDataInt("HistoryCount");
        LoadHistory();
        LoadHighScore();
        LoadUnlock();
    }

    public void LoadDefaultPreset()
    {
        firstPlay = false;
        useSelfEvaluation = true;
        useGearS2 = true;
        lastStage = StageType.N_S2;
        lastLevel = LevelType.Intro;

        //unlocks init
        for (int i = 0; i < (int) StageType.Max; i++)
        {
            StageType stageType = (StageType) i;
            for (int j = 0; j < (int) LevelType.Max; j++)
            {
                LevelType levelType = (LevelType) j;
                unlocks.Add(new GameDataKey(stageType , levelType) , false);
            }
        }
    }

    private void SaveUnlock()
    {
        foreach (var unlock in unlocks)
            Util.SaveLocalData(unlock.Key.stageType + "_" + unlock.Key.levelType + "_" + "Unlock", unlock.Value);
    }

    private void LoadUnlock()
    {
        unlocks.Clear();
        for (int i = 0; i < (int)StageType.Max; i++)
        {
            StageType stageType = (StageType)i;
            for (int j = 0; j < (int)LevelType.Max; j++)
            {
                LevelType levelType = (LevelType)j;
                if (Util.HasKey(stageType + "_" + levelType + "_" + "Unlock"))
                    SetUnlock(new GameDataKey(stageType, levelType) , Util.LoadLocalDataBool(stageType + "_" + levelType + "_" + "Unlock"));
            }
        }
    }

    private void SaveHighScore()
    {
        foreach (var highScore in highScoreDatas)
        {
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" +"HighScore" + "_" + "Date", highScore.Value.date);
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" + "HighScore" + "_" + "HBR", highScore.Value.hbr);
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" + "HighScore" + "_" + "Star", highScore.Value.star);
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" + "HighScore" + "_" + "GazeScore", highScore.Value.gazeScore);
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" + "HighScore" + "_" + "StageType", highScore.Value.stageType.ToString());
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" + "HighScore" + "_" + "LevelType", highScore.Value.levelType.ToString());
            Util.SaveLocalData(highScore.Key.stageType + "_" + highScore.Key.levelType + "_" + "HighScore" + "_" + "BaseLine", highScore.Value.baseLine.ToString());
        }
    }

    private void LoadHighScore()
    {
        highScoreDatas.Clear();
        for (int i = 0; i < (int) StageType.Max; i++)
        {
            StageType stageType = (StageType) i;
            for (int j = 0; j < (int) LevelType.Max; j++)
            {
                LevelType levelType = (LevelType) j;
                GameData newData = new GameData();

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "Date"))
                    newData.date = Util.LoadLocalDataString(stageType + "_" + levelType + "_" + "HighScore" + "_" + "Date");

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "HBR"))
                    newData.hbr = Util.LoadLocalDataFloat(stageType + "_" + levelType + "_" + "HighScore" + "_" + "HBR");

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "Star"))
                    newData.star = Util.LoadLocalDataInt(stageType + "_" + levelType + "_" + "HighScore" + "_" + "Star");

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "GazeScore"))
                    newData.gazeScore = Util.LoadLocalDataInt(stageType + "_" + levelType + "_" + "HighScore" + "_" + "GazeScore");

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "LevelType"))
                    newData.levelType = (LevelType)Enum.Parse(typeof(LevelType), Util.LoadLocalDataString(stageType + "_" + levelType + "_" + "HighScore" + "_" + "LevelType"));

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "StageType"))
                    newData.stageType = (StageType)Enum.Parse(typeof(StageType), Util.LoadLocalDataString(stageType + "_" + levelType + "_" + "HighScore" + "_" + "StageType"));

                if (Util.HasKey(stageType + "_" + levelType + "_" + "HighScore" + "_" + "BaseLine"))
                    newData.baseLine = Util.LoadLocalDataInt(stageType + "_" + levelType + "_" + "HighScore" + "_" + "BaseLine");

                highScoreDatas.Add(new GameDataKey(stageType , levelType) , newData);
            }
        }
    }

    private void SaveHistory()
    {
        for (int i = 0; i < historyDatas.Count; i++)
        {
            Util.SaveLocalData(i + "_" + "Date" , historyDatas[i].date);
            Util.SaveLocalData(i + "_" + "HBR", historyDatas[i].hbr);
            Util.SaveLocalData(i + "_" + "Star", historyDatas[i].star);
            Util.SaveLocalData(i + "_" + "GazeScore", (int)(historyDatas[i].gazeScore));
            Util.SaveLocalData(i + "_" + "StageType", historyDatas[i].stageType.ToString());
            Util.SaveLocalData(i + "_" + "LevelType", historyDatas[i].levelType.ToString());
            Util.SaveLocalData(i + "_" + "BaseLine", historyDatas[i].baseLine.ToString());
        }
        Util.SaveLocalData("HistoryCount" , historyDatas.Count);
    }

    private void LoadHistory()
    {
        int index = 0;
        while (index < historyCount)
        {
            GameData newData = new GameData();

            if (Util.HasKey(index + "_" + "Date"))
            {
                newData.date = Util.LoadLocalDataString(index + "_" + "Date");
            }
            if (Util.HasKey(index + "_" + "HBR"))
            {
                newData.hbr = Util.LoadLocalDataFloat(index + "_" + "HBR");
            }
            if (Util.HasKey(index + "_" + "Star"))
            {
                newData.star = Util.LoadLocalDataInt(index + "_" + "Star");
            }
            if (Util.HasKey(index + "_" + "GazeScore"))
            {
                newData.gazeScore = Util.LoadLocalDataInt(index + "_" + "GazeScore");
            }
            if (Util.HasKey(index + "_" + "LevelType"))
            {
                newData.levelType = (LevelType)Enum.Parse(typeof(LevelType) , Util.LoadLocalDataString(index + "_" + "LevelType")) ;
            }
            if (Util.HasKey(index + "_" + "StageType"))
            {
                newData.stageType = (StageType)Enum.Parse(typeof(StageType), Util.LoadLocalDataString(index + "_" + "StageType"));
            }
            if (Util.HasKey(index + "_" + "BaseLine"))
            {
                newData.baseLine = Util.LoadLocalDataInt(index + "_" + "BaseLine");
            }

            historyDatas.Add(newData);
            index++;
        }
    }
}
