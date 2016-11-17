using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonGaze : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private bool isAlphaButton = false;
	[SerializeField] private bool alwaysActive = false;

	private readonly float BUTTON_ANIMATION_TIME = 0.5f;

	private float buttonScale = 1.0f;
	private float buttonHoverScale = 1.1f;
	private float buttonClickScale = 0.9f;

	private bool isFocus = false;

	void Start ()
	{
		buttonScale = this.transform.localScale.x;
		buttonHoverScale *= buttonScale;
		buttonClickScale *= buttonScale;
	}

	void Update ()
	{
		if(isAlphaButton == false && this.GetComponent<Image>() != null)
		{
			if(this.GetComponent<Image>().color.a >= 0.9f)
			{
				this.GetComponent<Image>().raycastTarget = true;
			}
			else
			{
				this.GetComponent<Image>().raycastTarget = false;
			}
		}

		if(alwaysActive == true)
		{
			this.GetComponent<Image>().raycastTarget = true;
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		isFocus = true;
		this.SendMessage("OnButtonEnter", SendMessageOptions.DontRequireReceiver);
		HoverOn();
	}

	public void OnPointerExit(PointerEventData data)
	{
		isFocus = false;
		this.SendMessage("OnButtonExit", SendMessageOptions.DontRequireReceiver);
		HoverOff();
	}

	public void OnPointerClick(PointerEventData data)
	{
		Click();
	}

	void HoverOn()
	{
		iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one * buttonHoverScale, "time", BUTTON_ANIMATION_TIME, "easetype", iTween.EaseType.easeInOutSine));

		if(isAlphaButton == true)
		{
			iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 1.0f , "onupdatetarget", this.gameObject, "onupdate", "HoverAlphaTweenCallback", "time", BUTTON_ANIMATION_TIME, "easetype", iTween.EaseType.easeOutCirc));
		}
		else
		{
			foreach(Transform buttonOn in this.transform)
			{
				if(buttonOn.name.Equals("ButtonOn") == true)
				{
					buttonOn.GetComponent<Image>().enabled = true;
					buttonOn.GetComponent<AudioSource>().Play();
				}

				if(buttonOn.name.Equals("TextOn") == true)
				{
					buttonOn.GetComponent<Text>().enabled = true;
				}
			}
		}
	}

	void HoverOff()
	{
		iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one * buttonScale, "time", BUTTON_ANIMATION_TIME, "easetype", iTween.EaseType.easeInOutSine));

		if(isAlphaButton == true)
		{
			if(this.GetComponent<Image>().raycastTarget == true)
			{
				iTween.ValueTo(this.gameObject, iTween.Hash("from", 1.0f, "to", 0.5f , "onupdatetarget", this.gameObject, "onupdate", "HoverAlphaTweenCallback", "time", BUTTON_ANIMATION_TIME, "easetype", iTween.EaseType.easeOutCirc));
			}
		}
		else
		{
			foreach(Transform buttonOn in this.transform)
			{
				if(buttonOn.name.Equals("ButtonOn") == true)
				{
					buttonOn.GetComponent<Image>().enabled = false;
				}

				if(buttonOn.name.Equals("TextOn") == true)
				{
					buttonOn.GetComponent<Text>().enabled = false;
				}
			}
		}
	}

	void HoverAlphaTweenCallback(float newValue) 
	{ 
		this.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, newValue);
	}

	void Click()
	{
		this.GetComponent<AudioSource>().Play();

		iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one * buttonClickScale, "time", BUTTON_ANIMATION_TIME / 2, "easetype", iTween.EaseType.easeInSine));

		if(isFocus == true)
		{
			iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one * buttonHoverScale, "delay", BUTTON_ANIMATION_TIME / 2, "time", BUTTON_ANIMATION_TIME / 2, "easetype", iTween.EaseType.easeOutSine));
		}
		else
		{
			iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one * buttonScale, "delay", BUTTON_ANIMATION_TIME / 2, "time", BUTTON_ANIMATION_TIME / 2, "easetype", iTween.EaseType.easeOutSine));
		}

		Invoke("SendButtonClick", BUTTON_ANIMATION_TIME);
	}

	void SendButtonClick()
	{
		this.SendMessage("OnButtonClick", SendMessageOptions.DontRequireReceiver);
	}

	void ButtonInit()
	{
		this.transform.localScale = Vector3.one * buttonScale;
		if(isAlphaButton == true)
		{
			this.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		}
		else
		{
			foreach(Transform buttonOn in this.transform)
			{
				if(buttonOn.name.Equals("ButtonOn") == true)
				{
					buttonOn.GetComponent<Image>().enabled = false;
					break;
				}
			}
		}
	}
}
