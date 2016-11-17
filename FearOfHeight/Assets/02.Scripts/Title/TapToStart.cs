using UnityEngine;
using System.Collections;

public class TapToStart : MonoBehaviour
{
	void Start ()
	{
		OVRTouchpad.TouchHandler += HandleTouchHandler;
	}

	void OnDestroy()
	{
		OVRTouchpad.TouchHandler -= HandleTouchHandler;
	}

	void HandleTouchHandler (object sender, System.EventArgs e)
	{
		var touchArgs = (OVRTouchpad.TouchArgs)e;
		OVRTouchpad.TouchEvent touchEvent = touchArgs.TouchType;

		switch(touchEvent)
		{
		case OVRTouchpad.TouchEvent.SingleTap:
			this.GetComponent<AudioSource>().Play();
			if(GearSManager.Instance().IsConnected() == true)
			{
				// SceneChangeManager.Instance().ChangeScene("GearSConnecting");
			}
			else
			{
				// SceneChangeManager.Instance().ChangeScene("GearSCheck");
			}
			break;
		}
	}
}