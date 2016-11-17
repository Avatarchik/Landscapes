using UnityEngine;
using System.Collections;

public class ConnectConfirmButton : MonoBehaviour
{
	public void OnButtonClick()
	{
#if UNITY_EDITOR
		this.SendMessage("ButtonInit", SendMessageOptions.DontRequireReceiver);
		this.transform.root.gameObject.SetActive(false);
#else
		OVRManager.PlatformUIConfirmQuit();
#endif
	}
}
