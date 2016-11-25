using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{

#if UNITY_EDITOR

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    void Start()
    {
        Cursor.visible = false;

        Rigidbody m_Rigidbody = GetComponent<Rigidbody>();

        if (m_Rigidbody)
            m_Rigidbody.freezeRotation = true;
    }

    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            // transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(-rotationY, rotationX, 0)), 0.2f);
        }

        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }

        else if (axes == RotationAxes.MouseY)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
#endif
}
