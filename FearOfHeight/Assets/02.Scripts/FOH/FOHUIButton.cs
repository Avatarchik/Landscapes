using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using PostProcess;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class FOHUIButton : EventTrigger
{
    public FOHUIWindow window { private set; get; }

    protected Game game { private set; get; }

    [SerializeField]
    protected string onPointerEnterMethodName;
    [SerializeField]
    protected string onPointerExitMethodName;
    [SerializeField]
    protected string onPointerClickMethodName;

    

    protected void Awake()
    {
        window = GetComponentInParent<FOHUIWindow>();
        game = Game.Instance;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        game.sounds.Play("UI_BTN_Over", game.SE);
        // print("Enter");
        FindMethod(onPointerEnterMethodName)();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        // print("Exit");
        FindMethod(onPointerExitMethodName)();   
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        game.sounds.Play("UI_BTN_Select", game.SE);
        // print("Click");
        FindMethod(onPointerClickMethodName)();     
    }

    public void SetOnPointerClickMethodName(string name)
    {
        onPointerClickMethodName = name;
    }

    public void SetOnPointerEnterMethodName(string name)
    {
        onPointerEnterMethodName = name;
    }

    public void SetOnPointerExitMethodName(string name)
    {
        onPointerExitMethodName = name;
    }

    protected Action FindMethod(string methodName)
    {
        if (methodName == "FOHDoNothing")
            return () => { };
   
        MethodInfo method = window.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance
            | BindingFlags.Public | BindingFlags.DeclaredOnly);
        if (method == null)
            return null;
        return Delegate.CreateDelegate(typeof(Action), window, method) as Action;
    }
}
