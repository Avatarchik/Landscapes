using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FOHUILevelTweeningButton : FOHUIButton
{
    private Image buttonImage = null;
    public bool interactable = false;

    protected void Awake()
    {
        base.Awake();
        buttonImage = GetComponent<Image>();
    }

    private void OnDisable()
    {
        // buttonImage.DOFade(0f, 0f);
        buttonImage.transform.DOScale(1f, 0f);
    }

    public void EnableHighlight()
    {
        Sequence onHoverTween = DOTween.Sequence().SetId("onHoverTween");

        onHoverTween.Append(buttonImage.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.5f));
        onHoverTween.Join(buttonImage.DOFade(1f, 0.5f));

        onHoverTween.Play();
    }

    public void DisableHighlight()
    {
        DOTween.Complete("onHoverTween");
        Sequence onExitTween = DOTween.Sequence().SetId("onExitTween");

        onExitTween.Append(buttonImage.transform.DOScale(1f, 0.3f));
        onExitTween.Join(buttonImage.DOFade((100f / 255f), 0.3f));

        onExitTween.Play();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!game.disableAllButton)
        {
            if (interactable)
            {
                EnableHighlight();
                base.OnPointerEnter(eventData);
            }
        }     
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!game.disableAllButton)
        {
            if (interactable)
            {
                DisableHighlight();
                base.OnPointerExit(eventData);
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!game.disableAllButton)
        {
            if (!game.ui.tweening)
            {
                base.OnPointerClick(eventData);
            }
        }
    }
}