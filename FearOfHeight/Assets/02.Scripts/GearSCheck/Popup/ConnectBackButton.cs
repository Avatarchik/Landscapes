using UnityEngine;
using System.Collections;

public class ConnectBackButton : MonoBehaviour
{
	public void OnButtonClick()
	{
		this.SendMessage("ButtonInit", SendMessageOptions.DontRequireReceiver);
		this.transform.root.gameObject.SetActive(false);
	}
}
