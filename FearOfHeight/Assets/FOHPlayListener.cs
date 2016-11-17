using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Channels;
using UnityStandardAssets.CinematicEffects;

public class FOHPlayListener : FOHBehavior
{
    public AnimationCurve lv1Curve;
    public AnimationCurve lv2Curve;
    public AnimationCurve lv3Curve;
    public AnimationCurve lv4Curve;
    public AnimationCurve lv5Curve;
    public float smooth;

    private AnimationCurve nowCurve;
    private float totalPlayTime;
    private float posZ;
    private float nowTime;

    public void Init()
    {
        totalPlayTime = game.FohStage.mediaPlayer.Info.GetDurationMs()/1000.0f;

        switch (game.FohStage.nowLevelType)
        {
            case LevelType.LV1:
                nowCurve = lv1Curve;
                return;
            case LevelType.LV2:
                nowCurve = lv2Curve;
                return;
            case LevelType.LV3:
                nowCurve = lv3Curve;
                return;
            case LevelType.LV4:
                nowCurve = lv4Curve;
                return;
            case LevelType.LV5:
                nowCurve = lv5Curve;
                return;
        }
    }

    public void MoveByCurve()
    {
#if (UNITY_EDITOR)
        transform.rotation = game.scene.ovr.transform.rotation;
#endif
#if (!UNITY_EDITOR && UNITY_ANDROID)
        transform.rotation = game.scene.ovr.centerEyeAnchor.transform.rotation;
#endif
        nowTime += FOHTime.globalDeltaTime;
//        posZ = nowCurve.Evaluate(nowTime / totalPlayTime);
        posZ = nowCurve.Evaluate(nowTime);
        transform.position = Vector3.Slerp(transform.position , new Vector3(transform.position.x , transform.position.y , posZ), FOHTime.globalDeltaTime * smooth);
    }
}
