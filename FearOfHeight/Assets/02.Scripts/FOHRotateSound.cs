using UnityEngine;
using System.Collections;

public class FOHRotateSound : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
