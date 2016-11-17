using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour
{
	void Awake ()
	{
		DontDestroyOnLoad(this.gameObject);

		if(FindObjectsOfType(this.GetType()).Length > 1)
		{
			Destroy(this.gameObject);
		}
	}
}
