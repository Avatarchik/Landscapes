using System.Collections.Generic;
using UnityEngine;
using System;

public enum FOHWindowType
{
    None = 0,
    Title,
    GearS,
    Welcome,
    StageSelect,
    Tutorial,
    Evaluation,
    Download,
    Play,
    Survey,
    Option,
    OtherResults,
    Result,
    HeightSimulator,
    Max   
}

public class FOHUIActionManager : FOHBehavior
{
    private readonly Dictionary<Enum, FOHUIWindow> windows = new Dictionary<Enum, FOHUIWindow>();
    private Enum nowWindow;
//    public FOHUIWindow[] newWindows;

    public bool tweening = false;

    protected override void Awake()
    {
        base.Awake();
        game.SetUI(this);//TODO : Dept;
//        game.scene.SetUI(this);

        FOHUIWindow[] newWindows = GetComponentsInChildren<FOHUIWindow>();

        for (int i = 0; i < newWindows.Length; i++)
        {
            newWindows[i].gameObject.SetActive(true);
            newWindows[i].Init();
            if (windows.ContainsKey(newWindows[i].type))
                continue;
            windows.Add(newWindows[i].type, newWindows[i]);
        }
    }

    public override void ManualUpdate()
    {
        base.ManualUpdate();
        if(nowWindow == null)
            return;
        windows[nowWindow].ManualUpdate();
    }

    public void ChangeWindow(Enum type)
    {
        foreach (var window in windows)
            window.Value.Deactive();

        FOHUIWindow tmp;
        if (windows.TryGetValue(type, out tmp))
        {
            nowWindow = type;
            tmp.Active();
            return;
        }

        Debug.LogError("Not Find Window : " + type);
    }

    public FOHUIWindow GetWindow(Enum type)
    {
        FOHUIWindow tmp = null;
        if (windows.TryGetValue(type, out tmp))
        {
            return tmp;
        }
        Debug.LogError("Not found : " + type);
        return null;
    }
}
