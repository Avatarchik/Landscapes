using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class GearSWindow : FOHUIWindow
{
    [SerializeField] private GameObject gearSCheck;
    [SerializeField] private GameObject gearSConnecting;

    private GameObject gearSCheckBase;
    private GameObject gearSCheckConnectPopup;



    private bool isBaselineChecking = false;

    protected override void Awake()
    {
        base.Awake();

        gearSCheck.SetActive(false);
        gearSConnecting.SetActive(false);

        gearSCheckBase = gearSCheck.transform.FindChild("Base").gameObject;
        gearSCheckConnectPopup = gearSCheck.transform.FindChild("ConnectPopup").gameObject;
        gearSCheckBase.SetActive(true);
        gearSCheckConnectPopup.SetActive(false);
    }

    public override void Active()
    {
        base.Active();

        if (game.gearS.IsConnected())
        {
            gearSConnecting.SetActive(true);
            game.gearS.CheckBaseline(true);
            StartCoroutine("BaselineChecking");
        }

        else
        {
            gearSCheck.SetActive(true);
        }            
    }

    public void ContinueButtonClick()
    {
        game.mainScene.state = SceneState.Download;
    }

    public void ConnectButtonClick()
    {
        gearSCheckBase.SetActive(false);
        gearSCheckConnectPopup.SetActive(true);
    }

    public void ConfirmButtonClick()
    {
        #if UNITY_ANDROID
        OVRPlatformMenu.instance.ShowConfirmQuitMenu();
        #endif

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void BackButtonClick()
    {
        gearSCheckBase.SetActive(true);
        gearSCheckConnectPopup.SetActive(false);
    }

    IEnumerator BaselineChecking()
    {        
        while (true)
        {            
            if (game.gearS.GetBaselineHeartbeatCount() >= 10)
            {                
                game.gearS.CheckBaseline(false);                
                game.mainScene.state = SceneState.Download;
                yield break;
            }
            yield return null;
        }
    }
}
