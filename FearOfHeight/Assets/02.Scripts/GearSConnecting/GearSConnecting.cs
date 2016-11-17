using UnityEngine;
using System.Collections;

public class GearSConnecting : MonoBehaviour
{
	private bool isBaselineChecking = false;

	void Start ()
	{
		isBaselineChecking = true;
		GearSManager.Instance().CheckBaseline(true);
	}
	
	void Update ()
	{
		if(isBaselineChecking == true && GearSManager.Instance().GetBaselineHeartbeatCount() >= 10)
		{
			isBaselineChecking = false;
			GearSManager.Instance().CheckBaseline(false);
			Debug.Log("Baseline Heartbeat : " + GearSManager.Instance().GetBaselineHeartbeatRate());

			//SceneChangeManager.Instance().ChangeScene("Tutorial");
			// SceneChangeManager.Instance().ChangeScene("Title");
		}
	}
}
