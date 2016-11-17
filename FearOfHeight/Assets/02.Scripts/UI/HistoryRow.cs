using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HistoryRow : FOHBehavior
{
    public GameObject[] stages;
    public GameObject[] levels;
    public GameObject[] grades;
    public GameObject date;
    public GameObject highlight;

    private FOHAccount.GameData data;

    public void SetData(FOHAccount.GameData data)
    {
        this.data = data;

        for(int i=0; i< stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
        int stagetype = (int)this.data.stageType;
        if(stagetype == 3)
        {
            stagetype = 0;
        }

        stages[stagetype].SetActive(true);


        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(false);
        }
        levels[(int)this.data.levelType - 1].SetActive(true);

        for (int i = 0; i < grades.Length; i++)
        {
            grades[i].SetActive(false);
        }
        grades[(int)this.data.star - 1].SetActive(true);

        date.GetComponent<Text>().text = this.data.date;
    }
}
