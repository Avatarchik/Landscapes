using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class StageWrapper : FOHBehavior
{
    private float tweenTime = 0.7f;

    public GameObject stageBtn { get; set; }
    public GameObject selectedBar { get; set; }
    public LevelWrapper[] levelWrapper;

    protected override void Awake()
    {
        base.Awake();
        stageBtn = transform.GetChild(0).gameObject;
        selectedBar = transform.GetChild(1).gameObject;
    }

    public void Reset()
    {
        selectedBar.GetComponent<Image>().DOFade(0f, 0f);
        selectedBar.GetComponent<Image>().enabled = false;
        selectedBar.SetActive(false);
    }

    public void Close()
    {
        stageBtn.transform.DOScale(1f, tweenTime/2f);
        stageBtn.GetComponent<Image>().DOFade((100f / 255f), tweenTime/2f);

        selectedBar.GetComponent<Image>().DOFade(0f, tweenTime/2f);
        selectedBar.GetComponent<Image>().enabled = false;
        selectedBar.SetActive(false);

        for (int i = 0; i < levelWrapper.Length; i++)
        {
            levelWrapper[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator Open()
    {
        selectedBar.SetActive(true);
        selectedBar.GetComponent<Image>().enabled = true;
        selectedBar.GetComponent<Image>().DOFade(1f, tweenTime);

        for (int i = 0; i < levelWrapper.Length; i++)
        {
            levelWrapper[i].Reset();
            levelWrapper[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < levelWrapper.Length; i++)
        {
            yield return new WaitForSeconds(0.3f);
            levelWrapper[i].Open();
        }
        yield return new WaitForSeconds(0.3f * levelWrapper.Length + 0.1f);
        game.ui.tweening = false;
        game.input.TryPointerEnterEvent();
    }
}
