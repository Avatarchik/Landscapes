using UnityEngine;
using System.Collections;

public class FOHBehavior : MonoBehaviour
{
    protected Game game { private set; get; }

    protected virtual void Awake()
    {
        game = Game.Instance;
    }

    public virtual void ManualUpdate() { }
}