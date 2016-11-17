using UnityEngine;
using System.Collections;
using System;

public class FOHEvent : FOHBehavior
{
    [SerializeField]
    private string hoverEnterAction;

    [SerializeField]
    private string hoverExitAction;

    [SerializeField]
    private string clickAction;

    public void OnClick()
    {
        if(clickAction == "")
            return;
        game.ui.SendMessage(clickAction);
    }

    public void OnHoverEnter()
    {
        if(hoverEnterAction == "")
            return;
        game.ui.SendMessage(hoverEnterAction);
    }

    public void OnHoverExit()
    {
        if (hoverExitAction == "")
            return;
        game.ui.SendMessage(hoverExitAction);
    }
}
