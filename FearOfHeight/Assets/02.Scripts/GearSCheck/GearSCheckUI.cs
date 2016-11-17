using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GearSCheckUI : MonoBehaviour
{
	private float alpha = 0;

	private bool isFadeIn = false;
	private bool isTapEnabled = false;

	void Start ()
	{
		foreach(Transform ui in this.transform)
		{
			ui.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
		}

		StartCoroutine(FadeInStart());
	}

	void Update ()
	{
		if(isFadeIn == true && alpha < 1.0f)
		{
			foreach(Transform ui in this.transform)
			{
				alpha += Time.deltaTime;
				ui.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
			}
		}
	}

	IEnumerator FadeInStart()
	{
		yield return new WaitForSeconds(1.0f);

		isFadeIn = true;
	}
}