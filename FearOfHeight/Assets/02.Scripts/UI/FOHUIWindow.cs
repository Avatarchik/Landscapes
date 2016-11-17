using UnityEngine;
using System.Collections;

public class FOHUIWindow : FOHBehavior
{
    public FOHWindowType type;
    protected FOHUIActionManager ui;

    public virtual void Init()
    {
        ui = GetComponentInParent<FOHUIActionManager>();
    }

    public virtual void Active()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }

    public void FOHDoNothing()
    {
        Debug.Log("Nothing");
    }
}
