using UnityEngine;
using System.Collections;
using PostProcess;


public class Game
{
    private static Game instance = null;

    public static Game Instance
    {
        get
        {
            if(instance ==null)
                instance = new Game();
            return instance;
        }
    }

    public BlinkEffect blink { set; get; }
    public FOHSceneManager scene { private set; get; }
    public FSMBase mainScene { private set; get; }
    public FOHUIActionManager ui { private set; get; }
    public Platform platform { private set; get; }
    public Contents contents { private set; get; }
    public AirVRClient airVrClient { private set; get; }
    public FOHInput input { private set; get; }
    public FOHStage FohStage { private set; get; }
    public FOHSounds sounds { private set; get; }
    public AudioSource SE { set; get; }
    public AudioSource BGM { set; get; }
    public AudioSource NAVI { set; get; }
    public GearSManager gearS { private set; get; }
    public AndroidPluginManager androidPlugin { private set; get; }
    public FOHAccount account { private set; get; }
    public FOHBackground background { private set; get; }

    public bool disableAllButton = false;

    public void Init()
    {
        if (Util.IsEditorPlatform())
        {
            platform = new DesktopPlatform();
        }
        else if (Util.IsPlatformAndroid())
            platform = new AndroidPlatform();
        else if (Util.IsPlatformIOS())
            platform = new iOSPlatform();

        contents = new Contents();
        contents.Init();
        FohStage = new FOHStage();
        sounds = new FOHSounds();
        sounds.Load();
        account = new FOHAccount();
        account.Load();     

        gearS = GameObject.FindObjectOfType(typeof(GearSManager)) as GearSManager;
        androidPlugin = GameObject.FindObjectOfType(typeof(AndroidPluginManager)) as AndroidPluginManager;
    }

    public void SetScene(FOHSceneManager scene)
    {
        this.scene = scene;
    }

    public void SetScene(FSMBase scene)
    {
        this.mainScene = scene;
    }

    public void SetUI(FOHUIActionManager ui)
    {
        this.ui = ui;
    }

    public void SetInputManager(FOHInput input)
    {
        this.input = input;
    }

    public void SetBackground(FOHBackground background)
    {
        this.background = background;
    }
}
