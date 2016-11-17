using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.VR;


public enum SceneState
{
    Loading,
    Title,
    ConnectingConfirm,
    StageSelect,
    Play,
    Survey,
    GearS,
    Welcome,
    Evaluation,
    Tutorial,
    Download,
    Option,
    OtherResults,
    Result,
}

public class FOHSceneManager : FOHBehavior
{
    public GameObject crosshair;
    public FOHUIWindow ui { private set; get; }
    public OVRCameraRig ovr { private set; get; }

    private SceneState NextScene;

    protected override void Awake()
    {
        base.Awake();
        ovr = FindObjectOfType<OVRCameraRig>();
    }

    protected virtual void Start()
    {
        game.SetScene(this);
        Init();
    }

    protected virtual void Init()
    {
        game.SE = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
        game.BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        game.NAVI = GameObject.FindGameObjectWithTag("NAVI").GetComponent<AudioSource>();
        ui = FindObjectOfType < FOHUIWindow>();
        if(ui)
            ui.Init();
//        InputTracking.Recenter();
        game.blink.FadeOut();
    }

    protected virtual void Update()
    {
        if (game.input)
            game.input.ManualUpdate();
        if(ui)
            ui.ManualUpdate();
    }

    public virtual void OnBackButtonClick()
    {
        game.scene.SetState(SceneState.StageSelect);
    }

    public void SetState(SceneState state)
    {
        NextScene = state;
        ExitState();
    }

    private void EnterState()
    {
        switch (NextScene)
        {
            case SceneState.StageSelect:
                SceneManager.LoadScene("StageSelect");
                return;
            case SceneState.OtherResults:
                SceneManager.LoadScene("OtherResults");
                return;
            case SceneState.Play:
                SceneManager.LoadScene("Play");
                return;
            case SceneState.Survey:
                SceneManager.LoadScene("Survey");
                return;
            case SceneState.Evaluation:
                SceneManager.LoadScene("Evaluation");
                return;
            case SceneState.Result:
                SceneManager.LoadScene("Result");
                return;
        }
    }

    protected virtual void ExitState()
    {
        game.blink.FadeIn(EnterState);
    }
}
