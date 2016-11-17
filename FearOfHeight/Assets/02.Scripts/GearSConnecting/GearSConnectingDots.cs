using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GearSConnectingDots : MonoBehaviour
{
	[SerializeField] private Sprite[] dots = null;

	private int dotCount = 0;

	void OnEnable()
	{
		StartCoroutine(DotChange());
	}

    void OnDisable()
    {
        StopCoroutine(DotChange());
    }

    IEnumerator DotChange()
	{
		this.GetComponent<Image>().sprite = dots[dotCount % dots.Length];

		dotCount++;

		yield return new WaitForSeconds(0.25f);
		StartCoroutine(DotChange());
	}
}
