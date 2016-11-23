using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum BackgroundType
{
    Black,    
    Elevator,
    SkyWalk,
    HeightSimulator,
    Splash
}

public class FOHBackground : FOHBehavior
{
    public GameObject bGMesh;
    public MeshRenderer bGMeshRenderer;

    public GameObject blackScreen;

    public BackgroundType curBackground;

    protected override void Awake()
    {
        base.Awake();
        game.SetBackground(this);

        bGMeshRenderer = bGMesh.GetComponent<MeshRenderer>();
        blackScreen = GameObject.FindWithTag("BlackScreen");
        blackScreen.SetActive(false);
    }

    public override void ManualUpdate()
    {
        base.ManualUpdate();
    }

    public void BackGroundTween(BackgroundType stageBackground)
    {
        if (curBackground.ToString() != stageBackground.ToString())
        {
            if (curBackground.ToString() == BackgroundType.Black.ToString())
            {
                curBackground = stageBackground;
                bGMeshRenderer.material.mainTextureOffset = new Vector2(0.5f, 0.33f * ((int)curBackground));
                blackScreen.GetComponent<SpriteRenderer>().DOFade(0f, 0.25f);
            }
            else
            {
                curBackground = stageBackground;
                blackScreen.GetComponent<SpriteRenderer>().DOFade(1f, 0.25f).OnComplete(BackgorundOffComplete);
            }
        }
    }

    public void BackgorundOffComplete()
    {
        bGMeshRenderer.material.mainTextureOffset = new Vector2(0.5f, 0.33f * ((int)curBackground));
        blackScreen.GetComponent<SpriteRenderer>().DOFade(0f, 0.25f);
    }

    public void BackgroundBlack()
    {
        curBackground = BackgroundType.Black;
        blackScreen.GetComponent<SpriteRenderer>().DOFade(1f, 0f);
        blackScreen.SetActive(true);
    }

    public void BackgroundSplash()
    {
        bGMeshRenderer.material.mainTextureOffset = new Vector2(0.5f, 0f);
    }
}
