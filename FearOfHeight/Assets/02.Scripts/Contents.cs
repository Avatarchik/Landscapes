using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Contents
{
    public Container<GameObject> particles { private set; get; }
    // public Container<Material> materials { private set; get; }
    public Container<GameObject> stageEventObjects { private set; get; }

    private Dictionary<string, Material> panoramas = new Dictionary<string, Material>();
    private Dictionary<StageType, Dictionary<LevelType, string>> moviePath = new Dictionary<StageType, Dictionary<LevelType, string>>();

    public void Init()
    {
        // materials = new Container<Material>("Materials");

        for (int i = 0; i < (int)StageType.Max; i++)
        {
            StageType type = (StageType)i;
            moviePath[type] = new Dictionary<LevelType, string>();
            for (int j = 0; j < (int)LevelType.Max; j++)
            {
                LevelType levelType = (LevelType)j;

                string path = type.ToString() + "_" + levelType.ToString() + ".mp4";
                this.moviePath[type][levelType] = path;
            }
        }

        Material[] materials = Resources.LoadAll<Material>("Panoramas");
        foreach (var material in materials)
        {
            panoramas.Add(material.name, material);
        }
    }

    public Material GetPanoramaMaterial(FOHAccount.GameDataKey key)
    {
        Material tmp;
        if (panoramas.TryGetValue(key.stageType + "_" + key.levelType, out tmp))
            return tmp;
        return null;
    }

    public string GetMoviePath(StageType stage, LevelType level)
    {
        Dictionary<LevelType, string> tmp;
        if (moviePath.TryGetValue(stage, out tmp))
        {
            string data;
            if (tmp.TryGetValue(level, out data))
                return moviePath[stage][level];
        }

        return "";
    }
}