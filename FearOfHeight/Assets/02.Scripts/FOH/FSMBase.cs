using UnityEngine;
using System.Collections;
using System;

public class FSMBase : FOHBehavior
{
    public Enum previousState { private set; get; }
    public Action DoManualUpdate = DoNothing;
    public Action DoUpdate = DoNothing;
    public Action DoFixedUpdate = DoNothing;
    public Func<IEnumerator> ExitState = DoNothingCoroutine;
    public Action<Collider> DoOnTriggerEnter = DoNothingCollider;
    public Action<Collider> DoOnTriggerStay = DoNothingCollider;
    public Action<Collider> DoOnTriggerExit = DoNothingCollider;

    protected Actor actor;
    private Enum currentState;
    
    public Enum state
    {
        get
        {
            return currentState;
        }
        set
        {
            previousState = currentState;
            currentState = value;
            ConfigureCurrentState();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        actor = GetComponent<Actor>();
    }

    private static void DoNothingCollider(Collider collider)
    {

    }

    private static void DoNothing()
    {

    }

    private static IEnumerator DoNothingCoroutine()
    {
        yield break;
    }

    private void ConfigureCurrentState()
    {
        if (ExitState != null)
        {
            StartCoroutine(ExitState());
        }
        DoUpdate = ConfigureDelegate<Action>("Update", DoNothing);
        DoFixedUpdate = ConfigureDelegate<Action>("FixedUpdate", DoNothing);
        DoManualUpdate = ConfigureDelegate<Action>("ManualUpdate", DoNothing);
        DoOnTriggerEnter = ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
        DoOnTriggerStay = ConfigureDelegate<Action<Collider>>("OnTriggerStay", DoNothingCollider);
        DoOnTriggerExit = ConfigureDelegate<Action<Collider>>("OnTriggerExit", DoNothingCollider);
        ExitState = ConfigureDelegate<Func<IEnumerator>>("ExitState", DoNothingCoroutine);
        Func<IEnumerator> enterState = ConfigureDelegate<Func<IEnumerator>>("EnterState", DoNothingCoroutine);

        StartCoroutine(enterState());
    }

    T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
    {
        //CURRENTSTATE + METHODROOT라고 불리는 함수를 찾는다.
        //함수는 public 또는 private일 수 있다.
        var mtd = GetType().GetMethod(currentState.ToString() + methodRoot, System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);
        //만약 함수를 찾았다면
        if (mtd != null)
        {
            //일반적인 인스턴스가 필요로하는 타입의 델리게이트를 만들고
            //이를 T타입으로 캐스팅 한다.                     
            return Delegate.CreateDelegate(typeof(T), this, mtd) as T;
        }
        else
        {
            //만약 함수를 찾지 못했다면, default를 반환한다.
            return Default;
        }
    }
    protected virtual void Update()
    {
        DoUpdate();
    }
    protected virtual void FixedUpdate()
    {
        DoFixedUpdate();
    }
    public override void ManualUpdate()
    {
        base.ManualUpdate();
        DoManualUpdate();
    }
}
