using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class LevelWrapper : FOHBehavior
{
    public StageType stage;
    public LevelType level;
    public GameObject levelButton { get; set; }
    private float levelButtonYPos;
    private GameObject lockButton;
    private GameObject[] stars;

    private bool interactable = false;

    public int star { get; set; }
    public bool unLock { get; set; }

    protected override void Awake()
    {
        base.Awake();

        levelButton = transform.GetChild(0).gameObject;
        levelButtonYPos = levelButton.transform.localPosition.y;

        lockButton = transform.GetChild(1).gameObject;
        lockButton.SetActive(false);

        stars = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            stars[i] = transform.FindChild("Stars").GetChild(i).gameObject;
            stars[i].SetActive(false);
            stars[i].GetComponent<Image>().enabled = false;
        }

        star = 0;
        unLock = false;
    }

    private void OnEnable()
    {
        if (game.account == null)
            return;
        if (level.ToString() != LevelType.LV1.ToString())
        {
            unLock = game.account.IsUnlock(new FOHAccount.GameDataKey(stage, (LevelType)((int)level - 1)));
        }
        star = game.account.GetHighScoreData(new FOHAccount.GameDataKey(stage, level)).star;
    }
    public void Reset()
    {
        levelButton.GetComponent<Image>().DOFade(0f, 0f);
        levelButton.transform.DOLocalMoveY(50f, 0f);
        levelButton.GetComponent<Image>().enabled = false;
        levelButton.SetActive(false);
        levelButton.GetComponent<FOHUILevelTweeningButton>().interactable = false;

        lockButton.GetComponent<Image>().DOFade(0f, 0f);
        lockButton.transform.DOLocalMoveY(50f, 0f);
        lockButton.GetComponent<Image>().enabled = false;
        lockButton.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(false);
            stars[i].GetComponent<Image>().DOFade(0f, 0f);
            stars[i].GetComponent<Image>().enabled = false;
        }
    }

    public void Open()
    {
        // 잠겼으면?
        if (unLock || level == LevelType.LV1)
        {
            levelButton.SetActive(true);
            levelButton.GetComponent<Image>().enabled = true;
            levelButton.GetComponent<Image>().DOFade(100f / 255f, 0.2f * ((int)level + 1)).OnComplete(EnableInteract);
            levelButton.transform.DOLocalMoveY(levelButtonYPos, 0.2f * ((int)level + 1));
        }
        else
        {
            lockButton.SetActive(true);
            lockButton.GetComponent<Image>().enabled = true;
            lockButton.GetComponent<Image>().DOFade(100f / 255f, 0.2f * ((int)level + 1));
            lockButton.transform.DOLocalMoveY(levelButtonYPos, 0.2f * ((int)level + 1));
        }

        // 별 등장 애니메이션                
        if (star > 0)
        {
            stars[star - 1].SetActive(true);
            stars[star - 1].GetComponent<Image>().enabled = true;
            stars[star - 1].GetComponent<Image>().DOFade(1f, 0.2f).SetDelay(0.2f * ((int)level + 1));
        }
    }

    private void EnableInteract()
    {
        levelButton.GetComponent<FOHUILevelTweeningButton>().interactable = true;
    }
}

