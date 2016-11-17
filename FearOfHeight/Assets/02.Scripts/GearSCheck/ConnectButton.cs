using UnityEngine;
using System.Collections;

public class ConnectButton : MonoBehaviour
{
	[SerializeField] private GameObject connectPopup = null;

	public void OnButtonClick()
	{
		connectPopup.SetActive(true);
	}
}
