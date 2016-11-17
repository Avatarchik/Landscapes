using UnityEngine;
using System.Collections;

public class FOHMovingSound : MonoBehaviour
{
    public float actionStartTime;
    public float speed;
    public Vector3 direction;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        yield return null;

        float nowTime = 0.0f;

        while(nowTime < actionStartTime)
        {
            nowTime += FOHTime.globalDeltaTime;
            yield return null;
        }

        source.Play();

        while (true)
        {
            transform.Translate(direction * FOHTime.globalDeltaTime * speed);
            yield return null;
        }
    }
}
