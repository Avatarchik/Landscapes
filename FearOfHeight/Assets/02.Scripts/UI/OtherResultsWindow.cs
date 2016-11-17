using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class OtherResultsWindow : FOHUIWindow
{
    public struct Page
    {
        public List<FOHAccount.GameData> gameDatas;
    }

    [SerializeField]
    private GameObject noData;

    [SerializeField]
    private GameObject navigationWindow;

    public GameObject prev;
    public GameObject next;
    public float navigationInterval;
    
    [SerializeField]
    private GameObject[] navigations;

    private int nowPageIndex = 0;

    private HistoryRow[] rows;
    private int pageCount;
    private Page[] pages;
     
    public override void Init()
    {
        base.Init();
        rows = GetComponentsInChildren<HistoryRow>();

        pageCount = (game.account.historyDatas.Count - 1) / 3 + 1;
        pages = new Page[pageCount];

        if (game.account.historyDatas.Count != 0)
        {
            navigationWindow.SetActive(true);
            noData.SetActive(false);

            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].gameDatas = new List<FOHAccount.GameData>();
                if (i >= pages.Length - 1)
                {
                    int dataCount = game.account.historyDatas.Count % 3;
                    if (dataCount == 0)
                        dataCount = 3;
                    for (int j = 0; j < dataCount; j++)
                    {
                        pages[i].gameDatas.Add(game.account.historyDatas[i * 3 + j]);
                    }
                    continue;
                }

                for (int j = 0; j < 3; j++)
                {
                    pages[i].gameDatas.Add(game.account.historyDatas[i * 3 + j]);
                }
            }
        }

        else
        {
            navigationWindow.SetActive(false);
            noData.SetActive(true);
        }
        NavigatorAlign();
        ChangePage(0);
    }

    public void ChangePage(int index)
    {
        if(index < 0)
            return;
        ActiveHighlight(index);

        if (game.account.historyDatas.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                rows[i].gameObject.SetActive(false);
            }
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            if (i > pages[index].gameDatas.Count - 1)
            {
                rows[i].gameObject.SetActive(false);
                continue;
            }

            rows[i].SetData(pages[index].gameDatas[i]);
            rows[i].gameObject.SetActive(true);
        }

        nowPageIndex = index;
    }

    public void OnBackButtonClick()
    {
        game.scene.SetState(SceneState.StageSelect);
    }

    public void OnRow1Click()
    {
        ResultWindow.selectedData = pages[nowPageIndex].gameDatas[0];
        FOHResultScene.previousScene = SceneState.OtherResults;
        game.scene.SetState(SceneState.Result);
    }

    public void OnRow2Click()
    {
        ResultWindow.selectedData = pages[nowPageIndex].gameDatas[1];
        FOHResultScene.previousScene = SceneState.OtherResults;
        game.scene.SetState(SceneState.Result);
    }
    
    public void OnRow3Click()
    {
        ResultWindow.selectedData = pages[nowPageIndex].gameDatas[2];
        FOHResultScene.previousScene = SceneState.OtherResults;
        game.scene.SetState(SceneState.Result);
    }

    private void NavigatorAlign()
    {
        for (int i = 0; i < navigations.Length; i++)
        {
            if (i >= pages.Length)
            {
                navigations[i].SetActive(false);
                continue;
            }
            navigations[i].transform.localPosition = new Vector3(navigationInterval * i - navigationInterval * pages.Length * 0.5f, 0.0f, 0.0f);
        }

        prev.transform.localPosition = new Vector3(navigations[0].transform.localPosition.x - navigationInterval * 2.5f, 0.0f, 0.0f);
        next.transform.localPosition = new Vector3(navigations[pages.Length - 1].transform.localPosition.x + navigationInterval * 2.5f, 0.0f, 0.0f);
    }

    #region NavigatorEvent

    private void ActiveHighlight(int index)
    {
        for (int i = 0; i < navigations.Length; i++)
        {
            if(i != index)
                navigations[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
        }

        navigations[index].transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    // EXIT
    public void On1Exit()
    {
        if (nowPageIndex == 0)
        {
            navigations[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On2Exit()
    {
        if (nowPageIndex == 1)
        {
            navigations[1].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On3Exit()
    {
        if (nowPageIndex == 2)
        {
            navigations[2].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On4Exit()
    {
        if (nowPageIndex == 3)
        {
            navigations[3].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On5Exit()
    {
        if (nowPageIndex == 4)
        {
            navigations[4].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On6Exit()
    {
        if (nowPageIndex == 5)
        {
            navigations[5].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On7Exit()
    {
        if (nowPageIndex == 6)
        {
            navigations[6].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On8Exit()
    {
        if (nowPageIndex == 7)
        {
            navigations[7].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On9Exit()
    {
        if (nowPageIndex == 8)
        {
            navigations[8].transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void On1Click()
    {
        ChangePage(0);
    }
    public void On2Click()
    {
        ChangePage(1);
    }
    public void On3Click()
    {
        ChangePage(2);
    }
    public void On4Click()
    {
        ChangePage(3);
    }
    public void On5Click()
    {
        ChangePage(4);
    }
    public void On6Click()
    {
        ChangePage(5);
    }
    public void On7Click()
    {
        ChangePage(6);
    }
    public void On8Click()
    {
        ChangePage(7);
    }
    public void On9Click()
    {
        ChangePage(8);
    }
    public void PrevClick()
    {
        if(nowPageIndex <= 0)
            return;
        ChangePage(--nowPageIndex);
        ActiveHighlight(nowPageIndex);
    }
    public void NextClick()
    {
        if(nowPageIndex >= pages.Length -1)
            return;
        ChangePage(++nowPageIndex);
        ActiveHighlight(nowPageIndex);
    }

    #endregion
}
