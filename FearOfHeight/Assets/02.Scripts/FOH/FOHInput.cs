using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PostProcess;
using UnityEngine.EventSystems;
using UnityEngineInternal;

public enum InputType
{
    Click,
    ClickEmpty,
    Hover,
    Max
}

public class FOHInput : FOHBehavior
{
    [SerializeField] private float _rayLength = 5000.0f;
    [SerializeField] private LayerMask layerMask;

    private Game game {get { return Game.Instance; } }
    private Dictionary<InputType , bool> inputs = new Dictionary<InputType, bool>();
    public BlinkEffect[] EyeBlinkEffcet;
    public BlinkEffect[] AirVREyeBlinkEffcet;

    private Camera main;

    protected override void Awake()
    {
        base.Awake();
        game.SetInputManager(this);
        Init();
    }

    public void TryPointerEnterEvent()
    {
        Ray ray = new Ray(main.transform.position, main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayLength, layerMask))
        {
            FOHUILevelTweeningButton tmp = hit.collider.GetComponent<FOHUILevelTweeningButton>();
            if (tmp)
                tmp.EnableHighlight();
        }
    }

    public void Init()
    {
        main = Camera.main;

        for (int i = 0; i < (int)InputType.Max; i++)
        {
            InputType type = (InputType)i;
            inputs[type] = false;
        }
    }

    public void KeyDown(InputType type)
    {
        inputs[type] = true;
    }

    public void KeyUp(InputType type)
    {
        inputs[type] = false;
    }

    public bool IsKeyDown(InputType type)
    {
        return inputs[type];
    }

    public override void ManualUpdate()
    {
        base.ManualUpdate();

        if (game.scene && Input.GetButtonDown("Cancel"))
        {            
            game.scene.OnBackButtonClick();
        }

        /*
        if (game.FohStage.nowStageType == StageType.HeightSimulator)
        {
            Ray ray = new Ray(main.transform.position, main.transform.forward);
            RaycastHit hit;
        }
        */

        if (Input.GetMouseButtonDown(0))
        {
            if (inputs[InputType.Click] == false)
            {
                KeyDown(InputType.Click);
                KeyDown(InputType.ClickEmpty);

                Ray ray = new Ray(main.transform.position, main.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _rayLength, layerMask))
                {
                    KeyUp(InputType.ClickEmpty);
                }
            }
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            KeyUp(InputType.Click);
            KeyUp(InputType.ClickEmpty);
        }
    }

    public void EyeBlink(System.Action onComplete = null, System.Action onFadeInComplete = null)
    {
            foreach (BlinkEffect blinkEffect in EyeBlinkEffcet)
            {
                blinkEffect.Blink(onComplete, onFadeInComplete);
            }
    }
}
