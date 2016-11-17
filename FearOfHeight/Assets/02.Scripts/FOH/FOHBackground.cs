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
    public GameObject bGFadeMesh;
    public MeshRenderer bGFadeMeshRenderer;

    public BackgroundType curBackground;

    protected override void Awake()
    {
        base.Awake();
        game.SetBackground(this);

        bGMeshRenderer = bGMesh.GetComponent<MeshRenderer>();
        bGFadeMeshRenderer = bGFadeMesh.GetComponent<MeshRenderer>();
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
                bGMeshRenderer.material.DOColor(new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f), 0.5f);                
            }
            else
            {
                bGMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);
                bGFadeMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);

                bGFadeMeshRenderer.material.mainTextureOffset = new Vector2(0.5f, 0.33f * ((int)curBackground));
                curBackground = stageBackground;
                bGMeshRenderer.material.mainTextureOffset = new Vector2(0.5f, 0.33f * ((int)curBackground));
                bGFadeMesh.SetActive(true);
                bGFadeMeshRenderer.material.DOColor(Color.clear, 0.5f).OnComplete(BackgroundTweenComplete);
            }
        }
    }

    public void BackgroundTweenComplete()
    {
        bGFadeMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);
        bGFadeMesh.SetActive(false);
    }

    public void BackgroundBlack()
    {
        curBackground = BackgroundType.Black;        
        bGMeshRenderer.material.color = new Color(0f, 0f, 0f, 1f);
        bGFadeMeshRenderer.material.color = new Color(0f, 0f, 0f, 1f);
    }

    public void BackgroundStage()
    {
        bGMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);
        bGFadeMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);
    }

    public void BackgroundSplash()
    {
        bGMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);
        bGFadeMeshRenderer.material.color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1f);
        bGMeshRenderer.material.mainTextureOffset = new Vector2(0.5f, 0f);
    }
}
