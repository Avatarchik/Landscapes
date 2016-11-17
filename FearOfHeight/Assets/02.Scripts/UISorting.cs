using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISorting : MonoBehaviour
{
    public float total;
    public int totalStage;

    private SpriteRenderer[] levels;

    private void Awake()
    {
        levels = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        float resultX = total / (totalStage - 1);

        for (int i = 0; i < totalStage; i++)
        {
            levels[i].transform.localPosition = new Vector3(resultX * i - total * 0.5f, 0.0f , levels[i].transform.localPosition.z);
        }
    }
}
