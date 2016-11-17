using PostProcess;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FOHUIHighlightButton : FOHUIButton
{
    private Image hilightImage = null;

    protected void Awake()
    {
        base.Awake();
        hilightImage = transform.GetChild(0).GetComponent<Image>();
        hilightImage.enabled = false;
    }

    private void OnDisable()
    {
        hilightImage.enabled = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!game.disableAllButton)
        {
            hilightImage.enabled = true;
            base.OnPointerEnter(eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!game.disableAllButton)
        {
            hilightImage.enabled = false;
            base.OnPointerExit(eventData);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!game.disableAllButton)
        {
            base.OnPointerClick(eventData);
        }
    }
}
